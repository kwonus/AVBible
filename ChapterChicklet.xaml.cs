using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AVBible
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
            InitializeComponent();

            string label = ChapterChicklet.GetBookAbbreviation(book);
            this.Book.Content = label;

            this.BookChapter = (UInt16) ((book * 0x100) + chapter);
            this.Weight = weight;

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
        static public string GetBookAbbreviation(uint num)
        {
            switch (num)
            {
                case 1: return "Gen";
                case 2: return "Ex";
                case 3: return "Lev";
                case 4: return "Num";
                case 5: return "Deut";
                case 6: return "Josh";
                case 7: return "Judg";
                case 8: return "Ruth";
                case 9: return "1Sam";
                case 10: return "2Sam";
                case 11: return "1Kin";
                case 12: return "2Kin";
                case 13: return "1Chr";
                case 14: return "2Chr";
                case 15: return "Ezra";
                case 16: return "Neh";
                case 17: return "Est";
                case 18: return "Job";
                case 19: return "Ps";
                case 20: return "Prov";
                case 21: return "Eccl";
                case 22: return "SoS";
                case 23: return "Is";
                case 24: return "Jer";
                case 25: return "Lam";
                case 26: return "Ezek";
                case 27: return "Dan";
                case 28: return "Hos";
                case 29: return "Joel";
                case 30: return "Amos";
                case 31: return "Obad";
                case 32: return "Jon";
                case 33: return "Mic";
                case 34: return "Nah";
                case 35: return "Hab";
                case 36: return "Zeph";
                case 37: return "Hag";
                case 38: return "Zech";
                case 39: return "Mal";
                case 40: return "Matt";
                case 41: return "Mark";
                case 42: return "Luke";
                case 43: return "John";
                case 44: return "Acts";
                case 45: return "Rom";
                case 46: return "1Cor";
                case 47: return "2Cor";
                case 48: return "Gal";
                case 49: return "Eph";
                case 50: return "Phil";
                case 51: return "Col";
                case 52: return "1Thes";
                case 53: return "2Thes";
                case 54: return "1Tim";
                case 55: return "2Tim";
                case 56: return "Titus";
                case 57: return "Phlm";
                case 58: return "Heb";
                case 59: return "Jas";
                case 60: return "1Pet";
                case 61: return "2Pet";
                case 62: return "1John";
                case 63: return "2John";
                case 64: return "3John";
                case 65: return "Jude";
                case 66: return "Rev";
            }
            return "X";
        }
    }
}
