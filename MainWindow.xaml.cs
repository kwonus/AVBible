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

using AVWord.Wpf;
using Quelle.DriverDefault;
using QuelleHMI;

using AVSDK;
using AVXCLI;

namespace AVWord.App
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
            if (this.found != null)
            {
                var verses = new HashSet<UInt16>();
                var writ = new Writ176();
                var orderedMatches = this.found.segments.OrderBy(elements => elements & 0xFFFFFFFF);

                foreach (var elements in orderedMatches)
                {
                    UInt32 widx = SegmentElement.GetStart(elements);
                    UInt32 wend = SegmentElement.GetEnd(elements);

                    for (UInt32 element = widx; element <= wend; element ++)
                    {
                        if (!AVXCLI.AVLCLR.XWrit.GetRecord(element, ref writ))
                            return (0, false); // something unexpected went wrong
                        var vidx = writ.verseIdx;
                        byte bk = AVXCLI.AVLCLR.XVerse.GetBook(vidx);
                        if (bk == b)
                        {
                            if (!verses.Contains(vidx))
                                verses.Add(vidx);
                        }
                        if (bk > b)
                        {
                            return ((uint)verses.Count(), true);
                        }
                    }
                }
                return ((uint)verses.Count(), true);
            }
            return (0, false);
        }
        private (uint count, bool ok) GetBookChapterHitCount(byte b, byte c)
        {
            if (this.found != null)
            {
                var verses = new HashSet<UInt16>();
                var writ = new Writ176();
                var orderedMatches = this.found.segments.OrderBy(elements => elements & 0xFFFFFFFF);

                byte bk;
                byte ch;
                byte vs;
                byte ignore;

                foreach (var elements in orderedMatches)
                {
                    UInt32 widx = SegmentElement.GetStart(elements);
                    UInt32 wend = SegmentElement.GetEnd(elements);

                    for (UInt32 element = widx; element <= wend; element++)
                    {
                        if (!AVXCLI.AVLCLR.XWrit.GetRecord(element, ref writ))
                            return (0, false); // something unexpected went wrong
                        var vidx = writ.verseIdx;
                        if (!AVXCLI.AVLCLR.XVerse.GetEntry(vidx, out bk, out ch, out vs, out ignore))
                            return (0, false); // something unexpected went wrong

                        if (bk == b && ch == c)
                        {
                            if (!verses.Contains(vidx))
                                verses.Add(vidx);
                        }
                        if ((bk > b) || ((bk == b) && (ch > c)))
                        {
                            return ((uint)verses.Count(), true);
                        }
                    }
                }
                return ((uint)verses.Count(), true);
            }
            return (0, false);
        }
        private (uint count, bool ok) GetBookChapterVerseHitCount(byte b, byte c, byte v)
        {
            if (this.found != null)
            {
                var verses = new HashSet<UInt16>();
                var writ = new Writ176();
                var orderedMatches = this.found.segments.OrderBy(elements => elements & 0xFFFFFFFF);

                byte bk;
                byte ch;
                byte vs;
                byte ignore;

                foreach (var elements in orderedMatches)
                {
                    UInt32 widx = SegmentElement.GetStart(elements);
                    UInt32 wend = SegmentElement.GetEnd(elements);

                    for (UInt32 element = widx; element <= wend; element++)
                    {
                        if (!AVXCLI.AVLCLR.XWrit.GetRecord(element, ref writ))
                            return (0, false); // something unexpected went wrong
                        var vidx = writ.verseIdx;
                        if (!AVXCLI.AVLCLR.XVerse.GetEntry(vidx, out bk, out ch, out vs, out ignore))
                            return (0, false); // something unexpected went wrong

                        if (bk == b && ch == c && vs == v)
                        {
                            if (!verses.Contains(vidx))
                                verses.Add(vidx);
                        }
                        if (bk == b)
                        {
                            if (ch == c)
                            {
                                if (vs > v)
                                    return ((uint)verses.Count(), true);
                            }
                            else if (ch > c)
                            {
                                return ((uint)verses.Count(), true);
                            }
                        }
                        else if (bk > b)
                        {
                            return ((uint)verses.Count(), true);
                        }
                    }
                }
                return ((uint)verses.Count(), true);
            }
            return (0, false);
        }
        private HashSet<UInt16> ChapterHits
        {
            get
            {
                var verses = new HashSet<UInt16>();

                if (this.found != null)
                {
                    var writ = new Writ176();

                    foreach (var elements in found.segments)
                    {
                        UInt32 widx = SegmentElement.GetStart(elements);
                        UInt32 wend = SegmentElement.GetEnd(elements);

                        for (UInt32 element = widx; element <= wend; element++)
                        {
                            if (!AVXCLI.AVLCLR.XWrit.GetRecord(element, ref writ))
                                break; // something unexpected went wrong
                            var vidx = writ.verseIdx;

                            if (!verses.Contains(vidx))
                                verses.Add(vidx);
                        }
                    }
                }
                return verses;
            }
        }

        internal uint MaxiBookCnt = 0;
        internal uint MiniBookCnt = 0;

        //internal string SDK = @"S:\src\SDK\2018\Digital-AV";  // AV-Writ.DX8
        internal string SDK = @"C:\src\Digital-AV\z-series";

        private bool GetFile(string name, string dir, string url = "http://www.avtext.org/SDK", int minKB = 87, int maxKB = 6170, string type = "application/octet-stream")
        {
            string file = System.IO.Path.Combine(dir, name);
            bool exists = System.IO.File.Exists(file);

            if (!exists)
            {
                string web = url + "/" + name;
                var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(web);
                request.Method = "GET";
                request.ContentType = type;
                try
                {
                    var res = (System.Net.HttpWebResponse)request.GetResponse();
                    using (var s = res.GetResponseStream())
                    {
                        int get = maxKB * 1024;
                        var content = new byte[get];
                        int len = 0;
                        for (int read = s.Read(content, 0, get); read > 0; read = s.Read(content, len, get))
                        {
                            get -= read;
                            len += read;
                        }

                        if (len >= minKB * 1024)
                        {
                            System.IO.File.WriteAllBytes(file, content.Take(len).ToArray());
                            exists = true;
                        }
                    }
                    res.Close();
                }
                catch (Exception ex)
                {
                    exists = false;
                }
            }
            return exists;
        }
        private AVXCLI.AVLCLR provider;
        private InstantiatedQuelleSearchProvider iprovider;
        private QuelleDriver driver;
        private IQuelleSearchResult found;
        private async Task<Boolean> Initialize()
        {
            this.provider = new AVXCLI.AVLCLR();
            this.iprovider = new InstantiatedQuelleSearchProvider(provider);
            this.driver = new QuelleDriver();
            this.found = null;

            ChapterChicklet.App = this;

            return true;
        }

        private bool FullInit()
        {
            try
            {
                var AVInit = Initialize();
                var waiter = AVInit.GetAwaiter();
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

            this.provider = null;
            this.iprovider = null;
            this.driver = null;

            ViewbookStartNum = 0;

            BookStack.Selection = this.BookSelection;

            FullInit();

            id = 0;
        }
        public static UInt64 sequence = 0;
        public void AddPanel(ChapterChicklet chicklet)
        {
            int bk = chicklet.BookChapter >> 8;
            int ch = chicklet.BookChapter & 0xFF;
            string header = AVXCLI.AVLCLR.GetBookByNum((byte)bk) + " " + ch.ToString();

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
        private SolidColorBrush panel_fontColor = new SolidColorBrush(Colors.White);
        private SolidColorBrush panel_verseColor = new SolidColorBrush(Colors.Cyan);
        private SolidColorBrush panel_redLetter = new SolidColorBrush(Colors.LightCyan);
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
            AVSDK.Book book = (AVSDK.Book) (AVLCLR.GetBookByNum((byte)b));
            var chapterIdx = book.chapterIdx + c - 1;
            AVSDK.Chapter chapter = AVLCLR.Chapters[chapterIdx];
            UInt32 first = chapter.writIdx;
            UInt32 last = (UInt32)(first + chapter.wordCnt - 1);

            int v = 0;

            var pdoc = new System.Windows.Documents.Paragraph();

            bool paren = false;
            bool bov = false;

            var writ = new Writ176();

            for (var cursor = first; cursor <= last && AVXCLI.AVLCLR.XWrit.GetRecord(cursor, ref writ); AVXCLI.AVLCLR.XWrit.Next(), cursor = AVXCLI.AVLCLR.XWrit.cursor)
            {
                string vstr = "";
                string prePunc = "";
                string postPunc = "";

                bov = ((writ.trans & (byte)AVSDK.Transitions.VerseTransition) == (byte)AVSDK.Transitions.BeginingOfVerse);

                if (bov)
                {
                    ++v;
                    if (vstr.Length > 0)
                    {
                        var vdoc = new System.Windows.Documents.Run(vstr);
                        pdoc.Inlines.Add(vdoc);
                        vstr = "";
                    }
                    string padding = (first == cursor) ? "" : "  ";
                    var vlabel = new System.Windows.Documents.Run(padding + v.ToString());
                    vlabel.Foreground = this.panel_verseColor;
                    pdoc.Inlines.Add(vlabel);
                    vstr = " ";
                }
                else if (first != cursor)
                {
                    vstr += " ";
                }

                bool jesus = (writ.punc & 0x01) != 0;
                bool italics = ((writ.punc & 0x02) != 0);
                bool avx = (this.ButtonAVT.Content.ToString() == "AVX");
                string lex = "";
                if (((writ.punc & 0x04) != 0) && !paren)
                {
                    paren = true;
                    prePunc = "(";
                }
                byte modern = avx ? (byte)2 : (byte)1;  // modern == 2; kjv == 1;
                lex += AVLCLR.GetLexicalEntry(writ.word, modern);
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
                if (italics)
                {
                    var phrase = new System.Windows.Documents.Run(lex);
                    phrase.FontStyle = FontStyles.Italic;
                    pdoc.Inlines.Add(phrase);
                }
                else if (jesus)
                {
                    var phrase = new System.Windows.Documents.Run(lex);
                    phrase.Foreground = panel_redLetter;
                    pdoc.Inlines.Add(phrase);
                }
                else
                {
                    lex += postPunc;
                    postPunc = "";

                    var phrase = new System.Windows.Documents.Run(lex);
                    pdoc.Inlines.Add(phrase);
                }
                if (postPunc.Length > 0)
                {
                    var punc = new System.Windows.Documents.Run(postPunc);
                    pdoc.Inlines.Add(punc);
                }
            }
            doc.Blocks.Add(pdoc);

            var scrolling = new FlowDocumentScrollViewer();
            scrolling.Document = doc;
            return scrolling;
        }
        string GetChapterPassage(string bookName, int b, int c, string prefix, string suffix, bool header)
        {
            return "GetChapterPassage();";
            /*
            string passage = (prefix != null) ? prefix : string.Empty;

            if (header)
            {
                passage += "<b>";
                passage += bookName; // "<html>"
                passage += " ";
                passage += c.ToString();
                passage += "</b><br>\n";
            }
            var bible = this.cmd.Bible;
            var book = bible.ixbook.books[b - 1];
            var chapter = bible.ixchapter.chapters[book.chapterIdx];
            bool first = true;
            int v = 0;
            uint end = chapter.writIndex + chapter.wordCount - 1;
            WritDX4 dx = new WritDX4();

            if (this.avmap == null)
            {
                this.cmd.InitializeMap();

                if (this.cmd.Map != null && this.cmd.Map.GetType() == typeof(MMWritDx4))
                    this.avmap = (MMWritDx4)this.cmd.Map;
                else
                    this.avmap = new MMWritDx4(this.cmd.sdk);
            }
            bool eoc = false;
            for (var ok = this.avmap.GetRecord(AVMemMap.FIRST, ref dx); ok && !eoc; ok = this.avmap.GetRecord(AVMemMap.NEXT, ref dx))
            {
                bool vt = (((uint)Transitions.VerseTransition) & dx.trans) != (uint)0;
                bool eb = (((uint)Transitions.EndBit) & dx.trans) != (uint)0;
                eoc = eb && ((((uint)Transitions.ChapterTransition) & dx.trans) != (uint)0);
                bool bov = false;
                bool eov = false;
                if (vt)
                {
                    eov = eb;
                    bov = !eb;
                }

                if (eov)
                {
                    passage += "</a>";
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    passage += "  ";
                }
                if (bov)
                {
                    var vstr = (++v).ToString();

                    passage += "<a name='v";
                    passage += vstr;
                    passage += "'><sup><font color='maroon'>";
                    passage += vstr;
                    passage += "</sup></font>";
                }
                else
                {
                    passage += " ";
                }
                //              if (dx.stat != 0x0000)
                //                  passage += "<b>";
                bool italics = ((dx.punc & (uint)Punctuation.MODEitalics) != 0);
                if (italics)
                    passage += "<em>";

                passage += this.cmd.Lexicon.GetLex(dx.word, capitolize: true, modernize: true);

                if (italics)
                    passage += "</em>";
                //              if (writ.stat != 0x0000)
                //                  passage += "</b>";
            }
            passage += "</a>";
            if (suffix != null)
                passage += suffix;

            return passage;
            */
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
                var result = command.statement.ExecuteEx(iprovider);

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
            var cnt = AVXCLI.AVLCLR.GetChapterCount(bk);
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
            if (command.success && command.result != null && command.result.segments != null && command.result.segments.Count > 0)
            {
                byte bk;
                byte ch;
                byte vs;
                byte ignore;

                var matches = new Dictionary<byte, List<byte>>();

                foreach (UInt16 vidx in this.ChapterHits)
                {
                    if (!AVXCLI.AVLCLR.XVerse.GetEntry(vidx, out bk, out ch, out vs, out ignore))
                        return; // something unexpected went wrong

                    List<byte> book = null;
                    if (matches.ContainsKey(bk))
                    {
                        book = matches[bk];
                    }
                    else
                    {
                        book = new List<byte>();
                        matches.Add(bk, book);
                    }
                    if (!book.Contains(ch))
                    {
                        book.Add(ch);
                    }
                }
                foreach (byte b in matches.Keys.OrderBy(book => book))
                    foreach (byte c in matches[b])
                        AddChapterChicklet(b, c);
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
            this.ResizeMode = System.Windows.ResizeMode.CanResize;
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.WindowState = System.Windows.WindowState.Normal;
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

        private void OpenChapterHelper_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChapterView.Visibility = Visibility.Visible;

            ChapterHelper.Content = "";  // Down

            if (e != null)
                e.Handled = true;
        }

        private void CloseChapterHelper_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChapterView.Visibility = Visibility.Collapsed;

            ChapterHelper.Content = ""; // Up

            if (e != null)
                e.Handled = true;
        }
        private void ChapterHelper_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ChapterHelper.Content.ToString() == "")
                CloseChapterHelper_MouseUp(sender, e);
            else
                OpenChapterHelper_MouseUp(sender, e);
        }
        private void ChapterHelper_FingerUp(object sender, ManipulationCompletedEventArgs e)
        {
            this.ChapterHelper_MouseUp(sender, null);

            if (e != null)
                e.Handled = true;
        }
        private void BookSelection(uint bookNum)
        {
            //          this.RadioShowAll.IsChecked = true;
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
                if ((string)ButtonAVT.Content == "AV")
                {
                    ButtonAVT.Content = version = "AVX";
                }
                else
                {
                    ButtonAVT.Content = version = "AV";
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