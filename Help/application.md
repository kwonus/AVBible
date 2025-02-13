### AV-Bible - Basic Instructions

AV Bible provides user controls on the top and and left for searching and browsing the bible text.  In both cases, chapter references appear on the bottom as chicklets (e.g. Genesis Chapter One or "Gen 1").  Clicking on a chicklet opens that chapter on a panel in the middle of the app.  Alternatively, user HELP panels can also be opened.

Expand & Collapse controls (▲▼ and⯇ ⯈) allow the user to manage panel proportions within the application.

### The AV & AVX Lexicons

AV-Bible is implemented using a RAM-based data-model representation of the KJV bible text (Incidentally, the KJV is also referred to as the "Authorized Version" or AV).  Using a data-model for the underlying KJV text facilitates the ability of rendering the text with a modern English lexicon.  Consequently, KJV text can be dynamically transformed using this modern lexicon.  In other words, the Elizabethan English of the KJV need not be a barrier for understanding. Clicking the |AV| button and it will toggle to |AVX| and to |Side-by-Side| (incidentally, AVX is an abbreviation for AV-eXtensions).  Subsequent panel instantiations will render the text in accordance with your selected option.  Toggling does not change the existing text of of already-displayed-panels: it affects only newly opened panels.  Finally, for comparison purposes, AV chapter text can be opened side-by-side with its AVX counterpart.  Please note that AVX text renderings are always marked accordingly in the title bar of each AVX panel.

The button in the top-left-corner, as described above is the easiest way to toggle lexicon usage. Finer control is available on the Lexicon panel under the \<Configuration\> section.  Which lexicon is to be used for search can be controlled separately from which lexicon is used for rendering the results. In both search and display lexicons the choices are:

- AV/KJV
- AVX/Modern
- Dual/Both

In most situations, dual/both is the preferred setting, even if the rendering lexicon is set to KJV/AV. The dual/both setting allows a search on words like "has", "are", and "you" to return results for "hath", "art", and "ye". The rendering lexicon is matter of choice. Likewise, there are situations where it makes sense to choose a specific lexicon for searching. The lexicon toggle button and the checkboxes under configuration provide the ability to choose.

### The AV & AVX Difference Highlighting

By default, difference highlighting that show words that are different between the two distinct lexical renderings, only when verses are rendered side-by-side (which occurs when dual/both is chosen for rendering.  Opening a chapter opens both AV and AVX versions, with all differences highlighted. Opening individual books does not perform difference highlighting, by default. However, that is just the default settings.

Under the \<Configuration\> section, Difference Highlighting has three possible settings:

- Highlight Always
- Highlight Side-by-Side [default]
- Highlight Never

### Basic Workflow

Searching and browsing do not automatically render bible text into panels, they merely provide chicklet references on the bottom panel.  Clicking on any of these chicklets renders the text into a new panel.  Once a chicklet reference is rendered into a panel, the chicklet will become highlighted with a green outline.  Clicking on an already highlighted chicket will remove the rendered panel.

Searching also decorates the chicklets: chicklets with matching verses are shown with one to five dots, indicating the number of matching verses corresponding to each chapter.  Any chapter that contains more than five matching verses is marked with a horizontal line (meaning six or more verses matched the search condition).

### Panel Manipulation

Up to twelve text panels can be rendered simultaneously. If more than twelve panels are already visible, the oldest panel will be recycled. A single panel can be maximized by clicking the icon (This icon, top-right of the panel, is a toggle; a subsequent click will down-size that same panel).

Panels can also be removed by clicking on |Delete a panel| combo-box.

Help can be called up by clicking on the help button or by typing @help and an optional topic.

### AV-Bible installation options

As of this writing, there are two installation options for AV-Bible. The Windows application itself is substantially the same, regardless of which installation option is chosen. The preferred installation, the most full-featured installation, can be found at the project home of AV-Bible on GitHub. Installation instructions can be found here:

http://github.com/kwonus/AVBible/tree/main/Installation/Instructions 

If AV-Bible was installed from the GitHub project home, you will see *"AV Bible"* in the title bar of the app when it is running.

A more common installation of a more constrained AV-Bible is available on the Microsoft Store. However, restrictions on Microsoft Store Apps results in fewer features. If the app was installed from the Microsoft Store, you will see *"AV-Bible for Windows"* in the title bar.

Features of the app from a GitHub installation that are disabled in the Microsoft Store app are identified below:

- backup
- restore
- export *(exporting search results to a file)*

Moreover, one additional feature can be optionally installed via the GitHub installation:

- AV-Bible Addin for Microsoft Word (see the following section)

Upgrading to the full-featured GitHub installation is supported (See section 3 of the S4T help documentation).

### AV-Bible Addin for Microsoft Word *(AV-Bible features inside Microsoft Word)*

Certain limitations exist on applications that are showcased in the Microsoft Store. AV-Bible is no exception. Yet, additional capabilities are available with the traditional download/installer found on GitHub. Specifically. AV-Bible offers full integration with Microsoft Word. This additional capability requires direct download and installation from its project home on Github.</br>. Instructions are available here: [github.com/kwonus/AVBible/tree/main/Installation/Instructions](https://github.com/kwonus/AVBible/tree/main/Installation/Instructions/README.md)

Migration of history and macros into the full-featured application is fully supported. See the \@migrate command in the S4T help file.

