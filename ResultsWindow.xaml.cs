namespace AVBible
{
    using Blueprint.Blue;
    using System;
    using System.IO;
    using System.Windows;
    using AVBible.TabularResults;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        private bool CanClose;
        private string Content;
        public ResultsWindow()
        {
            InitializeComponent();

            this.CanClose = false;
            this.Content = string.Empty;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.CanClose)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        public void CloseResultsWindow()
        {
            this.CanClose = true;
            this.Close();
        }

        private void DisplayResultsPanel()
        {
            this.Show();
            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }

        public void ShowResultsPanel(string content)
        {
            this.Content = content;
#if DEBUG
            string tempfile = System.IO.Path.Combine(QContext.Home, "temp-history-output.html");
#endif
            try
            {
                CustomTable table = new(string.Empty);
                string[] lines = this.Content.Split(CustomTableStyle.TR, StringSplitOptions.RemoveEmptyEntries);
                int cnt = 0;
                foreach (var row in lines)
                {
                    string text = row.Trim();
                    if (string.IsNullOrEmpty(text)) continue;
                    switch (++cnt)
                    {
                        case 1: table = new(text); break;
                        case 2: table.AddRow(text, isHeader: true); break;
                        default: table.AddRow(text); break;
                    }
                }
#if DEBUG
                var test1 = table.Foreground;
                var test2 = table.Background;
                var test3 = table.FontSize;
                var test4 = table.FontFamilies;
#endif
                table.Render(this, this.ResultsFlowDoc);
            }
            catch
            {
                ;
            }
            this.DisplayResultsPanel();
        }
    }
}
