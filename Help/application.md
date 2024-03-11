### AV-Bible - Basic Instructions
AV Bible provides user controls on the top and and left for searching and browsing the bible text.  In both cases, chapter references appear on the bottom as chicklets (e.g. Genesis Chapter One or "Gen 1").  Clicking on a chicklet opens that chapter on a panel in the middle of the app.  Alternatively, user HELP panels can also be opened.

Expand & Collapse controls (▲▼ and⯇ ⯈) allow the user to manage panel proportions within the application.

### The AV & AVX Lexicons

AV-Bible is implemented using a RAM-based data-model representation of the KJV bible text (Incidentally, the official name of the KJV is the "Authorized Version" or AV).  Using a data-model for the underlying AV text facilitates the ability of rendering the AV text with a modern English lexicon.  In so doing, the AV text can be dynamically transformed using this modern lexicon.  In other words, if you are struggling with understanding Elizabethean English of the KJV, click on the |AV| button and it will toggle to |AVX| and to |Side-by-Side| (incidentally, AVX is an abbreviation for AV-eXtensions).  Subsequent panel instantiations will render the text in accordance with your selected option.  Toggling does not change the existing text of of already-displayed-panels: it affects only newly opened panels.  Finally, for comparison purposes, AV chapter text can be opened side-by-side with its AVX counterpart.  Please note that AVX text renderings are always marked accordingly in the title bar of each AVX panel.

### Basic Workflow

Searching and browsing do not automatically render bible text into panels, they merely provide chicklet references on the bottom panel.  Clicking on any of these chicklets renders the text into a new panel.  Once a chicklet reference is rendered into a panel, the chicklet will become highlighted with a green outline.  Clicking on an already higlighted chicket will remove the rendered panel.

Searching also decorates the chicklets: chicklets with matching verses are shown with one to five dots, indicating the number of matching verses corresponding to each chapter.  Any chapter that contains more than five matching verses is marked with a horizontal line (meaning six or more verses matched the search condition).

### Panel Manipulation

Up to twelve text panels can be rendered simultaneously. If more than twelve panels are already visible, the oldest panel will be recycled. A single panel can be maximized by clicking the icon (This icon, top-right of the panel, is a toggle; a subsequent click will down-size that same panel).

Panels can also be removed by clicking on |Delete a panel| combo-box.

Help can be called up by clicking on the help button or by typing @help and an optional topic.
