using AVXLib.Memory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Windows.Media.Streaming.Adaptive;
using Blacklight.Controls.Wpf;
using Markdig;
using Neo.Markdig.Xaml;
using System.Windows.Media;
using Windows.Devices.Enumeration;

namespace AVBible
{
    public class QuickStart
    {
        public Dictionary<string, (string title, string Answer, string Question)> Library;
        public DragDockPanel Panel { get; private set; }

        public QuickStart(ComboBox combo)
        {
            this.Library = new()
            {
                { "quickstart_overview", ("Quick Start - Overview",          this.Overview, string.Empty) },
                { "quickstart_search",   ("Quick Start - Searching",         this.Search,   string.Empty) },
                { "quickstart_browse",   ("Quick Start - Browsing",          this.Browse,   string.Empty) },
                { "quickstart_render",   ("Quick Start - Rendering",         this.Render,   string.Empty) },
                { "quickstart_close",    ("Quick Start - Closing",           this.Close,    string.Empty) },
                { "quickstart_sizing",   ("Quick Start - Sizing",            this.Sizing,   string.Empty) },
                { "quickstart_settings", ("Quick Start - Configuration",     this.Settings, string.Empty) },
                { "quickstart_help",     ("Quick Start - Getting more Help", this.Help,     string.Empty) }
            };
            foreach (ComboBoxItem item in combo.Items)
            {
                if (this.Library.ContainsKey(item.Name))
                {
                    var qa = this.Library[item.Name];
                    qa.Question = item.Content.ToString();
                }
            }
            var doc = MarkdownXaml.ToFlowDocument(this.Overview,
            new MarkdownPipelineBuilder()
            .UseXamlSupportedExtensions()
            .Build()
            );

            this.Panel = new DragDockPanel();
            this.Panel.Content = doc;
            this.Panel.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            this.Panel.PanelLifetime = 0;
            this.Panel.PanelReference = 0;
            this.Panel.Header = this.Library["quickstart_overview"].title;
        }
        public string GetContent(string key)
        {
            (string title, string Answer, string Question) qa = this.Library.ContainsKey(key) ? this.Library[key] : ("", "", "");

            if (string.IsNullOrEmpty(qa.title))
                return string.Empty;

            return (key == "quickstart_overview")
                ? qa.Answer
                : "### " + qa.Question+ "\n" + qa.Answer;
        }

        public string Overview
        {
            get => HelpWindow.GetHelp("quick-start");
        }

        public string Search
        {
            get => """
                To search, type an expression into the search box above. Afterwards, press the \\<Enter\\> key.\\
                Search expressions are mostly intuitive [think Google or Bing]. For additional information on more
                complicated expressions and syntax. Consult Help above and select "Help with S4T Grammar"\\
               
                As AV-Bible has both an AV lexicon and a contemporary English lexicon, search can use either or both.
                The lexicon choice for search can also be controlled by selecting Lexicon under
                Configuration in the left column of the app.\\

                For additional information about the two available lexicons in AV-Bible, consult the question about
                Rendering in the Quick Start menu above.
                """;
        }

        public string Browse
        {
            get => """
                To browse books of the bible, AV-Bible organizes books:\\
                - Books are first divided by Old and New Testaments\\
                - Within the Old Testament, you'll find: [Law, History, Wisdom & Poetry, Major Prophets, Minor Prophets]\\
                - Within the New Testament, you'll find: [Gospels & Acts, Church Epistles, Pastoral Epistles, General Epistles & Revelation]\\
                - After a group of books is chosen, an individual book can be selected.\\
                - Selecting a book opens a series of chicklets at the bottom of the app. There is one chicklet per chapter.\\
                - Multiple chapters can be openned simutaneously. Click on the chapter chicklet and it will open and trun green.\\
                - Click on the chapter chicklet that is already open and it will close and turn gray.\\
                - There is also thin line under the Old and New Testament headings. Those are pulldown menus that allow selections
                of books without knowing what group the book is listed under.\\
                """;
        }

        public string Render
        {
            get => """
                AV-Bible maintains two parallel lexicons. The primary lexicon is the Authorized Version (AV). The Authorized Version
                is what the King James Version (KJV) was originally called in 1611. Choosing AV renders the bible in Elizabethan
                English, also known as Early Modern English. It is worth mentioning here that a second lexicon is also available.
                This is labeled AVX, thich stands for AV eXtensions. This lexicon uses contemporary English equivalents to semi-archaic
                words found in the Elizabethan text (e.g. hath ==> has). It also Americanizes the spellings (e.g. honour ==> honor)
                AVX even modernizes pronouns so that thou becomes
                you<sub>1</sub> and ye becomes you<sub>n</sub>. The subscripts indicate singular (1) and plural (n).\\
                
                Clicking on the big AV label in the top left of the app toggles the lexicon. It can be toggled between these three options:\\
                - AV\\
                - AVX\\
                - Side-by-Side\\
                
                By default, when Side-by-Side is enabled, difference highlighting is also enabled. This allows the user to see
                difference in the lexicon between AV and AVX.\\
                
                The lexicon choice for rendering can also be controlled by selecting Lexicon under
                Configuration in the left column of the app.\\
                """;
        }

        public string Close
        {
            get => """
                There are two mechanisms for closing Books/Chapters. The first is the most intuitive. Any chicklet is the bottom
                portion of the app that is currently rendered will be outlined in green. Clicking on the chicklet will close it.\\

                There are, however, normal situations where a chapter is already display, but its chicklet is no longer visible
                in the app. Pulling down the menu with the "Delete a panel:" label allows you to delete any panel listed
                in that menu.\\
                """;
        }

        public string Sizing
        {
            get => """
                Like most Windows Desktop applications, this app window is sizable using standard Windows control mechanisms.
                Additionally Portions of the app and be collapsed and expanded by clickin on the various white triangles.
                """;
        }

        public string Settings
        {
            get => """
                The easiest way to change settings in AV-Bible is using the Configuration options,
                found in the left column of the app.\\
                
                Alternatively, AV-Bible allows free-form commands in the search box. One of the supported commands is
                \@Help. Another command is @Set. Another is @Get. For help on available settings, you can execute
                either of these commands in the search box:\\
                - \@help set\\
                - \@help get\\

                To see a list of all settings and their current values, you can execute
                this command in the search box:\\
                - \@get\\
                """;
        }

        public string Help
        {
            get => """
                Pulling down the menu with the "Help:" label will list all the high level help topics
                that are available in the app.\\

                Alternatively, AV-Bible allows free-form commands in the search box. One of the supported commands is
                \@Help. For example to get help on the Search for Truth topic, you can execute this command
                in the search box:\\
                \@help s4t
                """;

        }

        public DragDockPanel GetPanel(string topic)
        { 
            if (this.Library.ContainsKey(topic))
            {
                this.Panel.Header = this.Library[topic].title;
                this.Panel.Content = this.GetContent(topic);
            }
            else
            {
                this.Panel.Header = this.Library["quickstart_overview"].title;
                this.Panel.Content = this.GetContent("quickstart_overview");
            }
            return this.Panel;
        }
    }
}
