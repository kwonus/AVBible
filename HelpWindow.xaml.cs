namespace AVBible
{
    using AVXFramework;
    using Blueprint.Blue;
    using Microsoft.Web.WebView2.Core;
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private bool CanClose;
        public HelpWindow()
        {
            InitializeComponent();
            this.HtmlControl.Loaded += HelpWindow_Initialized; // this will initialize core of WebView2, and circumvents an exception being thrown.
            this.CanClose = false;
        }

        private async void HelpWindow_Initialized(object sender, RoutedEventArgs e)
        {
            try
            {
                // Initialize CoreWebView2
                var env = await CoreWebView2Environment.CreateAsync(null, QContext.Home);
                await this.HtmlControl.EnsureCoreWebView2Async(env);
            }
            catch
            {
                ;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.CanClose)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        public void CloseHelpWindow()
        {
            this.CanClose = true;
            this.Close();
        }
        public void ShowHelpPanel(string request)
        {
            string path = AVEngine.GetHelpFile(request);
            this.HtmlControl.Source = new Uri(path);
            this.Show();
            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }
    }
}
