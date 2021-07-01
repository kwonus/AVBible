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

namespace AVWord
{
    /// <summary>
    /// Interaction logic for ChapterPanel.xaml
    /// </summary>
    public partial class ViewPanel : UserControl
    {
        internal uint ID;
        internal Uri Source;
        public ViewPanel(Blacklight.Controls.Wpf.DragDockPanel panel, string text, uint id)
        {
            panel.Header = text;

            // TODO: Add delegate (search on KCW)
            //
            InitializeComponent();
            ID = id;
            Source = null;
        }
        public async Task Load(string content)
        {
            this.doc.Document.Blocks.Clear();
            var r = new Run(content);
            r.Foreground = new SolidColorBrush(Colors.White);
            var p = new Paragraph(r);
            this.doc.Document.Blocks.Add(p);
        }
        internal void ExpanderEvent(bool? minimized, bool? maximized)
        {
            int x = 0;
            x++;
        }
    }
}
