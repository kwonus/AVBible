using AVSearch.Interfaces;
using AVXFramework;
using Markdig;
using Neo.Markdig.Xaml;
using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace AVBible
{
    /// <summary>
    /// Interaction logic for WindowMarkDownFlow.xaml
    /// </summary>
    public partial class QuickStart : Window
    {
        private static string[] ParseDelimiters = ["_", "Help"];

        private bool CanClose;
        public QuickStart()
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
        public void ShowQuickStart(string request)
        {
            this.Show();
            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }
    }
}
