using AVWord.App;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AVWord.Wpf
{
    /// <summary>
    /// Interaction logic for ChapterChicklet.xaml
    /// </summary>
    public partial class ChapterChicklet : UserControl
    {
        public UInt16 BookChapter { get; private set; }
        public static MainWindow App { set; private get; }
        public byte Weight { set; private get; }

        public ChapterChicklet(byte book, byte chapter, byte weight, bool green)
        {
            this.BookChapter = (UInt16) ((book * 0x100) + chapter);
            this.Weight = weight;

            InitializeComponent();

            this.Show(book, chapter, green);
        }
        private bool Show(byte bookNum, byte chapterNum, bool green)
        {
            string chapter = chapterNum.ToString();

            string src = this.Picture.Source.ToString();
            this.Refresh(green);
            this.Chapter.Content = chapter;

            this.Visibility = Visibility.Visible;

            return true;
        }
        public bool Refresh(bool green)
        {
            string panel = green ? "GPanel" : "MPanel";
            string src = this.Picture.Source.ToString();
            int dash = src.LastIndexOf('-');
            int len = dash - "XPanel".Length;
            Uri uri;

            switch (this.Weight)
            {
                case 0xFF:
                    uri = new Uri(src.Substring(0, len) + panel + "-N.png");
                    break;
                case 0:
                    uri = new Uri(src.Substring(0, len) + panel + "-00.png");
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    uri = new Uri(src.Substring(0, len) + panel + "-" + this.Weight.ToString() + ".png");
                    break;
                default:
                    uri = new Uri(src.Substring(0, len) + panel + "-N.png");
                    break;
            }
            this.Picture.Source = new BitmapImage(uri);

            return true;
        }

        public void Button_Selected(object sender, RoutedEventArgs e)
        {
            this.Book.Foreground = new SolidColorBrush(Colors.White);
        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Book.Foreground = new SolidColorBrush(Colors.Cyan);
            this.Refresh(true);
            ChapterChicklet.App.AddPanel(this);
        }
    }
}
