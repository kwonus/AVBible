using Blueprint.Model.Implicit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AVBible
{
    /// <summary>
    /// Interaction logic for BookChickletMiniStack.xaml
    /// </summary>
    public partial class SectionStack : UserControl
    {
        public delegate void BookChooser(byte bookNum);

        public Label[] labels;

        Dictionary<string, Grid> ClickManager;
        Dictionary<string, Label> SectionMap;
        Dictionary<string, byte> BookMap;

        private BookChooser Selection;
        private static SectionStack SELF;
        private static MainWindow MAIN; 
        public static void SetBookSelector(BookChooser selection, MainWindow main)
        {
            SectionStack.SELF.Selection = selection;
            SectionStack.MAIN = main;
            SELF.Lexicon_Init();
        }
        public SectionStack()
        {
            InitializeComponent();
            this.Selection = null;
            SectionStack.SELF = this;
 
            BookMap = new();
            SectionMap = new();
            SectionMap[this.OTlaw.Name]              = this.OTlaw;
            SectionMap[this.OThistory.Name]          = this.OThistory;
            SectionMap[this.OTpoetry.Name]           = this.OTpoetry;
            SectionMap[this.OTmajorprophets.Name]    = this.OTmajorprophets;
            SectionMap[this.OTminorprophets.Name]    = this.OTminorprophets;
            SectionMap[this.NTgospelsANDacts.Name]   = this.NTgospelsANDacts;
            SectionMap[this.NTChurch.Name]           = this.NTChurch;
            SectionMap[this.NTPastoral.Name]         = this.NTPastoral;
            SectionMap[this.NTOtherEpistles.Name]    = this.NTOtherEpistles;
            SectionMap[this.ConfigLexicons.Name]     = this.ConfigLexicons;
            SectionMap[this.ConfigHighlights.Name]   = this.ConfigHighlights;

            ClickManager = new Dictionary<string, Grid>();
            ClickManager[this.OTlaw.Name]            = this.PanelLaw;
            ClickManager[this.OThistory.Name]        = this.PanelHistory;
            ClickManager[this.OTpoetry.Name]         = this.PanelPoetry;
            ClickManager[this.OTmajorprophets.Name]  = this.PanelMajorProphets;
            ClickManager[this.OTminorprophets.Name]  = this.PanelMinorProphets;
            ClickManager[this.NTgospelsANDacts.Name] = this.PanelGospelsAndActs;
            ClickManager[this.NTChurch.Name]         = this.PanelChurchEpistles;
            ClickManager[this.NTPastoral.Name]       = this.PanelPastoralEpistles;
            ClickManager[this.NTOtherEpistles.Name]  = this.PanelOtherEpistles;
            ClickManager[this.ConfigLexicons.Name]   = this.PanelConfigLexicons;
            ClickManager[this.ConfigHighlights.Name] = this.PanelConfigHighlights;

            this.labels = new Label[66];

            this.labels[(byte)0] = this.B01;
            this.labels[(byte)1] = this.B02;
            this.labels[(byte)2] = this.B03;
            this.labels[(byte)3] = this.B04;
            this.labels[(byte)4] = this.B05;
            this.labels[(byte)5] = this.B06;
            this.labels[(byte)6] = this.B07;
            this.labels[(byte)7] = this.B08;
            this.labels[(byte)8] = this.B09;
            this.labels[(byte)9] = this.B10;
            this.labels[(byte)10] = this.B11;
            this.labels[(byte)11] = this.B12;
            this.labels[(byte)12] = this.B13;
            this.labels[(byte)13] = this.B14;
            this.labels[(byte)14] = this.B15;
            this.labels[(byte)15] = this.B16;
            this.labels[(byte)16] = this.B17;
            this.labels[(byte)17] = this.B18;
            this.labels[(byte)18] = this.B19;
            this.labels[(byte)19] = this.B20;
            this.labels[(byte)20] = this.B21;
            this.labels[(byte)21] = this.B22;
            this.labels[(byte)22] = this.B23;
            this.labels[(byte)23] = this.B24;
            this.labels[(byte)24] = this.B25;
            this.labels[(byte)25] = this.B26;
            this.labels[(byte)26] = this.B27;
            this.labels[(byte)27] = this.B28;
            this.labels[(byte)28] = this.B29;
            this.labels[(byte)29] = this.B30;
            this.labels[(byte)30] = this.B31;
            this.labels[(byte)31] = this.B32;
            this.labels[(byte)32] = this.B33;
            this.labels[(byte)33] = this.B34;
            this.labels[(byte)34] = this.B35;
            this.labels[(byte)35] = this.B36;
            this.labels[(byte)36] = this.B37;
            this.labels[(byte)37] = this.B38;
            this.labels[(byte)38] = this.B39;
            this.labels[(byte)39] = this.B40;
            this.labels[(byte)40] = this.B41;
            this.labels[(byte)41] = this.B42;
            this.labels[(byte)42] = this.B43;
            this.labels[(byte)43] = this.B44;
            this.labels[(byte)44] = this.B45;
            this.labels[(byte)45] = this.B46;
            this.labels[(byte)46] = this.B47;
            this.labels[(byte)47] = this.B48;
            this.labels[(byte)48] = this.B49;
            this.labels[(byte)49] = this.B50;
            this.labels[(byte)50] = this.B51;
            this.labels[(byte)51] = this.B52;
            this.labels[(byte)52] = this.B53;
            this.labels[(byte)53] = this.B54;
            this.labels[(byte)54] = this.B55;
            this.labels[(byte)55] = this.B56;
            this.labels[(byte)56] = this.B57;
            this.labels[(byte)57] = this.B58;
            this.labels[(byte)58] = this.B59;
            this.labels[(byte)59] = this.B60;
            this.labels[(byte)60] = this.B61;
            this.labels[(byte)61] = this.B62;
            this.labels[(byte)62] = this.B63;
            this.labels[(byte)63] = this.B64;
            this.labels[(byte)64] = this.B65;
            this.labels[(byte)65] = this.B66;

            var books = new List<ComboBoxItem>();
            for (byte b = 0; b < 39; b++)
            {
                var item = new ComboBoxItem();
                item.Content = this.labels[b].Content;
                books.Add(item);
                BookMap.Add(this.labels[b].Content.ToString(), (byte)(b+1));
            }
            this.comboBoxOT.ItemsSource = books;

            books = new List<ComboBoxItem>();
            for (int b = 39; b < 66; b++)
            {
                var item = new ComboBoxItem();
                item.Content = this.labels[b].Content;
                books.Add(item);
                BookMap.Add(this.labels[b].Content.ToString(), (byte)(b + 1));
            }
            this.comboBoxNT.ItemsSource = books;
        }

        private void SectionClick(Label sender)
        {
            var panel = this.ClickManager[sender.Name];

            if (panel.Visibility == Visibility.Visible)
            {
                panel.Visibility = Visibility.Collapsed;
            }
            else
            {
                foreach (var p in ClickManager.Values)
                    if (p.Visibility == Visibility.Visible)
                        p.Visibility = Visibility.Collapsed;
                panel.Visibility = Visibility.Visible;
                SectionMap[sender.Name].FontWeight = FontWeights.Bold;
            }
        }

        private void Section_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != null && sender.GetType() == typeof(Label))
                SectionClick((Label)sender);
        }

        private void Section_OnTouchUp(object sender, TouchEventArgs e)
        {
            if (sender != null && sender.GetType() == typeof(Label))
                SectionClick((Label)sender);
        }
        private void Book_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != null && sender.GetType() == typeof(Label))
                BookClick((Label)sender);
        }
        private void BookClick(Label sender)
        {
            ChooseBook(sender.Content.ToString());
        }
        private void Book_OnTouchUp(object sender, TouchEventArgs e)
        {
            if (sender != null && sender.GetType() == typeof(Label))
                BookClick((Label)sender);
        }
        private static SolidColorBrush Highlight = new SolidColorBrush(Colors.Cyan);
        private static SolidColorBrush Normal = new SolidColorBrush(Colors.White);

        private void Book_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender != null)
                Section_MouseMove(null, null);

            if (sender != null && sender.GetType() == typeof(Label))
            {
                Label label = (Label)sender;
                uint b = BookMap[label.Content.ToString()];
                string bstr = b > 9 ? "B" + b.ToString() : "B0" + b.ToString();

                foreach (Label book in this.labels)
                {
                    if (book.Name == bstr)
                        book.Foreground = Highlight;
                    else
                        book.Foreground = Normal;
                }
            }
            else
            {
                foreach (Label book in this.labels)
                    book.Foreground = Normal;
            }
            if (e != null)
                e.Handled = true;
        }

        private void Section_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender != null)
                Book_MouseMove(null, null);

            if (sender != null && sender.GetType() == typeof(Label))
            {
                Label label = (Label)sender;

                foreach (Label section in this.SectionMap.Values)
                {
                    if (label.Name == section.Name)
                        label.FontWeight = FontWeights.Bold;
                    else if (section.FontWeight != FontWeights.Normal && this.ClickManager[section.Name].Visibility == Visibility.Collapsed)
                        section.FontWeight = FontWeights.Normal;
                }
            }
            else
            {
                foreach (Label section in this.SectionMap.Values)
                {
                    if (section.FontWeight != FontWeights.Normal && this.ClickManager[section.Name].Visibility == Visibility.Collapsed)
                        section.FontWeight = FontWeights.Normal;
                }
            }
            if (e != null)
                e.Handled = true;
        }

        private void Other_MouseMove(object sender, MouseEventArgs e)
        {
            ;
        }

        public byte lastChosenBook { get; private set; } = 0;       
        private void ChooseBook(string book)
        {
            byte booknum = this.BookMap[book];
            if (this.Selection != null)
                this.Selection(booknum);
            lastChosenBook = (byte)booknum;
        }

        private void comboBoxNT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboBoxNT.SelectedIndex >= 0)
            {
                ChooseBook(((ComboBoxItem)this.comboBoxNT.SelectedItem).Content.ToString());
                this.comboBoxNT.SelectedIndex = (-1);
            }
        }

        private void comboBoxOT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboBoxOT.SelectedIndex >= 0)
            {
                ChooseBook(((ComboBoxItem)this.comboBoxOT.SelectedItem).Content.ToString());
                this.comboBoxOT.SelectedIndex = (-1);
            }
        }

        private void SectionStack_MouseMove(object sender, MouseEventArgs e)
        {
            Book_MouseMove(null, null);
            Section_MouseMove(null, null);
            e.Handled = true;
        }

        private void SectionStack_MouseLeave(object sender, MouseEventArgs e)
        {
            Book_MouseMove(null, null);
            Section_MouseMove(null, null);
            e.Handled = true;
        }

        private void comboBoxConfig_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ;
        }

        private void Highlighting_Changed(object sender, RoutedEventArgs e)
        {
            ; // nothing to do
        }

        public bool reentrant = false;
        private void Lexicon_Init()
        {
            reentrant = true;

            if (MAIN.Settings.Lexicon.Domain.Value == QLexicon.QLexiconVal.BOTH)
            {
                this.LexSearchAV.IsChecked = true;
                this.LexSearchAVX.IsChecked = true;
            }
            else if (MAIN.Settings.Lexicon.Domain.Value == QLexicon.QLexiconVal.AVX)
            {
                this.LexSearchAV.IsChecked = false;
                this.LexSearchAVX.IsChecked = true;
            }
            else if (MAIN.Settings.Lexicon.Domain.Value == QLexicon.QLexiconVal.AV)
            {
                this.LexSearchAV.IsChecked = true;
                this.LexSearchAVX.IsChecked = false;
            }

            if (MAIN.Settings.Lexicon.Render.Value == QLexicon.QLexiconVal.BOTH)
            {
                this.LexRenderAV.IsChecked = true;
                this.LexRenderAVX.IsChecked = true;
            }
            else if (MAIN.Settings.Lexicon.Render.Value == QLexicon.QLexiconVal.AVX)
            {
                this.LexRenderAV.IsChecked = false;
                this.LexRenderAVX.IsChecked = true;
            }
            else if (MAIN.Settings.Lexicon.Render.Value == QLexicon.QLexiconVal.AV)
            {
                this.LexRenderAV.IsChecked = true;
                this.LexRenderAVX.IsChecked = false;
            }

            reentrant = false;
        }

        private void Lexicon_Changed(object sender, RoutedEventArgs e)
        {
            if (reentrant)
            {
                e.Handled = true;
                return;
            }
            reentrant = true;
            bool searchAV  = this.LexSearchAV .IsChecked.HasValue && this.LexSearchAV .IsChecked.Value;
            bool searchAVX = this.LexSearchAVX.IsChecked.HasValue && this.LexSearchAVX.IsChecked.Value;

            bool renderAV  = this.LexRenderAV .IsChecked.HasValue && this.LexRenderAV .IsChecked.Value;
            bool renderAVX = this.LexRenderAVX.IsChecked.HasValue && this.LexRenderAVX.IsChecked.Value;

            bool forceSearchUpdate = !(searchAV || searchAVX);
            bool forceRenderUpdate = !(renderAV || renderAVX);

            if (forceSearchUpdate && forceRenderUpdate)
            {
                MAIN.Settings.Lexicon.Domain.Value = QLexicon.QLexiconVal.AV;
                MAIN.Settings.Lexicon.Render.Value = QLexicon.QLexiconVal.AV;

                searchAV  = true;
                searchAVX = true;

                MAIN.Settings.Update();
            }
            else if (forceSearchUpdate)
            {
                MAIN.Settings.Lexicon.Domain.Value = MAIN.Settings.Lexicon.Render.Value;
                searchAV  = renderAV;
                searchAVX = renderAVX;

                MAIN.Settings.Update();
            }
            else if (forceRenderUpdate)
            {
                MAIN.Settings.Lexicon.Render.Value = MAIN.Settings.Lexicon.Domain.Value;
                renderAV  = searchAV;
                renderAVX = searchAVX;

                MAIN.Settings.Update();
            }
            else
            {
                if (searchAV && searchAVX)
                    MAIN.Settings.Lexicon.Domain.Value = QLexicon.QLexiconVal.BOTH;
                else if (searchAV)
                    MAIN.Settings.Lexicon.Domain.Value = QLexicon.QLexiconVal.AV;
                else if (searchAVX)
                    MAIN.Settings.Lexicon.Domain.Value = QLexicon.QLexiconVal.AVX;

                if (renderAV && renderAVX)
                    MAIN.Settings.Lexicon.Render.Value = QLexicon.QLexiconVal.BOTH;
                else if (renderAV)
                    MAIN.Settings.Lexicon.Render.Value = QLexicon.QLexiconVal.AV;
                else if (renderAVX)
                    MAIN.Settings.Lexicon.Render.Value = QLexicon.QLexiconVal.AVX;

                MAIN.Settings.Update();
            }
            if (forceSearchUpdate || forceRenderUpdate)
            {
                if (searchAV)
                    this.LexSearchAV.IsChecked = true;
                if (searchAVX)
                    this.LexSearchAV.IsChecked = true;

                if (renderAV)
                    this.LexRenderAV.IsChecked = true;
                if (renderAVX)
                    this.LexRenderAV.IsChecked = true;
            }
            MAIN.ButtonAVT_Click(sender, e);
            reentrant = false;
        }
    }
}

