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
using System.Windows.Documents;

namespace AVBible
{
    public class QuickStart
    {
        private ComboBox Questions;
        private Dictionary<string, string> Answers;
        public DragDockPanel SingletonPanel { get; private set; }

        public QuickStart(ComboBox combo)
        {
            this.Questions = combo;
            this.Answers = new()
            {
                { "quickstart_overview", this.Overview },
                { "quickstart_search",   this.Search   },
                { "quickstart_browse",   this.Browse   },
                { "quickstart_render",   this.Render   },
                { "quickstart_close",    this.Close    },
                { "quickstart_sizing",   this.Sizing   },
                { "quickstart_settings", this.Settings },
                { "quickstart_help",     this.Help     }
            };
        }
        private string GetQuestionAndAnswer(string topic, string question)
        {
            string answer = this.Answers.ContainsKey(topic) ? this.Answers[topic] : string.Empty;

            return "### " + question + "\n" + answer;
        }

        public string Overview
        {
            get => HelpWindow.GetHelp("quick-start");
        }

        public string Search
        {
            get => """
                To search, type an expression into the search box above. Afterwards, press the \<Enter\> key.
                Search expressions are mostly intuitive [think Google or Bing]. For additional information on more
                complicated expressions and syntax. Consult Help above and select "Help with S4T Grammar"
               
                As AV-Bible has both an AV lexicon and a contemporary English lexicon, search can use either or both.
                The lexicon choice for search can also be controlled by selecting Lexicon under
                Configuration in the left column of the app.

                For additional information about the two available lexicons in AV-Bible, consult the question about
                Rendering in the Quick Start menu above.
                """;
        }

        public string Browse
        {
            get => """
                To browse books of the bible, AV-Bible organizes books:\
                - Books are first divided by Old and New Testaments
                - Within the Old Testament, you'll find: [Law, History, Wisdom & Poetry, Major Prophets, Minor Prophets]
                - Within the New Testament, you'll find: [Gospels & Acts, Church Epistles, Pastoral Epistles, General Epistles & Revelation]
                - After a group of books is chosen, an individual book can be selected.
                - Selecting a book opens a series of chicklets at the bottom of the app. There is one chicklet per chapter.
                - Multiple chapters can be openned simutaneously. Click on the chapter chicklet and it will open and trun green.
                - Click on the chapter chicklet that is already open and it will close and turn gray.
                - There is also thin line under the Old and New Testament headings. Those are pulldown menus that allow selections of books without knowing what group the book is listed under.
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
                you<sub>1</sub> and ye becomes you<sub>n</sub>. The subscripts indicate singular (1) and plural (n).
                
                Clicking on the big AV label in the top left of the app toggles the lexicon. It can be toggled between these three options:
                - AV
                - AVX
                - Side-by-Side
                
                By default, when Side-by-Side is enabled, difference highlighting is also enabled. This allows the user to see
                difference in the lexicon between AV and AVX.
                
                The lexicon choice for rendering can also be controlled by selecting Lexicon under
                Configuration in the left column of the app.
                """;
        }

        public string Close
        {
            get => """
                There are two mechanisms for closing Books/Chapters. The first is the most intuitive. Any chicklet is the bottom
                portion of the app that is currently rendered will be outlined in green. Clicking on the chicklet will close it.

                There are, however, normal situations where a chapter is already display, but its chicklet is no longer visible
                in the app. Pulling down the menu with the "Delete a panel:" label allows you to delete any panel listed
                in that menu.
                """;
        }

        public string Sizing
        {
            get => """
                Like most Windows Desktop applications, this app window is sizable using standard Windows control mechanisms.
                Additionally, Portions of the app and be collapsed and expanded by clickin on the various white triangles.
                """;
        }

        public string Settings
        {
            get => """
                The easiest way to change settings in AV-Bible is using the Configuration options,
                found in the left column of the app.
                
                Alternatively, AV-Bible allows free-form commands in the search box. One of the supported commands is
                \@Help. Another command is @Set. Another is @Get. For help on available settings, you can execute
                either of these commands in the search box:
                - \@help set
                - \@help get

                To see a list of all settings and their current values, you can execute
                this command in the search box:
                - \@get
                """;
        }

        public string Help
        {
            get => """
                Pulling down the menu with the "Help:" label will list all the high level help topics
                that are available in the app.

                Alternatively, AV-Bible allows free-form commands in the search box. One of the supported commands is
                \@Help. For example to get help on the Search for Truth topic, you can execute this command
                in the search box:
                - \@help s4t
                """;
        }

        public DragDockPanel GetPanel(string topic = null, string question = null)
        {
            FlowDocument doc = (topic != null) && (question != null) && this.Answers.ContainsKey(topic)
                ? MarkdownXaml.ToFlowDocument(
                    this.GetQuestionAndAnswer(topic, question),
                    new MarkdownPipelineBuilder().UseXamlSupportedExtensions().Build())
                : MarkdownXaml.ToFlowDocument(
                    this.GetQuestionAndAnswer("quickstart_overview", this.Questions.Items[0].ToString()),
                    new MarkdownPipelineBuilder().UseXamlSupportedExtensions().Build())
            ;
            string header = string.IsNullOrWhiteSpace(question) ? "Quick Start" : "Quick Start - " + question;

            this.SingletonPanel = new DragDockPanel();
            this.SingletonPanel.Content = doc;
            this.SingletonPanel.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            this.SingletonPanel.PanelLifetime = 0;
            this.SingletonPanel.PanelReference = 0;
            this.SingletonPanel.Header = header;

            return this.SingletonPanel;
        }
        public DragDockPanel UpdatePanel(string topic, string question)
        {
            FlowDocument doc = this.Answers.ContainsKey(topic)
                ? MarkdownXaml.ToFlowDocument(
                    this.GetQuestionAndAnswer(topic, question),
                    new MarkdownPipelineBuilder().UseXamlSupportedExtensions().Build())
                : MarkdownXaml.ToFlowDocument(
                    this.GetQuestionAndAnswer("quickstart_overview", this.Questions.Items[0].ToString()),
                    new MarkdownPipelineBuilder().UseXamlSupportedExtensions().Build())
            ;
            string header = string.IsNullOrWhiteSpace(question) ? "Quick Start" : "Quick Start - " + question;

            this.SingletonPanel.Content = doc;
            this.SingletonPanel.Header = header;

            return this.SingletonPanel;
        }
    }
}
