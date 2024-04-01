namespace AVBible
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        private bool CoreInitialized;
        private bool CanClose;
        private string Content;
        public ResultsWindow()
        {
            InitializeComponent();
            this.HtmlControl.Loaded += ResultsWindow_Initialized; // this will initialize core of WebView2, and circumvents an exception being thrown.

            this.CanClose = false;
            this.CoreInitialized = false;
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

            if (this.CoreInitialized)
            {
                this.HtmlControl.NavigateToString(this.Content);
            }
            else
            {
                this.HtmlControl.Source = new Uri("about:blank");
            }
            this.DisplayResultsPanel();
        }

        private async void ResultsWindow_Initialized(object sender, RoutedEventArgs e)
        {
            // Initialize CoreWebView2
            await this.HtmlControl.EnsureCoreWebView2Async(null);

            // Load HTML content
            this.HtmlControl.NavigateToString(this.Content);

            this.CoreInitialized = true;
        }
    }
}
