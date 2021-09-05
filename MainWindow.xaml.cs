using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Blacklight.Controls.Wpf;

using AVBible;
using Quelle.DriverDefault;
using QuelleHMI;

using AVSDK;
using AVText;
using System.Windows.Media.TextFormatting;
using System.IO;

namespace AVBible
{
    internal class ChapterSpec
    {
        public byte Book { get; private set; }
        public byte Chapter { get; private set; }
        public byte Weight { get; private set; }

        private ChapterSpec(byte bookNum, byte chapterNum, byte weight = 0xFF)
        {
            Book = bookNum;
            Chapter = chapterNum;
            Weight = weight <= 5 ? (byte) weight : (byte) 0xFF;
        }
        public ChapterSpec GetChapterSpec(byte bookNum, byte chapterNum, UInt16[] verses)
        {
            if (verses.Length > 5)
            {
                return new ChapterSpec(bookNum, chapterNum);
            }
            else if (verses.Length > 1)
            {
                byte cnt = 0;
                for (var i = 1; i < verses.Length; i++)
                {
                    for (UInt16 bits = verses[i]; bits != 0; bits <<= 0x1)
                    {
                        if ((bits & 0x1) == 0x1)
                            cnt++;
                        if (cnt > 5)
                            return new ChapterSpec(bookNum, chapterNum);
                    }
                }
                return new ChapterSpec(bookNum, chapterNum, cnt);
            }
            else
            {
                return new ChapterSpec(bookNum, chapterNum, (byte)0);
            }
        }
        internal static bool Check(ChapterSpec left, ChapterSpec right)
        {
            var test1 = (object)left;
            var test2 = (object)right;

            return (test1 != null) && (test2 != null);
        }
        internal static bool Check(ChapterSpec single)
        {
            var test = (object)single;
            return (test != null);
        }
        public static bool operator <(ChapterSpec left, ChapterSpec right)
        {
            if (!Check(left, right))
                return false;

            return (left.Book < right.Book)
                || ((left.Book == right.Book) && (left.Chapter < right.Chapter));
        }
        public static bool operator >(ChapterSpec left, ChapterSpec right)
        {
            if (!Check(left, right))
                return false;

            return (left.Book > right.Book)
                || ((left.Book == right.Book) && (left.Chapter > right.Chapter));
        }
        public static bool operator ==(ChapterSpec left, ChapterSpec right)
        {
            if ((((object)left) == null) && (((object)right) == null))
                return true;

            if (!Check(left, right))
                return false;

            return (left.Book == right.Book) && (left.Chapter == right.Chapter);
        }
        public static bool operator !=(ChapterSpec left, ChapterSpec right)
        {
            return !(left == right);
        }
        public static bool operator <=(ChapterSpec left, ChapterSpec right)
        {
            if (!Check(left, right))
                return false;   // if both objects are null (or either is null), then fail the comparion test (== op allows both ojects to be null)

            return (left < right) || (left == right);
        }
        public static bool operator >=(ChapterSpec left, ChapterSpec right)
        {
            if (!Check(left, right))
                return false;   // if both objects are null (or either is null), then fail the comparion test (== op allows both ojects to be null)

            return (left > right) || (left == right);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //DragDockPanel.CollapseOnMinize = true;
        private int id;
        private const double narrow = 70.0;

        internal uint ViewbookStartNum;
        internal uint ChapterChickletIndex = 0;

        private (uint count, bool ok) GetBookHitCount(byte b)
        {
            var book = AVXAPI.SELF.XBook.GetBookByNum(b);
            if ((this.found != null) && book.HasValue)
            {
                UInt16 c1 = book.Value.chapterIdx;
                UInt16 cn = (UInt16) (c1 + book.Value.chapterCnt - 1);
                var chapterFirst = AVXAPI.SELF.XChapter.chapters[c1];
                var chapterLast = AVXAPI.SELF.XChapter.chapters[cn];

                UInt16 verseFirst = chapterFirst.verseIdx;
                UInt16 verseLast = (UInt16) (chapterLast.verseIdx + AVXAPI.SELF.XChapter.GetVerseCount(cn) - 1);

                var writ = new Writ176();
                var verses = from v in this.found.verses where v >= verseFirst && v <= verseLast select v;

                return ((uint) verses.Count(), true);
            }
            return (0, false);
        }
        private (uint count, bool ok) GetBookChapterHitCount(byte b, byte c)
        {
            var book = AVXAPI.SELF.XBook.GetBookByNum(b);
            if ((this.found != null) && book.HasValue && c < book.Value.chapterCnt && c >= 1)
            {
                UInt16 bc= book.Value.chapterIdx;
                var chapter = AVXAPI.SELF.XChapter.chapters[bc+c-1];

                UInt16 verseFirst = chapter.verseIdx;
                UInt16 verseLast = (UInt16)(chapter.verseIdx + AVXAPI.SELF.XChapter.GetVerseCount(bc) - 1);

                var writ = new Writ176();
                var verses = from v in this.found.verses where v >= verseFirst && v <= verseLast select v;

                return ((uint)verses.Count(), true);
            }
            return (0, false);
        }
        private (uint count, bool ok) GetBookChapterVerseHitCount(byte b, byte c, byte v)
        {
            var book = AVXAPI.SELF.XBook.GetBookByNum(b);
            if ((this.found != null) && book.HasValue && c < book.Value.chapterCnt && c >= 1 && v >= 1)
            {
                UInt16 bc = book.Value.chapterIdx;
                var chapter = AVXAPI.SELF.XChapter.chapters[bc + c - 1];

                byte vcnt = AVXAPI.SELF.XChapter.GetVerseCount((UInt16) (bc + c - 1));

                if (v <= vcnt && this.found.verses.Contains((UInt16)(chapter.verseIdx + v - 1)))
                    return (1, true);
            }
            return (0, false);
        }

        internal uint MaxiBookCnt = 0;
        internal uint MiniBookCnt = 0;

        private AVXAPI provider;
        //private InstantiatedQuelleSearchProvider iprovider;
        private QuelleDriver driver;
        private IQuelleSearchResult found;
        private async Task<Boolean> Initialize()
        {
            this.provider = new AVXAPI();
            //this.iprovider = new InstantiatedQuelleSearchProvider(provider);
            this.driver = new QuelleDriver();
            this.found = null;

            ChapterChicklet.App = this;

            return true;
        }
        public static string HelpFolder { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DigitalAV", "AV-Bible", "Help");
        public static string About { get; private set; } = "README";
        public static string Search { get; private set; } = "searching";
        public static string Instructions { get; private set; } = "instructions";
        public static Dictionary<string, string> Help { get; private set; } = new Dictionary<string, string>();
        public static Dictionary<string, string> HelpTitle { get; private set; } = new Dictionary<string, string>();
        private bool FullInit()
        {
            try
            {
                var AVInit = Initialize();
                var waiter = AVInit.GetAwaiter();

                try
                {
                    System.IO.Directory.CreateDirectory(HelpFolder);
                    Help[About] = AVMemMap.Fetch(About + ".md", HelpFolder);
                    Help[Search] = AVMemMap.Fetch(Search + ".md", HelpFolder);
                    Help[Instructions] = AVMemMap.Fetch(Instructions + ".md", HelpFolder);

                    HelpTitle[About] = "HELP - About AV Bible";
                    HelpTitle[Search] = "HELP - Searching";
                    HelpTitle[Instructions] = "HELP - User Instructions";
                }
                catch
                {
                    ;
                }

                if (waiter.GetResult())
                {
                    this.BookSelection(1);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ;
            }
            return false;
        }

        protected virtual void SaveWindowState()
        {
            try
            {
                var x = this.WindowState == WindowState.Maximized ? "1" : "0";
                var l = this.Left.ToString();
                var t = this.Top.ToString();
                var h = this.Height.ToString();
                var w = this.Width.ToString();
                var conf = ConfigurationManager.AppSettings;
                conf.Set("FormMax", x);
                conf.Set("FormLeft", l);
                conf.Set("FormTop", t);
                conf.Set("FormHeight", h);
                conf.Set("FormWidth", w);
            }
            catch
            {
                ;
            }
        }
        protected virtual void LoadWindowState()
        {
            var conf = ConfigurationManager.AppSettings;
            var x = conf.Get("FormMax");
            var l = conf.Get("FormLeft");
            var t = conf.Get("FormTop");
            var h = conf.Get("FormHeight");
            var w = conf.Get("FormWidth");

            bool maximized = x != null && x == "1";

            if (l != null)
                this.Left = int.Parse(l);
            if (t != null)
                this.Top = int.Parse(t);
            if (h != null)
                this.Height = int.Parse(h);
            if (w != null)
                this.Width = int.Parse(w);

            if (maximized)
            {
                Window_Maximize();
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
                Window_UnMaximize(null, null);
            }
        }
        protected virtual void LoadAppState()
        {
            this.ButtonAVT_Click(null, null);
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadWindowState();
            LoadAppState();

            SectionStack.SetBookSelector(this.BookSelection);

            this.provider = null;
            //this.iprovider = null;
            this.driver = null;

            ViewbookStartNum = 0;

            FullInit();

            id = 0;
        }
        public static UInt64 sequence = 0;
        public void AddHelpPanel(string topic)
        {
            string header = HelpTitle.ContainsKey(Instructions) ? HelpTitle[Instructions] : "HELP";
            string help = Help.ContainsKey(Instructions) ? Help[Instructions] : null;
            
            if (Help.ContainsKey(topic) && HelpTitle.ContainsKey(topic))
            {
                header = HelpTitle[topic];
                help = Help[topic];
            }
            DragDockPanel panel = null;
            foreach (DragDockPanel existing in this.AVPanel.Items)
            {
                if (existing.Header.ToString() == header)
                {
                    panel = existing;
                    panel.PanelLifetime = ++sequence;
                    panel.PanelReference = 0;
                    break;
                }
            }
            if (panel == null)
            {
                // Recycle the oldest panel
                //
                if (this.AVPanel.Items.Count >= 12)
                {
                    int position = -1;
                    int delete = (-1);
                    UInt16 removal = 0;
                    UInt64 min = UInt64.MaxValue;
                    foreach (var item in this.AVPanel.Items)
                    {
                        ++position;
                        var test = (DragDockPanel)item;
                        if (test.PanelLifetime < min)
                        {
                            min = test.PanelLifetime;
                            removal = test.PanelReference;
                            delete = position;
                        }
                    }
                    if (delete >= 0)
                    {
                        this.AVPanel.Items.RemoveAt(delete);
                        foreach (var item in this.ChapterStack.Children)
                        {
                            var update = (ChapterChicklet)item;
                            if (update.BookChapter == removal)
                            {
                                update.Refresh(false);
                                break;
                            }
                        }
                    }
                }
                var content = this.GetHelp(help);
                panel = new DragDockPanel();
                panel.Content = content;
                panel.PanelLifetime = ++sequence;
                panel.PanelReference = 0;
                panel.Header = header;
                this.AVPanel.Items.Add(panel);

                ResetComboDeleteItems();
            }
        }
        public void AddPanel(ChapterChicklet chicklet)
        {
            int bk = chicklet.BookChapter >> 8;
            int ch = chicklet.BookChapter & 0xFF;
            var book = AVXAPI.SELF.XBook.GetBookByNum((byte)bk).Value;
            string header = book.name + " " + ch.ToString();
            if (this.ButtonAVX.Content.ToString() == "AVX")
                header += " (AVX)";

            DragDockPanel panel = null;
            foreach (DragDockPanel existing in this.AVPanel.Items)
            {
                if (existing.Header.ToString() == header)
                {
                    panel = existing;
                    panel.PanelLifetime = ++sequence;
                    panel.PanelReference = chicklet.BookChapter;
                    break;
                }
            }
            if (panel == null)
            {
                // Recycle the oldest panel
                //
                if (this.AVPanel.Items.Count >= 12)
                {
                    int position = -1;
                    int delete = (-1);
                    UInt16 removal = 0;
                    UInt64 min = UInt64.MaxValue;
                    foreach (var item in this.AVPanel.Items)
                    {
                        ++position;
                        var test = (DragDockPanel) item;
                        if (test.PanelLifetime < min)
                        {
                            min = test.PanelLifetime;
                            removal = test.PanelReference;
                            delete = position;
                        }
                    }
                    if (delete >= 0)
                    {
                        this.AVPanel.Items.RemoveAt(delete);
                        foreach (var item in this.ChapterStack.Children)
                        {
                            var update = (ChapterChicklet)item;
                            if (update.BookChapter == removal)
                            {
                                update.Refresh(false);
                                break;
                            }                                
                        }
                    }
                }
                var content = this.GetChapter(bk, ch);
                panel = new DragDockPanel();
                panel.Content = content;
                panel.PanelLifetime = ++sequence;
                panel.PanelReference = chicklet.BookChapter;
                //view = new ViewPanel(panel, content, 1);
                panel.Header = header;
                this.AVPanel.Items.Add(panel);
            }
            ResetComboDeleteItems();
        }

        private void ResetComboDeleteItems()
        {
            comboBoxDeletePanel.Items.Clear();

            foreach (DragDockPanel existing in this.AVPanel.Items)
            {
                comboBoxDeletePanel.Items.Add(existing.Header.ToString());
            }
        }

        private uint ChapterViewMax
        {
            get
            {
                double width = this.ActualWidth;
                if (width < this.MinWidth)
                {
                    if (this.Width > this.MinWidth)
                        width = this.Width;
                    else
                        width = this.MinWidth;
                }
                /*
                if (this..BookStack.Visibility == Visibility.Visible)
                    width -= this.BookStack.Picture1.Width;
                else if (this.MiniBookStack.Visibility == Visibility.Visible)
                    width -= this.MiniBookStack.Picture1.ActualWidth;
                */
                uint panelCnt;
                for (panelCnt = 5; panelCnt > 1; --panelCnt)
                {
                    uint panelSize = ((uint)width) / panelCnt;
                    if (panelSize > 260)
                        break;
                }
                uint chickletCnt = panelCnt * 2;

                return chickletCnt;
            }
        }

        internal void SetChapterStackParams(uint max)
        {
            uint size = ChapterViewMax;

            uint small = size;
            uint large = size * 2;
            if (large > max - size)
            {
                large = max - size;

                if (large < 1)
                    large = 1;
            }
            if (small > max - size)
            {
                small = max - size;
                if (small >= large)
                    small = large / 2;
                if (small < 1)
                    small = 1;
            }
        }
        internal bool HtmlControlReady(FlowDocumentScrollViewer control)
        {
            return control.IsInitialized;
        }
        internal void LoadHtmlContent(FlowDocumentScrollViewer control, string html)
        {
            for (bool now = HtmlControlReady(control); !now; now = this.HtmlControlReady(control))
            {
                ;
            }
            var r = new System.Windows.Documents.Run(html);
            var p = new System.Windows.Documents.Paragraph(r);
            control.Document.Blocks.Add(p);
        }

        private void ClearChapterChicklets()
        {
            ChapterChickletIndex = 0;

            this.AVPanel.Items.Clear();
            this.AVPanel.MaxRows = 4;
            this.AVPanel.MaxColumns = 3;
        }
        private ChapterChicklet InitNextChapterChicklet()
        {
            ChapterChicklet chicklet = null;

            if (ChapterChickletIndex < this.ChapterViewMax)
            {
                switch (++ChapterChickletIndex)
                {

                }
            }
            return chicklet;
        }

        private void GetFontStrings(out string prefix, out string suffix)
        {
            prefix = "<font size='5' face='calibre'>";
            suffix = "</font>";
        }

        private bool GetPageHtml(out string prefix, out string suffix, string bookName, int b, int c, int v)
        {
            bool header = (b >= 1) && (b <= 66);
            bool script = header && (v >= 1);

            string font;
            string fontSuffix;
            GetFontStrings(out font, out fontSuffix);

            if (header || script)
            {
                prefix = "<html>";
                suffix = fontSuffix;

                if (header)
                {
                    prefix += "<head><title>";
                    prefix += bookName;
                    if (v != 0)
                        prefix += c.ToString() + ":" + v.ToString();
                    else
                        prefix += c.ToString();

                    prefix += "</title></head>";
                }
                prefix += "<body>";
                if (script)
                {
                    string anchor = "v" + v.ToString();

                    prefix += "<script language=\"javascript\">";

                    prefix += "window.onload=function()";
                    prefix += "{";
                    prefix += "document.getElementsByName('";
                    prefix += anchor;
                    prefix += "')[0].focus();";
                    prefix += "}";

                    prefix += "</script>";
                }
                prefix += font;
                suffix += "</body>";
                suffix += "</html>";
            }
            else
            {
                prefix = font;
                suffix = fontSuffix;
            }
            return true;
        }

        private FontFamily panel_fontFamily = new FontFamily("calibri");
        private int panel_fontSize = 16;
        private int panel_fontHead = 20;

        private string PostPunc(byte punc)
        {
            switch (punc & 0xE0)
            {
                case 0x80: return "!";
                case 0xC0: return "?";
                case 0xE0: return ".";
                case 0xA0: return "-";
                case 0x20: return ";";
                case 0x40: return ",";
                case 0x60: return ":";
                default:   return  "";
            }
        }
        FlowDocumentScrollViewer GetChapter(int b, int c, bool header = false, string bookName = null)
        {
            var doc = new System.Windows.Documents.FlowDocument();
            doc.FontSize = this.panel_fontSize;
            doc.FontFamily = this.panel_fontFamily;
            doc.Foreground = new SolidColorBrush(Colors.White);

            if (header)
            {
                var rhead = new System.Windows.Documents.Run(bookName + " " + c.ToString());
                var phead = new System.Windows.Documents.Paragraph(rhead);
                phead.FontSize = this.panel_fontHead;
                phead.FontWeight = FontWeights.Bold;
                doc.Blocks.Add(phead);
            }
            var book = AVXAPI.SELF.XBook.GetBookByNum((byte)b).Value;
            var chapterIdx = book.chapterIdx + c - 1;
            AVSDK.Chapter chapter = AVXAPI.SELF.Chapters[chapterIdx];
            UInt32 first = chapter.writIdx;
            UInt32 last = (UInt32)(first + chapter.wordCnt - 1);


            HashSet<UInt16> verses;
            HashSet<UInt32> tokens;
            var records = new Dictionary<UInt32, UInt16>(); // <widx, wcnt>

            var writ = new Writ176();
            if (this.found != null)
            {
                tokens = this.found.tokens;
                verses = this.found.verses;
            }
            else
            {
                tokens = new HashSet<UInt32>();
                verses = new HashSet<UInt16>();
            }
            byte v = 0;

            var pdoc = new System.Windows.Documents.Paragraph();

            bool paren = false;
            bool bov = false;

            for (var cursor = first; cursor <= last && AVXAPI.SELF.XWrit.GetRecord(cursor, ref writ); AVXAPI.SELF.XWrit.Next(), cursor = AVXAPI.SELF.XWrit.cursor)
            {
                string vstr = "";
                string postPunc = "";

                bov = ((writ.trans & (byte)AVSDK.Transitions.VerseTransition) == (byte)AVSDK.Transitions.BeginingOfVerse);

                if (bov)
                {
                    ++v;
                    if (vstr.Length > 0)
                    {
                        var vdoc = new System.Windows.Documents.Run(vstr);
                        pdoc.Inlines.Add(vdoc);
                    }
                    string padding = (first == cursor) ? "" : "  ";
                    var vlabel = new System.Windows.Documents.Run(padding + ((int)v).ToString());
                    vlabel.Foreground = Brushes.Cyan;
                    pdoc.Inlines.Add(vlabel);
                    vstr = " ";
                }
                else if (first != cursor)
                {
                    vstr += " ";
                }

                bool jesus = (writ.punc & 0x01) != 0;
                bool italics = ((writ.punc & 0x02) != 0);
                bool avx = (this.ButtonAVX.Content.ToString() == "AVX");
                string lex = "";
                if (((writ.punc & 0x04) != 0) && !paren)
                {
                    paren = true;
                    pdoc.Inlines.Add("(");
                }
                lex += avx ? AVLexicon.GetLexModern(writ.word) : AVLexicon.GetLex(writ.word);
                if ((writ.punc & 0x10) != 0)
                {
                    bool s = (lex[lex.Length - 1] | 0x20) == 's';
                    lex += s ? "'" : "'s";
                }
                if ((writ.punc & 0x0C) == 0x0C)
                {
                    paren = false;
                    postPunc = ")";
                }
                if ((writ.punc & 0xE0) != 0)
                {
                    postPunc += this.PostPunc(writ.punc);
                }
                if (vstr.Length > 0)
                {
                    var vdoc = new System.Windows.Documents.Run(vstr);
                    pdoc.Inlines.Add(vdoc);
                }
                lex += postPunc;
                postPunc = "";

                var phrase = new System.Windows.Documents.Run(lex);
                if (italics)
                {
                    phrase.FontStyle = FontStyles.Italic;
                }
                if (verses.Contains(writ.verseIdx))
                {
                     phrase.FontWeight = FontWeights.Bold;

                    if (tokens.Contains(cursor))
                    {
                        phrase.Background = Brushes.LightCyan;
                        phrase.Foreground = Brushes.Black;
                    }
                    else
                    {
                        phrase.Foreground = Brushes.Cyan;
                    }
                }
                else
                {
                    phrase.Foreground = Brushes.White;
                    phrase.FontWeight = FontWeights.Normal;
                }
                /*
                if (jesus)
                {
                    phrase.Foreground = Brushes.Maroon;
                }
                */
                pdoc.Inlines.Add(phrase);

                if (postPunc.Length > 0)
                {
                    var punc = new System.Windows.Documents.Run(postPunc);
                    if (verses.Contains(writ.verseIdx))
                    {
                        phrase.Foreground = Brushes.Cyan;
                        punc.FontWeight = FontWeights.Bold;
                    }
                    pdoc.Inlines.Add(punc);
                }
            }
            doc.Blocks.Add(pdoc);

            var scrolling = new FlowDocumentScrollViewer();
            scrolling.Document = doc;
            return scrolling;
        }
        FlowDocumentScrollViewer GetHelp(string md)   // MarkDown file
        {
            var doc = new System.Windows.Documents.FlowDocument();

            var style = new Style(typeof(System.Windows.Documents.Paragraph));
            style.Setters.Add(new Setter(System.Windows.Documents.Block.MarginProperty, new Thickness(0)));
            doc.Resources.Add(typeof(System.Windows.Documents.Paragraph), style);

            doc.FontSize = this.panel_fontSize;
            doc.FontFamily = this.panel_fontFamily;
            doc.Foreground = new SolidColorBrush(Colors.White);

            if (md != null && File.Exists(md))
            {
                var input = new FileStream(md, FileMode.Open, FileAccess.Read);
                var reader = new StreamReader(input);

                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    // Eventually, we might differentiate between different header levels
                    if (line.StartsWith("###"))
                    {
                        var index = line.LastIndexOf('#');
                        var rhead = new System.Windows.Documents.Run(line.Substring(index+1).Trim());
                        var phead = new System.Windows.Documents.Paragraph(rhead);
                        phead.Foreground = Brushes.Green;
                        phead.FontSize = this.panel_fontHead;
                        phead.FontWeight = FontWeights.Bold;
                        doc.Blocks.Add(phead);
                    }
                    else if (line.StartsWith("##"))
                    {
                        var rhead = new System.Windows.Documents.Run(line.Substring(2).Trim());
                        var phead = new System.Windows.Documents.Paragraph(rhead);
                        phead.FontSize = this.panel_fontHead;
                        phead.FontWeight = FontWeights.Bold;
                        doc.Blocks.Add(phead);
                    }
                    else if (line.StartsWith("#"))
                    {
                        var rhead = new System.Windows.Documents.Run(line.Substring(1).Trim());
                        var phead = new System.Windows.Documents.Paragraph(rhead);
                        phead.FontSize = this.panel_fontHead;
                        phead.FontWeight = FontWeights.Bold;
                        doc.Blocks.Add(phead);
                    }
                    else
                    {
                        var breaks = line.Split("<br/>", StringSplitOptions.None);
                        foreach (string paragraph in breaks)
                        {
                            var stripped = paragraph.Replace("*", "");
                            var pdoc = new System.Windows.Documents.Paragraph();
                            var vdoc = new System.Windows.Documents.Run(stripped);
                            pdoc.Inlines.Add(vdoc);
                            doc.Blocks.Add(pdoc);
                        }
                    }
                }
                reader.Close();
                input.Close();
            }
            var scrolling = new FlowDocumentScrollViewer();
            scrolling.Document = doc;
            return scrolling;
        }

        private static bool RezeroImage(Image image, string zero)
        {
            string src = image.Source.ToString();
            if (!src.EndsWith(zero))
            {
                int newLen = 0;
                if (zero == "-0.png")
                    newLen = src.Length - "-00.png".Length;
                else if (zero == "-00.png")
                    newLen = src.Length - "-0.png".Length;
                if (newLen > 0)
                {
                    var uri = new Uri(src.Substring(0, newLen) + zero);
                    image.Source = new BitmapImage(uri);
                    return true;
                }
            }
            return false;
        }
        public Boolean ShowAll { get; private set; }

        private void ResetChapterView(ChapterSpec spec)
        {
            SetSearchView(); // used to be SetChapterViewViaSearch(index, reset)
        }
        (bool success, HMICommand hmi, IQuelleSearchResult result) QuelleCommand(string text)
        {
            HMICommand command = new HMICommand(text.Replace('+', ';')); // allow plus to be used to delimit search segments

            if (command.statement != null && command.statement.segmentation != null && command.statement.segmentation.Count >= 1 && command.errors.Count == 0)
            {
                var result = command.statement.ExecuteEx(this.provider);

                if (result != null)
                {
                    foreach (var message in command.warnings)
                    {
                        Console.WriteLine("WARNING: " + message);
                    }
                    this.found = result;
                    return (true, command, result);
                }
                else
                {
                    foreach (var message in command.errors)
                    {
                        Console.WriteLine("ERROR: " + message);
                    }
                }
            }
            else
            {
                Console.WriteLine("error: " + "Statement is not expected to be null; Quelle driver implementation error");
            }
            return (false, command, null);
        }
        private void SetEntireView(byte bk)
        {
            this.ChapterView.Visibility = Visibility.Visible;

            ChapterStack.Children.Clear();
            var cnt = (bk >= 1 && bk <= 66) ? AVXAPI.SELF.XBook.GetBookByNum(bk).Value.chapterCnt : (byte)0;
            for (byte c = 1; c <= cnt; c++)
                AddChapterChicklet(bk, c);
        }
        private void AddChapterChicklet(byte b, byte c)
        {
            bool green = false;
            UInt16 encoded = (UInt16)((b << 8) + c);

            foreach (var item in this.AVPanel.Items)
            {
                var test = (DragDockPanel)item;
                if (test.PanelReference == encoded)
                {
                    green = true;
                    break;
                }
            }
            var matches = this.found != null ? this.GetBookChapterHitCount(b, c).count : 0;
            var weight = matches <= 6 ? (byte)matches : (byte) 6;
            var chicklet = new ChapterChicklet(b, c, weight, green);
            this.ChapterStack.Children.Add(chicklet);
        }
        private void SetSearchView(int index = 0, bool reset = true)
        {
            this.ChapterView.Visibility = Visibility.Visible;

            ChapterStack.Children.Clear();

            var command = QuelleCommand(this.TextCriteria.Text);

            var verses = new HashSet<UInt16>();
            if (command.success && command.result != null && command.result.verses != null && command.result.verses.Count > 0)
            {
                byte bkLast = 0;
                byte chLast = 0;

                byte bk;
                byte ch;
                byte vs;
                byte ignore;

                foreach (UInt16 vidx in from verse in command.result.verses orderby verse select verse)
                {
                    if (!AVXAPI.SELF.XVerse.GetEntry(vidx, out bk, out ch, out vs, out ignore))
                        return; // something unexpected went wrong

                    if (bk == bkLast && ch == chLast)
                        continue;

                    bkLast = bk;
                    chLast = ch;

                    AddChapterChicklet(bk, ch);
                }
            }
        }
        private void TextCriteria_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                //              RadioShowSearch.IsChecked = true;
                ShowAll = false;
                SetSearchView();
                e.Handled = true;
            }
            else if (e.Key == Key.F12)
            {
                if (this.SearchImage.Visibility != Visibility.Visible)
                {
                    this.TextCriteria.Margin = new Thickness(TextCriteria.Margin.Left, TextCriteria.Margin.Top, 68, TextCriteria.Margin.Bottom);
                    this.SearchImage.Visibility = Visibility.Visible;
                }
                else
                {
                    this.TextCriteria.Margin = new Thickness(TextCriteria.Margin.Left, TextCriteria.Margin.Top, 20, TextCriteria.Margin.Bottom);
                    this.SearchImage.Visibility = Visibility.Collapsed;
                }
                e.Handled = true;
            }
        }
        private void Search_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e != null)
                e.Handled = true;

