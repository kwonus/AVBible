namespace AVBible
{
    using Blueprint.Blue;
    using Microsoft.Web.WebView2.Core;
    using System;
    using System.IO;
    using System.Windows;

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
            int attempts = 0;
Retry:
            string tempfile = System.IO.Path.Combine(QContext.Home, "temp-history-output.html");
            try
            {
                ++ attempts;
                this.HtmlControl.NavigateToString(this.Content);
                if (File.Exists(tempfile))
                {
                    File.Delete(tempfile);
                }
            }
            catch // In 9.25.1.11 version (2025/1/11), first display alwys produces an exception
            {
                if (attempts == 1)
                try
                {
                    // Initialize CoreWebView2
                    var task1 = CoreWebView2Environment.CreateAsync(null, QContext.Home);
                    task1.Wait();
                    var task2 = this.HtmlControl.EnsureCoreWebView2Async(task1.Result);

                    goto Retry;
                }
                catch
                {
                    ;
                }
                System.IO.File.WriteAllText(tempfile, this.Content);
                this.HtmlControl.Source = new Uri(tempfile);
            }
            this.Show();
            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }

        public void ShowResultsPanel(string content)
        {
            this.Content = content;
            this.DisplayResultsPanel();
        }
    }
}
