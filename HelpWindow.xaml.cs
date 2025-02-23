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
    public static class Help_RustFFI
    {
        [DllImport("av_help.dll", EntryPoint = "get_library_revision")]
        public static extern UInt32 get_library_revision();

        [DllImport("av_help.dll", EntryPoint = "acquire_help")]
        internal static extern ParsedStatementHandle acquire_help(string stmt);
        [DllImport("av_help.dll", EntryPoint = "release_help")]
        internal static extern void release_help(IntPtr memory);
    }
    internal class ParsedStatementHandle : SafeHandle
    {
        public ParsedStatementHandle() : base(IntPtr.Zero, true) { }

        public override bool IsInvalid
        {
            get { return this.handle == IntPtr.Zero; }
        }

        public string AsString()
        {
            int len = 0;
            while (Marshal.ReadByte(handle, len) != 0) { ++len; }
            byte[] buffer = new byte[len];
            Marshal.Copy(handle, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                Help_RustFFI.release_help(handle);
            }
            return true;
        }
    }
    public abstract class HelpLib
    {
        public static string GetContents(string topic)
        {
            try
            {
                using (ParsedStatementHandle handle = Help_RustFFI.acquire_help(topic))
                {
                    return handle.AsString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        public static UInt32 Revision
        {
            get
            {
                try
                {
                    return Help_RustFFI.get_library_revision();
                }
                catch
                {
                    return 0;
                }
            }
        }
    }

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
                    string content = HelpWindow.GetHelp(segments[1]);

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

        public static readonly char[] splitters = ['_', '&', '+', ' ', '@'];
        internal static string GetHelp(string request)
        {
            string[] topics = request.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

            foreach (string topic in topics)
            {
                switch (topic.ToLower())
                {
                    case "search-for-truth":
                    case "s4t":
                    case "grammar": return HelpLib.GetContents("AV-Bible-S4T");

                    case "selection":
                    case "criteria":
                    case "search":
                    case "expression":
                    case "find":
                    case "scope":
                    case "scoping":
                    case "filter": return HelpLib.GetContents("selection");

                    case "ye":
                    case "thee":
                    case "thou":
                    case "thy":
                    case "early":
                    case "kjv":
                    case "language":
                    case "english": return HelpLib.GetContents("language");

                    case "settings":
                    case "assign":
                    case "set":
                    case "clear":
                    case "get":
                    case "use": return HelpLib.GetContents("settings");

                    case "macro":
                    case "history":
                    case "macros":
                    case "tags":
                    case "hashtag":
                    case "tagging":
                    case "apply":
                    case "delete":
                    case "review": return HelpLib.GetContents("hashtags");

                    case "application":
                    case "app": return HelpLib.GetContents("application");

                    case "output":
                    case "print":
                    case "export": return HelpLib.GetContents("export");

                    case "system":
                    case "command": return HelpLib.GetContents("system");
                }
            }
            return HelpLib.GetContents("application");
        }
    }
}