            //          RadioShowSearch.IsChecked = true;
            this.ShowAll = false;
            SetSearchView(0, true);
        }
        private void Search_FingerUp(object sender, ManipulationCompletedEventArgs e)
        {
            if (e != null)
                e.Handled = true;

            Search_MouseUp(sender, null);
        }

        private void LabelX_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = new SolidColorBrush(Colors.Silver);
            label.FontWeight = FontWeights.Normal;
        }

        private void LabelX_MouseMove(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = new SolidColorBrush(Colors.White);
            label.FontWeight = FontWeights.Bold;
        }

        private void Window_UnMaximize(object sender, MouseButtonEventArgs e) // (Char) 0xF036
        {
            this.ResizeMode = ResizeMode.CanResize;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Normal;
            LabelUnMax.Visibility = Visibility.Collapsed;
            LabelMin.Visibility = Visibility.Collapsed;
            LabelX.Visibility = Visibility.Collapsed;

            SaveWindowState();
        }
        private void Finger_UnMaximize(object sender, ManipulationCompletedEventArgs e)
        {
            if (e != null)
                e.Handled = true;

            this.Window_UnMaximize(sender, null);
        }

        private void Window_Maximize()
        {
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.WindowState = System.Windows.WindowState.Maximized;
            LabelUnMax.Visibility = Visibility.Visible;
            LabelMin.Visibility = Visibility.Visible;
            LabelX.Visibility = Visibility.Visible;

            SaveWindowState();
        }

        private void Window_Minimize(object sender, MouseButtonEventArgs e)  //  (Char) 0xF035
        {
            if (e != null)
                e.Handled = true;

            this.WindowState = System.Windows.WindowState.Minimized;
        }
        private void Finger_Minimize(object sender, ManipulationCompletedEventArgs e)
        {
            if (e != null)
                e.Handled = true;

            this.Window_Minimize(sender, null);
        }

        private void LabelX_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e != null)
                e.Handled = true;

            Close();
        }
        private void LabelX_FingerUp(object sender, ManipulationCompletedEventArgs e)
        {
            if (e != null)
                e.Handled = true;

            LabelX_MouseUp(sender, null);
        }

        private void ColumnCompactor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Visibility toggle = this.BookSectionView.Visibility;

            if (toggle == Visibility.Visible)
            {
                toggle = Visibility.Collapsed;
                ColumnCompactor.Content = "";
            }
            else
            {
                toggle = Visibility.Visible;
                ColumnCompactor.Content = "";
            }

            BookSectionView.Visibility = toggle;
            ShowFilter.Visibility = toggle;
            ShowBigBooks.Visibility = toggle;

            var x = this.ChapterViewMax;   // if window tiles were to change, it would be here

            if (e != null)
                e.Handled = true;
        }

        private void ColumnCompactor_FingerUp(object sender, ManipulationCompletedEventArgs e)
        {
            if (e != null)
                e.Handled = true;

            this.ColumnCompactor_MouseUp(sender, null);
        }

        private void MainWin_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                this.Window_Maximize();
            }
        }

        private void MainWin_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var x = this.ChapterViewMax;   // if window tiles were to change, it would be here
        }
        public void LessChapterHelper_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.ChapterView.Height > 2*110)
            {
                this.ChapterView.Height = 2*110;
                this.ChapterHelperUp.Visibility = Visibility.Visible;
            }
            else
            {
                this.ChapterView.Height = 110;
                this.ChapterHelperDown.Visibility = Visibility.Collapsed;
                this.ChapterHelperUp.Visibility = Visibility.Visible;
                this.ChapterHelperMin.Visibility = Visibility.Visible;
            }

            if (e != null)
                e.Handled = true;
        }
        public void LessChapterHelper_FingerUp(object sender, ManipulationCompletedEventArgs e)
        {
            this.OpenChapterHelper_MouseUp(sender, null);

            if (e != null)
                e.Handled = true;
        }
        public void OpenChapterHelper_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ChapterView.Visibility == Visibility.Visible)
            {
                if (this.ChapterView.Height < 2*110)
                {
                    this.ChapterView.Height = 2*110;
                    this.ChapterHelperDown.Visibility = Visibility.Visible;
                    this.ChapterHelperMin.Visibility = Visibility.Collapsed;
                }
                else if (this.ChapterView.Height < 3*110)
                {
                    this.ChapterView.Height = 3*119;
                    this.ChapterHelperDown.Visibility = Visibility.Visible;
                    this.ChapterHelperUp.Visibility = Visibility.Collapsed;
                    this.ChapterHelperMin.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                this.ChapterView.Visibility = Visibility.Visible;
                this.ChapterHelperMin.Visibility = Visibility.Visible;
            }
            if (e != null)
                e.Handled = true;
        }
        public void OpenChapterHelper_FingerUp(object sender, ManipulationCompletedEventArgs e)
        {
            this.OpenChapterHelper_MouseUp(sender, null);

            if (e != null)
                e.Handled = true;
        }
        public void CloseChapterHelper_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.ChapterView.Visibility = Visibility.Collapsed;
            this.ChapterHelperDown.Visibility = Visibility.Collapsed;
            this.ChapterHelperUp.Visibility = Visibility.Visible;
            this.ChapterHelperMin.Visibility = Visibility.Collapsed;
            this.ChapterView.Height = 110;

            if (e != null)
                e.Handled = true;
        }
        public void CloseChapterHelper_FingerUp(object sender, ManipulationCompletedEventArgs e)
        {
            this.CloseChapterHelper_MouseUp(sender, null);

            if (e != null)
                e.Handled = true;
        }
        private void BookSelection(uint bookNum)
        {
            this.ShowAll = true;
            SetEntireView((byte)bookNum);
        }

        private void MainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveWindowState();
        }

        private void TextCriteria_FingerUp(object sender, ManipulationCompletedEventArgs e)
        {
            //  This indicates a touch-enabled tablet
            //
            if (this.SearchImage.Visibility != Visibility.Visible)
            {
                this.TextCriteria.Margin = new Thickness(TextCriteria.Margin.Left, TextCriteria.Margin.Top, 68, TextCriteria.Margin.Bottom);
                this.SearchImage.Visibility = Visibility.Visible;
            }
            if (e != null)
                e.Handled = true;
        }

        private void ButtonAVT_Click(object sender, RoutedEventArgs e)
        {
            string version;

            var conf = ConfigurationManager.AppSettings;
            if (sender != null)
            {
                if ((string)ButtonAVX.Content == "AV")
                {
                    ButtonAVX.Content = version = "AVX";
                }
                else
                {
                    ButtonAVX.Content = version = "AV";
                }
                //				conf.BibleVersion = version;
            }
            else
            {
                ////			version = conf.BibleVersion;
            }
            ////			ButtonAVT.Content = version;
            //	TO DO:
            //
            //	set appropriate version here.
        }

        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void ChapterStack_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BookStack_Loaded(object sender, RoutedEventArgs e)
        {

        }
        /*
        private void RadioChapters_Changed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.RadioSearch.IsChecked.HasValue && this.RadioSearch.IsChecked.Value)
                {
                    this.ChapterSearchView.Visibility = Visibility.Visible;
                    this.ChapterBookView.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.ChapterBookView.Visibility = Visibility.Visible;
                    this.ChapterSearchView.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                ;
            }
        }
        */
        private void comboBoxHelpPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxHelpPanel.SelectedItem != null)
            {
                var help = (ComboBoxItem) (comboBoxHelpPanel.SelectedItem);

                foreach (var topic in Help.Keys)
                    if (help.Name.Equals(topic, StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.AddHelpPanel(topic);
                        break;
                    }
            }
        }
        private void comboBoxDeletePanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboBoxDeletePanel.SelectedItem != null)
            {
                var selected = this.comboBoxDeletePanel.SelectedItem.ToString();
                DragDockPanel panel = null;
                UInt16 encoding = 0;
                foreach (DragDockPanel existing in this.AVPanel.Items)
                {
                    var header = existing.Header.ToString();
                    if (header == selected)
                    {
                        panel = existing;
                        encoding = panel.PanelReference;
                        break;
                    }
                }
                if (panel != null)
                {
                    this.AVPanel.Items.Remove(panel);
                    this.comboBoxDeletePanel.Items.Remove(selected);
                }
                foreach (var item in this.ChapterStack.Children)
                {
                    var chicklet = (ChapterChicklet)item;
                    if (chicklet.BookChapter == encoding)
                        chicklet.Refresh(false);
                }
            }
        }
    }
}