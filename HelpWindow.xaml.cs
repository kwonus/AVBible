using AVXFramework;
using Markdig;
using Neo.Markdig.Xaml;
using System;
using System.Windows;
using System.Windows.Controls;

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
                    string path = AVEngine.GetHelpFile(segments[1], asFlowDoc: true);
                    if (System.IO.File.Exists(path))
                    {
                        var content = System.IO.File.ReadAllText(path);
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

            foreach (string topic in topics)
            {
                switch (topic.ToLower())
                {
                    case "search-for-truth":
                    case "s4t":
                    case "grammar": this.HelpTabs.SelectedIndex = this.Tab_GrammarHelp.TabIndex; break;

                    case "selection":
                    case "criteria":
                    case "search":
                    case "expression":
                    case "find":
                    case "scope":
                    case "scoping":
                    case "filter": this.HelpTabs.SelectedItem = this.Tab_SearchHelp; break;

                    case "ye":
                    case "thee":
                    case "thou":
                    case "thy":
                    case "early":
                    case "kjv":
                    case "language":
                    case "english": this.HelpTabs.SelectedItem = this.Tab_LanguageHelp; break;

                    case "control":
                    case "settings":
                    case "assign":
                    case "set":
                    case "clear":
                    case "get":
                    case "use": this.HelpTabs.SelectedItem = this.Tab_SettingsHelp; break;

                    case "macro":
                    case "history":
                    case "macros":
                    case "tags":
                    case "hashtag":
                    case "tagging":
                    case "apply":
                    case "delete":
                    case "review": this.HelpTabs.SelectedItem = this.Tab_HashTagHelp; break;

                    case "application":
                    case "app": this.HelpTabs.SelectedItem = this.Tab_AppHelp; break;

                    case "output":
                    case "print":
                    case "export": this.HelpTabs.SelectedItem = this.Tab_ExportHelp; break;

                    case "system":
                    case "command": this.HelpTabs.SelectedItem = this.Tab_CommandhHelp; break;
                }
            }
            this.HelpTabs.SelectedItem = this.Tab_AppHelp;
        }
    }
}
