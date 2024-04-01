namespace AVBible
{
    using System;
    using System.IO;
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
        public void ShowHelpPanel(string request)
        {
            string path = HelpWindow.GetHelpFile(request);
            this.HtmlControl.Source = new Uri(path);
            this.Show();
            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }
        private static char[] splitters = ['_', '&', '+', ' ', '@'];
        private static string? _HelpFolder = null;
        public static string GetHelpFile(string request)
        {
            string[] topics = request.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

            foreach (string topic in topics)
            {
                switch (topic.ToLower())
                {
                    case "selection":
                    case "criteria":
                    case "search":
                    case "expression":
                    case "find":
                    case "scope":
                    case "scoping":
                    case "filter": return Path.Combine(HelpFolder, "index-selection.html");

                    case "ye":
                    case "thee":
                    case "thou":
                    case "thy":
                    case "early":
                    case "kjv":
                    case "english": return Path.Combine(HelpFolder, "index-language.html");

                    case "settings":
                    case "assign":
                    case "set":
                    case "clear":
                    case "get":
                    case "absorb": return Path.Combine(HelpFolder, "index-settings.html");

                    case "macro":
                    case "history":
                    case "macros":
                    case "tags":
                    case "tagging":
                    case "invoke":
                    case "apply":
                    case "delete":
                    case "review": return Path.Combine(HelpFolder, "index-hashtags.html");

                    case "application": return Path.Combine(HelpFolder, "index-application.html");

                    case "output":
                    case "print":
                    case "export": return Path.Combine(HelpFolder, "index-export.html");

                    case "system": return Path.Combine(HelpFolder, "index-system.html");
                }
            }
            return Path.Combine(HelpFolder, "index.html");
        }
        public static string HelpFolder
        {
            get
            {
                if (_HelpFolder != null)
                    return _HelpFolder;

                string cwd = System.AppDomain.CurrentDomain.BaseDirectory;
                for (string help = Path.Combine(cwd, "Help"); help.Length > @"X:\Help".Length; help = Path.Combine(cwd, "Help"))
                {
                    if (Directory.Exists(help))
                    {
                        _HelpFolder = help;
                        return help;
                    }
                    var parent = Directory.GetParent(cwd);
                    if (parent == null)
                        break;
                    cwd = parent.FullName;
                }
                return (@"C:\Program Files\AV-Bible\Help");
            }
        }
    }
}
