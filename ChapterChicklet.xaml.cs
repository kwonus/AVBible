using AVWord.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AVWord.Wpf
{
    /// <summary>
    /// Interaction logic for ChapterChicklet.xaml
    /// </summary>
    public partial class ChapterChicklet : UserControl
    {
        public UInt16 BookChapter { get; private set; }
        public static MainWindow App { set; private get; }

        public ChapterChicklet()
        {
            InitializeComponent();
            this.Book.Content = "?";
            this.Chapter.Content = "?";
        }
        public ChapterChicklet(byte book, byte chapter, byte weight, bool green)
        {
            this.BookChapter = (UInt16) ((book * 0x100) + chapter);

            InitializeComponent();

            this.Book.Content = "TBD";
            this.Chapter.Content = ((UInt16)chapter).ToString();

            this.Show(book, chapter, weight, green);
        }
        public bool Show(byte bookNum, byte chapterNum, byte weight, bool green)
        {
            string panel = green ? "GPanel" : "MPanel";

            string book = BookChickletMini.GetBookAbbreviation(bookNum);
            string chapter = chapterNum.ToString();

            string src = this.Picture.Source.ToString();
            Uri uri;

            switch (weight)
            {
                case 0xFF:
                    uri = new Uri(src.Substring(0, src.Length - 12) + panel + "-N.png");
                    break;
                case 0:
                    uri = new Uri(src.Substring(0, src.Length - 12) + panel + "-0.png");
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    uri = new Uri(src.Substring(0, src.Length - 12) + panel + "-" + weight.ToString() + ".png");
                    break;
                default:
                    uri = new Uri(src.Substring(0, src.Length - 12) + panel + "-0.png");
                    break;
            }
            this.Picture.Source = new BitmapImage(uri);
            this.Book.Content = book;
            this.Chapter.Content = chapter;

            this.Visibility = Visibility.Visible;

            return true;
        }
        public void Hide()
        {
            this.Visibility = Visibility.Hidden;
        }

        public void Button_Selected(object sender, RoutedEventArgs e)
        {
            this.Book.Foreground = new SolidColorBrush(Colors.White);
        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Book.Foreground = new SolidColorBrush(Colors.Cyan);
            ChapterChicklet.App.AddPanel(this);

            /*if (this.Selection != null)
            {
            this.AVPanel.Items.Add(new DragDockPanel());

            SetChapterStackParams(this.ChapCnt);
            
            var book = this.Book.Content.ToString();
                var chapter = this.Chapter.Content.ToString();

                if (sender.GetType() == typeof(Button))
                    this.Selection((Button)sender, book, uint.Parse(chapter));
                else
                    this.Selection(null, book, uint.Parse(chapter));
            }*/
        }
    }
}
