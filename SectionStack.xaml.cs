using System;
using System.Collections.Generic;
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
        public delegate void BookChooser(uint bookNum);

        public Label[] labels;

        Dictionary<string, Grid> ClickManager;
        Dictionary<string, Label> SectionMap;
        Dictionary<string, uint> BookMap;

        private BookChooser Selection;
        private static SectionStack SELF;
        public static void SetBookSelector(BookChooser selection)
        {
            SectionStack.SELF.Selection = selection;
        }
        public SectionStack()
        {
            InitializeComponent();
            this.Selection = null;
            SectionStack.SELF = this;

            BookMap = new Dictionary<string, uint>();
            SectionMap = new Dictionary<string, Label>();
            SectionMap.Add(this.OTlaw.Name, this.OTlaw);
            SectionMap.Add(this.OThistory.Name, this.OThistory);
            SectionMap.Add(this.OTpoetry.Name, this.OTpoetry);
            SectionMap.Add(this.OTmajorprophets.Name, this.OTmajorprophets);
            SectionMap.Add(this.OTminorprophets.Name, this.OTminorprophets);
            SectionMap.Add(this.NTgospelsANDacts.Name, this.NTgospelsANDacts);
            SectionMap.Add(this.NTChurch.Name, this.NTChurch);
            SectionMap.Add(this.NTPastoral.Name, this.NTPastoral);
            SectionMap.Add(this.NTOtherEpistles.Name, this.NTOtherEpistles);

            ClickManager = new Dictionary<string, Grid>();
            ClickManager.Add(this.OTlaw.Name, this.PanelLaw);
            ClickManager.Add(this.OThistory.Name, this.PanelHistory);
            ClickManager.Add(this.OTpoetry.Name, this.PanelPoetry);
            ClickManager.Add(this.OTmajorprophets.Name, this.PanelMajorProphets);
            ClickManager.Add(this.OTminorprophets.Name, this.PanelMinorProphets);
            ClickManager.Add(this.NTgospelsANDacts.Name, this.PanelGospelsAndActs);
            ClickManager.Add(this.NTChurch.Name, this.PanelChurchEpistles);
            ClickManager.Add(this.NTPastoral.Name, this.PanelPastoralEpistles);
            ClickManager.Add(this.NTOtherEpistles.Name, this.PanelOtherEpistles);

            this.labels = new Label[66];

            this.labels[0] = this.B01;
            this.labels[1] = this.B02;
            this.labels[2] = this.B03;
            this.labels[3] = this.B04;
            this.labels[4] = this.B05;
            this.labels[5] = this.B06;
            this.labels[6] = this.B07;
            this.labels[7] = this.B08;
            this.labels[8] = this.B09;
            this.labels[9] = this.B10;
            this.labels[10] = this.B11;
            this.labels[11] = this.B12;
            this.labels[12] = this.B13;
            this.labels[13] = this.B14;
            this.labels[14] = this.B15;
            this.labels[15] = this.B16;
            this.labels[16] = this.B17;
            this.labels[17] = this.B18;
            this.labels[18] = this.B19;
            this.labels[19] = this.B20;
            this.labels[20] = this.B21;
            this.labels[21] = this.B22;
            this.labels[22] = this.B23;
            this.labels[23] = this.B24;
            this.labels[24] = this.B25;
            this.labels[25] = this.B26;
            this.labels[26] = this.B27;
            this.labels[27] = this.B28;
            this.labels[28] = this.B29;
            this.labels[29] = this.B30;
            this.labels[30] = this.B31;
            this.labels[31] = this.B32;
            this.labels[32] = this.B33;
            this.labels[33] = this.B34;
            this.labels[34] = this.B35;
            this.labels[35] = this.B36;
            this.labels[36] = this.B37;
            this.labels[37] = this.B38;
            this.labels[38] = this.B39;
            this.labels[39] = this.B40;
            this.labels[40] = this.B41;
            this.labels[41] = this.B42;
            this.labels[42] = this.B43;
            this.labels[43] = this.B44;
            this.labels[44] = this.B45;
            this.labels[45] = this.B46;
            this.labels[46] = this.B47;
            this.labels[47] = this.B48;
            this.labels[48] = this.B49;
            this.labels[49] = this.B50;
            this.labels[50] = this.B51;
            this.labels[51] = this.B52;
            this.labels[52] = this.B53;
            this.labels[53] = this.B54;
            this.labels[54] = this.B55;
            this.labels[55] = this.B56;
            this.labels[56] = this.B57;
            this.labels[57] = this.B58;
            this.labels[58] = this.B59;
            this.labels[59] = this.B60;
            this.labels[60] = this.B61;
            this.labels[61] = this.B62;
            this.labels[62] = this.B63;
            this.labels[63] = this.B64;
            this.labels[64] = this.B65;
            this.labels[65] = this.B66;

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
            uint booknum = this.BookMap[book];
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
    }
}

