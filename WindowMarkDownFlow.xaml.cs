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
    public partial class WindowMarkDownFlow : Window
    {
        private static string[] ParseDelimiters = ["_", "Help"];
        public WindowMarkDownFlow()
        {
            InitializeComponent();

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
    }
}
