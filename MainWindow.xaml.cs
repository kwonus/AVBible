﻿namespace AVBible
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Blacklight.Controls.Wpf;

    using System.IO;
    using AVSearch.Model.Results;
    using AVSearch.Model.Expressions;
    using AVXFramework;
    using AVXLib;
    using System.Text;
    using AVSearch.Interfaces;
    using Blueprint.Blue;
    using System.Linq;
    using Blueprint.Model.Implicit;
    using AVXLib.Memory;

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
        private HelpWindow Help;

        internal uint ViewbookStartNum;
        internal uint ChapterChickletIndex = 0;
        internal AVEngine Engine = new();
        internal QueryResult? Results = null;
        internal QSettings Settings;

        private (uint count, bool ok) GetBookHitCount(byte b)
        {
            uint count = 0;
            bool ok = false;
            byte v = 0;
            byte c = 0;

            if (b >= 1 && b <= 66)
            {
                SearchExpression exp = this.Results.Expression;
                ok = true;

                if ((exp.Hits > 0) && exp.Books.ContainsKey(b))
                {
                    QueryBook book = exp.Books[b];
                    foreach (var match in book.Matches.Values)
                    {
                        if (match.Start.InRange(b, c, v))
                            continue; // skip (duplicate)
                        c = match.Start.C;
                        v = match.Start.V;
                        count++;
                    }
                }
            }
            return (count, ok);
        }
        private (uint count, bool ok) GetBookChapterHitCount(byte b, byte c)
        {
            uint count = 0;
            bool ok = false;
            byte v = 0;

            if (b >= 1 && b <= 66)
            {
                SearchExpression exp = this.Results.Expression;
                ok = true;

                if ((exp.Hits > 0) && exp.Books.ContainsKey(b))
                {
                    QueryBook book = exp.Books[b];
                    foreach (var match in book.Matches.Values)
                    {
                        if (match.Start.InRange(b, c, v))
                            continue; // skip (duplicate)
                        v = match.Start.V;
                        count++;
                    }
                }
            }
            return (count, ok);
        }
        private (uint count, bool ok) GetBookChapterVerseHitCount(byte b, byte c, byte v)
        {
            uint count = 0;
            bool ok = false;

            if (b >= 1 && b <= 66)
            {
                SearchExpression exp = this.Results.Expression;
                ok = true;

                if ((exp.Hits > 0) && exp.Books.ContainsKey(b))
                {
                    QueryBook book = exp.Books[b];
                    foreach (var match in book.Matches.Values)
                    {
                        if (match.Start.InRange(b, c, v))
                            count++;
                    }
                }
            }
            return (count, ok);
        }

        internal uint MaxiBookCnt = 0;
        internal uint MiniBookCnt = 0;

        private async Task<Boolean> Initialize()
        {
            ChapterChicklet.App = this;

            return true;
        }
        private static char[] splitters = ['_', '&', '+', ' ', '@'];
        public static string GetHelpFile(string request)
        {
            string[] topics = request.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

            foreach (string topic in topics)
            {
                switch (topic.ToLower())
                {
                    case "selection":
                    case "criteria":
                    case "search":
                    case "expression":
                    case "find":
                    case "scope":
                    case "scoping":
                    case "filter": return Path.Combine(HelpFolder, "index-selection.html");

                    case "ye":
                    case "thee":
                    case "thou":
                    case "thy":
                    case "early":
                    case "kjv":
                    case "english": return Path.Combine(HelpFolder, "index-language.html");

                    case "settings":
                    case "assign":
                    case "set":
                    case "clear":
                    case "get":
                    case "absorb": return Path.Combine(HelpFolder, "index-settings.html");

                    case "macro":
                    case "history":
                    case "macros":
                    case "tags":
                    case "tagging":
                    case "invoke":
                    case "apply":
                    case "delete":
                    case "review": return Path.Combine(HelpFolder, "index-hashtags.html");

                    case "application": return Path.Combine(HelpFolder, "index-application.html");

                    case "output":
                    case "print":
                    case "export": return Path.Combine(HelpFolder, "index-export.html");

                    case "system": return Path.Combine(HelpFolder, "index-system.html");
                }
            }
            return Path.Combine(HelpFolder, "index.html");
        }
        private static string? _HelpFolder = null;
        public static string HelpFolder
        {
            get
            {
                if (_HelpFolder != null)
                    return _HelpFolder;

                string cwd = System.AppDomain.CurrentDomain.BaseDirectory;
                for (string help = Path.Combine(cwd, "Help"); help.Length > @"X:\Help".Length; help = Path.Combine(cwd, "Help"))
                {
                    if (Directory.Exists(help))
                    {
                        _HelpFolder = help;
                        return help;
                    }
                    var parent = Directory.GetParent(cwd);
                    if (parent == null)
                        break;
                    cwd = parent.FullName;
                }
                return (@"C:\Program Files\AV-Bible\Help");
            }
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
        private string temp_settings_file = @"C:\Users\Me\AppData\Roaming\AV-Bible\settings.yaml";
        protected virtual void LoadAppState()
        {
            this.Settings = new QSettings(temp_settings_file);
            this.ButtonAVT_Click(null, null);
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadWindowState();
            LoadAppState();

            this.Help = new HelpWindow();

            SectionStack.SetBookSelector(this.BookSelection);

            ViewbookStartNum = 0;

            FullInit();

            id = 0;
        }
        public static UInt64 sequence = 0;
        public void ShowHelpPanel(string request)
        {
            string path = GetHelpFile(request);
            this.Help.HtmlControl.Source = new Uri(path);
            this.Help.Show();
            this.Help.Activate();
            this.Help.Topmost = true;
            this.Help.Topmost = false;
            this.Help.Focus();
#if !PANEL_BASED_HELP
#else
            string topic = Path.GetFileNameWithoutExtension(path);

            string header = "HELP (" + topic + ")";

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
                var content = this.GetHelp(path);
                panel = new DragDockPanel();
                panel.Content = content;
                panel.PanelLifetime = ++sequence;
                panel.PanelReference = 0;
                panel.Header = header;
                this.AVPanel.Items.Add(panel);

                ResetComboDeleteItems();
            }
#endif
        }
        public bool DeletePanel(ChapterChicklet chicklet)
        {
            bool deleted = false;

            byte b = (byte)(chicklet.BookChapter >> 8);
            byte c = (byte)(chicklet.BookChapter & 0xFF);

            const int AV = 0;
            const int AVX = 1;
            bool[] both = new bool[] { false, false };

            switch (this.ButtonAVX.Content.ToString().ToUpper())
            {
                case "AV":   both[AV]  = true; break;
                case "AVX":  both[AVX] = true; break;
                default:     both[AV]  = true;
                             both[AVX] = true; break;
            }
            for (int v = AV; v <= AVX; v++)
            {
                if (!both[v])
                    continue;
                if (b >= 1 && b <= 66)
                {
                    AVXLib.Memory.Book book = ObjectTable.AVXObjects.Mem.Book.Slice(b, 1).Span[0];

                    string header = book.name + " " + c.ToString();
                    if (v == AVX)
                        header += " (AVX)";

                    DragDockPanel panel = null;
                    UInt16 encoding = 0;
                    foreach (DragDockPanel existing in this.AVPanel.Items)
                    {
                        var candidate = existing.Header.ToString();
                        if (candidate == header)
                        {
                            panel = existing;
                            encoding = panel.PanelReference;
                            break;
                        }
                    }
                    if (panel != null)
                    {
                        this.AVPanel.Items.Remove(panel);
                        deleted = true;
                    }
                }
            }
            if (deleted)
            {
                ResetComboDeleteItems();
                ClearHighlightsForPanelsNotFound();
            }
            return deleted;
        }
        public void AddPanel(ChapterChicklet chicklet)
        {
            byte b = (byte)(chicklet.BookChapter >> 8);
            byte c = (byte)(chicklet.BookChapter & 0xFF);

            const int AV = 0;
            const int AVX = 1;
            bool[] both = new bool[] { false, false };

            switch (this.ButtonAVX.Content.ToString().ToUpper())
            {
                case "AV":   both[AV]  = true; break;
                case "AVX":  both[AVX] = true; break;
                default:     both[AV]  = true;
                             both[AVX] = true; break;
            }
            for (int v = AV; v <= AVX; v++)
            {
                if (!both[v])
                    continue;
                if (b >= 1 && b <= 66)
                {
                    AVXLib.Memory.Book book = ObjectTable.AVXObjects.Mem.Book.Slice(b, 1).Span[0];

                    string header = book.name + " " + c.ToString();
                    if (v == AVX)
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
                        panel = new DragDockPanel();
                        panel.PanelReference = chicklet.BookChapter;
                        panel.Header = header;
                        this.AVPanel.Items.Add(panel);
                    }
                    var content = this.GetChapter(b, c, (v == AVX));
                    panel.Content = content;
                    panel.PanelLifetime = ++sequence;
                }
                ResetComboDeleteItems();
            }
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

        private bool GetPageHtml(string bookName, byte b, byte c, byte v)
        {
            bool header = (b >= 1) && (b <= 66);
            bool script = header && (v >= 1);

            string font;
            string fontSuffix;
            GetFontStrings(out font, out fontSuffix);

            QueryBook? book = null;
            ISettings? settings = null;

            if (this.Results != null && this.Results.Expression != null)
            {
                var exp = this.Results.Expression;
                if (exp.Books.ContainsKey(b))
                {
                    settings = exp.Settings;
                    book = exp.Books[b];
                }
            }
            if (settings == null)
            {
                settings = new QSettings(this.temp_settings_file);
            }

            if (header || script)
            {
                StringBuilder builder = new(1024);

                builder.Append("<html>");

                if (header)
                {
                    builder.Append("<head><title>");
                    builder.Append(bookName);
                    builder.Append(" ");
                    builder.Append(c.ToString());
                    if (v != 0)
                    {
                        builder.Append(":");
                        builder.Append(v.ToString());
                    }

                    builder.Append("</title></head>");
                }
                builder.Append("<body>");
                ChapterRendering rendering = Engine.GetChapter(b, c, book != null ? book.Matches : new());
                Engine.RenderChapterAsHtml(builder, rendering, settings);

                builder.Append("</body>");
                builder.Append("</html>");
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
                default: return "";
            }
        }
        private System.Windows.Documents.Run GetVerseLabel(byte num, bool BoC, bool backlight)
        {
            string padding = BoC ? "" : "  ";
            System.Windows.Documents.Run vlabel = new System.Windows.Documents.Run(padding + num.ToString() + " ");
            vlabel.Foreground = Brushes.Cyan;
            if (backlight)
            {
                vlabel.Background = Brushes.LightCyan;
            }
            else
            {
                vlabel.Background = Brushes.Black;
            }
            return vlabel;
        }

        private FlowDocumentScrollViewer GetChapter(byte b, byte c, bool modernize, bool header = false, string bookName = null)
        {
            bool diffs = !ButtonAVX.Content.ToString().StartsWith("AV", StringComparison.OrdinalIgnoreCase);
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
            if (b >= 1 && b <= 66 && c >= 1)
            {
                AVXLib.Memory.Book bk = ObjectTable.AVXObjects.Mem.Book.Slice(b, 1).Span[0];

                if (c <= bk.chapterCnt)
                {
                    var pdoc = new System.Windows.Documents.Paragraph();
                    pdoc.TextAlignment = TextAlignment.Justify;

                    QueryBook? book = null;
                    ISettings? settings = null;

                    if (this.Results != null && this.Results.Expression != null)
                    {
                        var exp = this.Results.Expression;
                        if (exp.Books.ContainsKey(b))
                        {
                            settings = exp.Settings;
                            book = exp.Books[b];
                        }
                    }
                    if (settings == null)
                    {
                        settings = new QSettings(this.temp_settings_file);
                    }
                    Dictionary<uint, QueryMatch> matches = book != null ? book.Matches : new();
                    ChapterRendering chapter = Engine.GetChapter(b, c, matches);

                    byte v = 0;

                    bool paren = false;
                    bool BoC = true;
                    bool alreadyAddedSpaceAfter = false;
 
                    foreach (VerseRendering verse in chapter.Verses.Values)
                    {
                        bool BoV = true;
                        ++v;

                        bool backlightRun = false;
                        foreach (WordRendering word in verse.Words)
                        {
                            bool backlight = false;
                            var highlights = from highlight in matches.Values where word.Coordinates >= highlight.Start && word.Coordinates <= highlight.Until select highlight;
                            foreach (var item in highlights.Take(1))
                            {
                                backlight = true;
                            }
                            if (BoV)
                            {
                                var vlabel = GetVerseLabel(v, BoC, backlightRun && backlight);
                                pdoc.Inlines.Add(vlabel);
                                alreadyAddedSpaceAfter = true;
                                BoC = false;
                                BoV = false;
                            }
                            if (!backlightRun)
                            {
                                pdoc.Inlines.Add(" ");
                                alreadyAddedSpaceAfter = !backlight;
                            }

                            if ((!alreadyAddedSpaceAfter) || (!backlightRun))
                            {
                                if (backlight)  // for better visuals, highlight space after all backlights
                                {
                                    var space = new System.Windows.Documents.Run(" ");
                                    if (backlightRun || backlight)
                                    {
                                        space.Background = Brushes.LightCyan;
                                        space.Foreground = Brushes.Black;
                                    }
                                    else
                                    {
                                        space.Background = Brushes.Black;
                                        space.Foreground = Brushes.White;
                                    }
                                    pdoc.Inlines.Add(space);
                                }
                                else
                                {
                                    pdoc.Inlines.Add(" ");
                                }
                            }
                            else
                            {
                                alreadyAddedSpaceAfter = false;
                            }
                            if (backlightRun && !backlight)
                            {
                                pdoc.Inlines.Add(" ");
                            }
                            string postPunc = "";
                            bool jesus = (word.Punctuation & 0x01) != 0;
                            bool italics = ((word.Punctuation & 0x02) != 0);
                            string lex = "";
                            if (((word.Punctuation & 0x04) != 0) && !paren)
                            {
                                paren = true;

                                var open = new System.Windows.Documents.Run("(");
                                if (backlightRun)
                                {
                                    open.Background = Brushes.LightCyan;
                                    open.Foreground = Brushes.Black;
                                }
                                else
                                {
                                    open.Background = Brushes.Black;
                                    open.Foreground = Brushes.White;
                                }
                                pdoc.Inlines.Add(open);
                            }
                            lex += modernize ? word.Modern : word.Text;
                            if ((word.Punctuation & 0x10) != 0)
                            {
                                bool s = (lex[lex.Length - 1] | 0x20) == 's';
                                lex += s ? "'" : "'s";
                            }
                            if ((word.Punctuation & 0x0C) == 0x0C)
                            {
                                paren = false;
                                postPunc = ")";
                            }
                            backlightRun = backlight;

                            if ((word.Punctuation & 0xE0) != 0)
                            {
                                postPunc += this.PostPunc(word.Punctuation);
                            }
                            {// Wall off the phrase variable so that it is NOT inadvertently referenced outside of this scope
                                bool diff = diffs && word.Modernized;

                                var phrase = new System.Windows.Documents.Run(lex);
                                if (italics)
                                {
                                    phrase.FontStyle = FontStyles.Italic;
                                }
                                if (backlightRun)
                                {
                                    if (diff)
                                        phrase.Background = Brushes.PaleGreen;
                                    else
                                        phrase.Background = Brushes.LightCyan;

                                    if (jesus)
                                        phrase.Foreground = Brushes.MediumVioletRed;
                                    else
                                        phrase.Foreground = Brushes.Black;
                                }
                                else
                                {
                                    if (diff)
                                    {
                                        phrase.Background = Brushes.PaleGreen;
                                        if (jesus)
                                            phrase.Foreground = Brushes.MediumVioletRed;
                                        else
                                            phrase.Foreground = Brushes.Black;
                                    }
                                    else
                                    {
                                        phrase.Background = Brushes.Black;
                                        if (jesus)
                                            phrase.Foreground = Brushes.LightPink;
                                        else
                                            phrase.Foreground = Brushes.White;
                                    }
                                }
                                if (word.Triggers.Count > 0)
                                {
                                    phrase.FontWeight = FontWeights.Bold;
                                }
                                else
                                {
                                    phrase.FontWeight = FontWeights.Normal;
                                }
                                pdoc.Inlines.Add(phrase);
                            }
                            if (modernize && word.Modernized)
                            {
                                char n = '0';

                                if (word.PnPos.number == NUMBER.Singular)
                                    n = '1';
                                else if (word.PnPos.number == NUMBER.Plural)
                                    n = 'n';

                                if (word.NuPos == "vbd2s")
                                    n = '1'; // this masks a bug in pnpos bits for wast

                                if (n == '0')
                                {
                                    // Pronoun bits are mostly well-behaved, but this code closes some gaps
                                    //
                                    if (word.Modern.StartsWith("you", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        if (word.Text.StartsWith("th", StringComparison.InvariantCultureIgnoreCase))
                                            n = '1';
                                        else if (word.Text.StartsWith("y", StringComparison.InvariantCultureIgnoreCase))
                                            n = 'n';
                                    }
                                    else if (word.Text.EndsWith("st", StringComparison.InvariantCultureIgnoreCase) && !word.Modern.EndsWith("st", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        n = '1';
                                    }
                                    else if (word.Text.Equals("art", StringComparison.InvariantCultureIgnoreCase) || word.Text.Equals("dost", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        n = '1';
                                    }
                                }

                                if (n != '0')
                                {
                                    var superscript = new System.Windows.Documents.Run(n.ToString());
                                    superscript.Typography.Variants = FontVariants.Superscript;
                                    if (backlightRun)
                                    {
                                        superscript.Background = Brushes.LightCyan;
                                        superscript.Foreground = Brushes.Black;
                                    }
                                    else
                                    {
                                        superscript.Background = Brushes.Black;
                                        superscript.Foreground = Brushes.White;
                                    }
                                    pdoc.Inlines.Add(superscript);
                                }
                            }

                            if (postPunc.Length > 0)
                            {
                                var punc = new System.Windows.Documents.Run(postPunc);

                                if (backlightRun)
                                {
                                    punc.Background = Brushes.LightCyan;
                                    punc.Foreground = Brushes.Black;
                                }
                                else
                                {
                                    punc.Background = Brushes.Black;
                                    punc.Foreground = Brushes.White;
                                }
                                pdoc.Inlines.Add(punc);
                            }
                            if (backlight)  // for better visuals, highlight space after all backlights
                            {
                                alreadyAddedSpaceAfter = true;
                                var space = new System.Windows.Documents.Run(" ");
                                if (backlightRun || backlight)
                                {
                                    space.Background = Brushes.LightCyan;
                                    space.Foreground = Brushes.Black;
                                }
                                else
                                {
                                    space.Background = Brushes.Black;
                                    space.Foreground = Brushes.White;
                                }
                                pdoc.Inlines.Add(space);
                            }
                        }
                    }
                    doc.Blocks.Add(pdoc);
                }
            }
            var scrolling = new FlowDocumentScrollViewer();
            scrolling.Document = doc;
            return scrolling;
        }
        private System.Windows.Documents.Run GetSegment(StringBuilder segment, int stars)
        {
            System.Windows.Documents.Run run = new(segment.ToString());

            switch (stars)
            {
                case 3:
                    run.FontWeight = FontWeights.Bold;
                    run.FontStyle = FontStyles.Italic;
                    break;
                case 2:
                    run.FontWeight = FontWeights.Bold;
                    break;
                case 1:
                    run.FontStyle = FontStyles.Italic;
                    break;
            }
            return run;
        }
        private System.Windows.Documents.TableCell GetCellContent(string input)
        {

            System.Windows.Documents.Block block = GetBlock(input);
            return new System.Windows.Documents.TableCell(block);
        }
        private System.Windows.Documents.Block GetBlock(string input)
        {
            System.Windows.Documents.Section section = new();

            string[] lines = input.Split("<br/>", StringSplitOptions.None);
            foreach (string line in lines)
            {
                int stars = 0;
                bool emphasis = false;
                char prev = '\0';

                System.Windows.Documents.Paragraph span = new();
                StringBuilder segment = new(25);
                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];

                    string context = c.ToString();

                    if (c == '\\')
                    {
                        if (prev == '\\')
                        {
                            context = "\\";
                        }
                        else
                        {
                            prev = c;
                            continue;
                        }
                    }
                    else if (prev == '\\')
                    {
                        context = "\\" + c;
                    }

                    if (emphasis == false && context == "*")
                    {
                        if (segment.Length > 0)
                        {
                            var part = GetSegment(segment, stars);
                            span.Inlines.Add(part);
                            segment.Clear();
                        }
                        stars++;
                        continue;
                    }

                    emphasis = (stars > 0);

                    if (emphasis == true && context == "*")
                    {
                        int agreement = stars-1;
                        for (i++; i < line.Length; i++)
                        {
                            c = line[i];
                            if (c == '*')
                            {
                                if (--agreement == 0)
                                    break;
                            }
                            else
                            {
                                i--;
                                break;
                            }
                        }
                        var part = GetSegment(segment, stars);
                        span.Inlines.Add(part);
                        segment.Clear();

                        stars = agreement;
                    }
                    else
                    {
                        segment.Append(c);
                    }
                    prev = c;
                }
                if (segment.Length > 0)
                {
                    var part = GetSegment(segment, stars);
                    span.Inlines.Add(part);
                    segment.Clear();
                }
                if (lines.Length == 1)
                {
                    return span;
                }
                section.Blocks.Add(span);
            }
            return section;
        }
        FlowDocumentScrollViewer GetHelp(string md)   // MarkDown file
        {
            var doc = new System.Windows.Documents.FlowDocument();

            var styleParagraph = new Style(typeof(System.Windows.Documents.Paragraph));
            styleParagraph.Setters.Add(new Setter(System.Windows.Documents.Block.MarginProperty, new Thickness(0)));
            doc.Resources.Add(typeof(System.Windows.Documents.Paragraph), styleParagraph);

            var styleTable = new Style(typeof(System.Windows.Documents.Table));
            styleTable.Setters.Add(new Setter(System.Windows.Documents.Table.BorderThicknessProperty, new Thickness(10.0)));
            doc.Resources.Add(typeof(System.Windows.Documents.Table), styleTable);

            doc.FontSize = this.panel_fontSize;
            doc.FontFamily = this.panel_fontFamily;
            doc.Foreground = new SolidColorBrush(Colors.White);

            if (md != null && File.Exists(md))
            {
                System.Windows.Documents.Table table = null;

                var input = new FileStream(md, FileMode.Open, FileAccess.Read);
                var reader = new StreamReader(input);

                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    string trimmed = line.Trim();
                    if (trimmed.StartsWith("|") && trimmed.EndsWith("|") && trimmed.Length >= 3)
                    {
                        bool header = (table == null);
                        if (header)
                        {
                            table = new();
                            table.Background = Brushes.LightGray;
                            table.Foreground = Brushes.Black;
                            table.BorderThickness = new Thickness(16.0);
                            table.BorderBrush = new SolidColorBrush(Colors.Black);
                            table.CellSpacing = 5.0;
                        }
                        string[] columns = trimmed.Substring(1, trimmed.Length - 2).Split('|', StringSplitOptions.None);

                        if (columns.Length == 0 || columns[0].Contains("---"))  // empty rows and header markers are ignored
                            continue;

                        System.Windows.Documents.TableRow row = new();
                        if (header)
                        {
                            row.Background = new SolidColorBrush(Colors.White);
                        }
                        foreach (var column in columns)
                        {
                            System.Windows.Documents.TableCell cell = GetCellContent(column.Trim());

                            if (header)
                            {
                                cell.FontWeight = FontWeights.Bold;
                            }
                            row.Cells.Add(cell);
                        }
                        System.Windows.Documents.TableRowGroup group = new();
                        group.Rows.Add(row);
                        table.RowGroups.Add(group);
                        continue;
                    }
                    else if (table != null)
                    {
                        doc.Blocks.Add(table);
                        table = null;
                    }
                    // differentiate between different header levels
                    if (trimmed.StartsWith("####"))
                    {
                        int index = 4;
                        for (/**/; index < trimmed.Length && trimmed[index] == '#'; index++)
                            ;
                        System.Windows.Documents.Block phead = this.GetBlock(line.Substring(index).Trim());
                        phead.Foreground = Brushes.Silver;
                        phead.FontSize = this.panel_fontHead;
                        phead.FontWeight = FontWeights.Bold;
                        doc.Blocks.Add(phead);
                    }
                    else if (trimmed.StartsWith("###"))
                    {
                        var index = line.LastIndexOf('#');
                        System.Windows.Documents.Block phead = this.GetBlock(line.Substring(index+1).Trim());
                        phead.Foreground = Brushes.Green;
                        phead.FontSize = this.panel_fontHead;
                        phead.FontWeight = FontWeights.Bold;
                        doc.Blocks.Add(phead);
                    }
                    else if (trimmed.StartsWith("##"))
                    {
                        System.Windows.Documents.Block phead = this.GetBlock(line.Substring(2).Trim());
                        phead.FontSize = this.panel_fontHead;
                        phead.FontWeight = FontWeights.Bold;
                        doc.Blocks.Add(phead);
                    }
                    else if (trimmed.StartsWith("#"))
                    {
                        System.Windows.Documents.Block phead = this.GetBlock(line.Substring(1).Trim());
                        phead.FontSize = this.panel_fontHead;
                        phead.FontWeight = FontWeights.Bold;
                        doc.Blocks.Add(phead);
                    }
                    else if (trimmed.StartsWith("--"))
                    {
                        int index = line.IndexOf('-');
                        string modified = line.Substring(0, index) + "○ " + line.Substring(index+2).Trim();
                        System.Windows.Documents.Block block = this.GetBlock(modified);
                        doc.Blocks.Add(block);
                    }
                    else if (trimmed.StartsWith("-"))
                    {
                        int index = line.IndexOf('-');
                        string modified = line.Substring(0, index) + (index < 1 ? "● " : "○ ") + line.Substring(index + 1).Trim();
                        System.Windows.Documents.Block block = this.GetBlock(modified);
                        doc.Blocks.Add(block);
                    }
                    else
                    {
                        System.Windows.Documents.Block block = this.GetBlock(line);
                        doc.Blocks.Add(block);
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
        private void ResetChapterView(ChapterSpec spec)
        {
            SetSearchView(); // used to be SetChapterViewViaSearch(index, reset)
        }

        (bool success, SelectionResultType status, QueryResult results, SearchScope scope) QuelleCommand(string text)
        {
            bool success = false;
            var tuple = Engine.Execute(text);
            SelectionResultType status = SelectionResultType.InvalidStatement;
            if (tuple.ok)
            {
                if (tuple.stmt.Commands != null)
                    status = tuple.stmt.Commands.Status;
                else if (tuple.stmt.Singleton != null)
                    status = SelectionResultType.SingletonSuccess;
            }

            var message = !string.IsNullOrWhiteSpace(tuple.message);
            if (message)
            {
                success = tuple.message.Equals("ok", StringComparison.InvariantCultureIgnoreCase);
                if (!success)
                    Console.Error.WriteLine(tuple.message);
            }
            if (success)
            {
                if (tuple.stmt.Singleton != null)
                {
                    var type = tuple.stmt.Singleton.GetType();

                    if (type == typeof(QExit))
                    {
                        this.Close();
                    }
                    if (type == typeof(QHelp))
                    {
                        ShowHelpPanel(text);
                        this.Results = null;
                    }
                }
                else if (tuple.search != null && tuple.search.Expression != null)
                {
                    this.Results = tuple.search;
                }
                return (true, status, tuple.search, tuple.search.Expression.Scope);
            }
            this.Results = null;
            return (false, status, tuple.search, tuple.search.Expression.Scope);
        }
        private void SetEntireView(byte bk)
        {
            this.ChapterView.Visibility = Visibility.Visible;
            ChapterStack.Children.Clear();

            byte cnt = (bk >= 1 && bk <= 66) ? ObjectTable.AVXObjects.Mem.Book.Slice(bk, 1).Span[0].chapterCnt : (byte)0;
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
            byte weight = 0;
            if (this.Results != null)
            {
                SearchExpression exp = this.Results.Expression;

                if (exp != null)
                {
                    if (exp.Books.ContainsKey(b))
                    {
                        QueryBook bk = exp.Books[b];
                        if (bk.Chapters.ContainsKey(c))
                        {
                            QueryChapter ch = bk.Chapters[c];

                            foreach (QueryMatch match in bk.Matches.Values)
                            {
                                if (match.Start.InRange(b, c) || match.Until.InRange(b, c))
                                    weight++;
                            }
                        }
                    }
                }
            }
            if (weight > 6)
                weight = 6;

            var chicklet = new ChapterChicklet(b, c, weight, green);
            this.ChapterStack.Children.Add(chicklet);
        }
        private void SetSearchView(int index = 0, bool reset = true)
        {
            this.ChapterView.Visibility = Visibility.Visible;

            ChapterStack.Children.Clear();

            var command = QuelleCommand(this.TextCriteria.Text);
            this.ChapterStack.Children.Clear();

            var verses = new HashSet<UInt16>();
            if (this.Results != null && command.success && command.results != null && (command.results.TotalHits > 0 || command.status == SelectionResultType.ScopeOnlyResults))
            {
                byte bkLast = 0;
                byte chLast = 0;

                if (command.status == SelectionResultType.SearchResults)
                {
                    SearchExpression exp = this.Results.Expression;
                    if (exp != null)
                    {
                        if (exp.Hits > 0)
                        {
                            foreach (QueryBook book in exp.Books.Values)
                            {
                                foreach (var match in book.Matches.Values)
                                {
                                    byte c = match.Start.C;
                                    if (c != chLast && book.BookNum != bkLast)
                                    {
                                        AddChapterChicklet(book.BookNum, c);
                                        chLast = c;
                                        bkLast = book.BookNum;
                                    }

                                    c = match.Until.C;
                                    if (c != chLast && book.BookNum != bkLast)
                                    {
                                        AddChapterChicklet(book.BookNum, c);
                                        chLast = c;
                                        bkLast = book.BookNum;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (command.status == SelectionResultType.ScopeOnlyResults)
                {
                    if (command.scope.Count > 0)
                    {
                        for (byte b = 1; b <= 66; b++)
                        {
                            if (command.scope.ContainsKey(b))
                            {
                                foreach (byte c in command.scope[b].Chapters)
                                {
                                    AddChapterChicklet(b, c);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void TextCriteria_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                /* TO DO: 2024/Q1
                this.found = null;
                */
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
        private void BookSelection(byte bookNum)
        {
            if (bookNum >= 1 && bookNum <= 66)
            {
                SetEntireView((byte)bookNum);
            }
            else
            {
                DragDockPanel panel = null;
                foreach (DragDockPanel candidate in this.AVPanel.Items)
                {
                    if (panel != null && panel.PanelReference == 0)  // hekp panel
                        continue;

                    if (panel == null)
                    {
                        panel = candidate;
                    }
                    else if (panel.PanelLifetime < candidate.PanelLifetime)
                    {
                        panel = candidate;
                    }
                }
                if (panel != null)
                {
                    uint lastSelected = (uint)(panel.PanelReference / 0x100);
                    SetEntireView((byte)lastSelected);
                }
                else if (this.BookStack.lastChosenBook >= 1 && this.BookStack.lastChosenBook <= 66)
                {
                    SetEntireView(this.BookStack.lastChosenBook);
                }
                else
                {
                    SetEntireView(1);
                }
            }
        }

        private void MainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Help.CloseHelpWindow();
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
                    this.Settings.Lexicon.Render.Value = QLexicon.QLexiconVal.AVX;
                    if (!this.Settings.SearchAsAVX)
                        this.Settings.Lexicon.Domain.Value = QLexicon.QLexiconVal.BOTH;
                    this.Settings.Update();
                }
                else if ((string)ButtonAVX.Content == "AVX")
                {
                    ButtonAVX.Content = version = "Side-by-Side";
                    this.Settings.Lexicon.Render.Value = QLexicon.QLexiconVal.BOTH;
                    if (!(this.Settings.SearchAsAVX && this.Settings.SearchAsAV))
                        this.Settings.Lexicon.Domain.Value = QLexicon.QLexiconVal.BOTH;
                    this.Settings.Update();
                }
                else // BOTH
                {
                    ButtonAVX.Content = version = "AV";
                    this.Settings.Lexicon.Render.Value = QLexicon.QLexiconVal.AV;
                    if (!this.Settings.SearchAsAV)
                        this.Settings.Lexicon.Domain.Value = QLexicon.QLexiconVal.BOTH;
                    this.Settings.Update();
                }
            }
            else
            {
                if (this.Settings.Lexicon.Render.Value == QLexicon.QLexiconVal.AV)
                {
                    if ((string)ButtonAVX.Content != "AV")
                    {
                        ButtonAVX.Content = version = "AV";
                    }
                }
                else if (this.Settings.Lexicon.Render.Value == QLexicon.QLexiconVal.AVX)
                {
                    if ((string)ButtonAVX.Content != "AVX")
                    {
                        ButtonAVX.Content = version = "AVX";
                    }
                }
                else // BOTH
                {
                    if ((string)ButtonAVX.Content != "Side-by-Side")
                    {
                        ButtonAVX.Content = version = "Side-by-Side";
                    }
                }
            }
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
        private void comboBoxHelpPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxHelpPanel.SelectedItem != null)
            {
                var help = (ComboBoxItem) (comboBoxHelpPanel.SelectedItem);

                this.ShowHelpPanel(help.Name);

                comboBoxHelpPanel.SelectedItem = null;  // this will allow panel to be reopenned if it is closed (we always want to generate a changed event.
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
                ClearHighlightsForPanelsNotFound();
            }
        }
        private void ClearHighlightsForPanelsNotFound()
        {
            HashSet<UInt16> encodings = new();

            foreach (DragDockPanel panel in this.AVPanel.Items)
            {
                UInt16 encoding = panel.PanelReference;

                if (!encodings.Contains(encoding))
                    encodings.Add(encoding);
            }
            foreach (var item in this.ChapterStack.Children)
            {
                var chicklet = (ChapterChicklet)item;
                if (!encodings.Contains(chicklet.BookChapter))
                    chicklet.Refresh(false);
            }
        }
    }
}