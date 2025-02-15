using AVXFramework;
using Markdig;
using Neo.Markdig.Xaml;
using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AVBible
{
    /// <summary>
    /// Interaction logic for WindowMarkDownFlow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private static string[] ParseDelimiters = ["_", "Help"];

        private bool CanClose;
        public HelpWindow()
        {
            InitializeComponent();
            this.CanClose = false;

            FlowDocumentScrollViewer[] tabs = [ this.Doc_AppHelp, this.Doc_CommandHelp, this.Doc_ExportHelp, this.Doc_GrammarHelp, this.Doc_HashtagHelp, this.Doc_LanguageHelp, this.Doc_SearchHelp, this.Doc_SettingsHelp ];

            foreach (FlowDocumentScrollViewer tab in tabs)
            {
                string[] segments = tab.Name.Split(ParseDelimiters, StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length == 2)
                {
                    var mem = HelpWindow.GetHelp(segments[1]);
                    if (mem != null)
                    {
                        using (TextReader reader = new StreamReader(mem))
                        {
                            var content = reader.ReadToEnd();
                            var doc = MarkdownXaml.ToFlowDocument(content,
                                new MarkdownPipelineBuilder()
                                .UseXamlSupportedExtensions()
                                .Build()
                            );
                            tab.Document = doc;
                            // Set the page width
                            // tab.IsTwoPageViewEnabled = false;
                            // Set the page padding (top, left, bottom, right)
                            // flowDocumentReader.Document.PagePadding = new Thickness(20, 10, 20, 10);
                        }
                    }
                }
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
            this.Show();
            this.Activate();
            this.SetHelpTopic(request);
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }

        public void SetHelpTopic(string request)
        {
            string[] topics = request.Split(AVEngine.splitters, StringSplitOptions.RemoveEmptyEntries);

            TabItem selected = this.Tab_AppHelp;
            foreach (string topic in topics)
            {
                switch (topic.ToLower())
                {
                    case "search-for-truth":
                    case "s4t":
                    case "grammar": selected = this.Tab_GrammarHelp; break;

                    case "selection":
                    case "criteria":
                    case "search":
                    case "expression":
                    case "find":
                    case "scope":
                    case "scoping":
                    case "filter": selected = this.Tab_SearchHelp; break;

                    case "ye":
                    case "thee":
                    case "thou":
                    case "thy":
                    case "early":
                    case "kjv":
                    case "language":
                    case "english": selected = this.Tab_LanguageHelp; break;

                    case "control":
                    case "settings":
                    case "assign":
                    case "set":
                    case "clear":
                    case "get":
                    case "use": selected = this.Tab_SettingsHelp; break;

                    case "macro":
                    case "history":
                    case "macros":
                    case "tags":
                    case "hashtag":
                    case "tagging":
                    case "apply":
                    case "delete":
                    case "review": selected = this.Tab_HashTagHelp; break;

                    case "application":
                    case "app": selected = this.Tab_AppHelp; break;

                    case "output":
                    case "print":
                    case "export": selected = this.Tab_ExportHelp; break;

                    case "system":
                    case "command": selected = this.Tab_CommandhHelp; break;
                }
            }
            Dispatcher.BeginInvoke(new Action(() => this.HelpTabs.SelectedItem = selected), DispatcherPriority.Render);
        }
        internal static MemoryStream? GetFileContent(string name)
        {
            // The name of your resource file without the extension
            string resourceFileName = "ResourceHelp";

            // The name of the entry in the .resx file
            string resourceName = name;

            // Get the current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Create a ResourceManager
            ResourceManager resourceManager = new ResourceManager(resourceFileName, assembly);

            // Get the resource as a byte array (assuming the file is a binary resource)
            byte[] fileData = (byte[])resourceManager.GetObject(resourceName);

            return fileData != null ? new MemoryStream(fileData) : null;
        }

        public static readonly char[] splitters = ['_', '&', '+', ' ', '@'];
        private static MemoryStream? GetHelp(string request)
        {
            string[] topics = request.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

            foreach (string topic in topics)
            {
                switch (topic.ToLower())
                {
                    case "search-for-truth":
                    case "s4t":
                    case "grammar": return GetFileContent("AV-Bible-S4T");

                    case "selection":
                    case "criteria":
                    case "search":
                    case "expression":
                    case "find":
                    case "scope":
                    case "scoping":
                    case "filter": return GetFileContent("selection");

                    case "ye":
                    case "thee":
                    case "thou":
                    case "thy":
                    case "early":
                    case "kjv":
                    case "language":
                    case "english": return GetFileContent("language");

                    case "settings":
                    case "assign":
                    case "set":
                    case "clear":
                    case "get":
                    case "use": return GetFileContent("settings");

                    case "macro":
                    case "history":
                    case "macros":
                    case "tags":
                    case "hashtag":
                    case "tagging":
                    case "apply":
                    case "delete":
                    case "review": return GetFileContent("hashtags");

                    case "application":
                    case "app": return GetFileContent("application");

                    case "output":
                    case "print":
                    case "export": return GetFileContent("export");

                    case "system":
                    case "command": return GetFileContent("system");
                }
            }
            return GetFileContent("application");
        }
    }
}
