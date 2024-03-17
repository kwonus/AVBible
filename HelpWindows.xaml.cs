namespace AVBible
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private bool CanClose;
        public HelpWindow()
        {
            InitializeComponent();
            this.CanClose = false;
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
    }
}
