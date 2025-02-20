### These are instructions for installing AV-Bible *(Version 9.25.2.11)*

#### Requirements:

- Windows 11 (x64 -- System Type: 64-bit -- as revealed by \<System Information\> app, found via Windows Start-Menu)
- AV-Bible 2025 now provides the option of installing the AV-Bible Addin for Microsoft Word

#### Prerequisite Downloads:

***Prerequisite I -*** Download and Install *Microsoft .NET 8.0 Desktop Runtime (v8.0.11)*

[click here](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.11-windows-x64-installer)

***Prerequisite II -***  Download and Install *Microsoft Visual C++ Runtime*

[click here](https://aka.ms/vs/17/release/vc_redist.x64.exe)

#### AV-Bible Setup Instructions:

**STEP #1:**  Download and Save this setup program:<br/>
https://github.com/kwonus/AVBible/raw/main/Installation/Setup/AV-Bible-2025-Setup.exe</br>

![Screen-Shot](./images/AV-Bible-2025-Save.png)

**STEP #2:**  Click on the downloads icon. Next click on the ellipses [ ... ] and click Keep</br>

![Screen-Shot](./images/AV-Bible-2025-Keep.png)

**STEP #3:**  Click on the <u>Show more</u>.</br>

![Screen-Shot](./images/AV-Bible-2025-More.png)

**STEP #4:**  Click <u>Keep anyway</u></br>

![Screen-Shot](./images/AV-Bible-2025-Trust.png)

**STEP #5:**  Click on the <u>Open file</u> link.</br>

![Screen-Shot](./images/AV-Bible-2025-Open.png)

**STEP #6:**  Initiate App setup by accepting license agreement and clicking \<Next\></br>
![Screen-Shot](./images/Setup.png)

**STEP #7:**  Select App features and then click \<Next\></br>
![Screen-Shot](./images/Features.png)

- To enable the standard AV-Bible Windows application, check the first box. Or select "Compact installation" from the pull-down menu.
- To enable AV-Bible Addin for for Microsoft Word, check the final two boxes. The AV-Bible Addin requires the AV-Data-Manager. To enable AV-Bible features inside of Microsoft Word, both features need be enabled simutaneously.
- To enable all features, check all boxes or select "Full installation" from the pull-down menu.
- It is possible to install AV-Data-Manager without installing any other features. This option is useful when Migrating from the Microsoft Store App edition of AV-Bible to the full-featured application found here. Otherwise, installing ***only*** AV-Data-Manager will not be of any use.

**STEP #8:**  Click \<Next\> until you see this dialog</br>
![Screen-Shot](./images/Finish.png)

- Always leave \<Launch AV-Bible\> checked. Otherwise the Word Addin will be non-functional.

**STEP #9:**  Click \<Finish\> and your installation is complete.

- To assure continued functionality of AV-Bible integration with Microsoft Word, AV-Data-Manager will launch automatically every time you logon to your computer. The easiest way to disable AV-Data-Manager is to uninstall the AV-Bible app.



#### AV-Bible Usage Instructions:

The program that you just installed is named "AV-Bible". An optional desktop icon can be added during setup. This simplifies  program launch.

**Once it is running ...**

You should be able to view help files by clicking on the help menu pull-down:</br>

![Screen-Shot](./images/avbible-help.png)



#### AV-Bible inside of Microsoft Word:

If you installed the AV-Bible Addin for Microsoft, the Word application will present you with two additional tabs on the ribbon:</br>
![Screen-Shot](./images/Ribbon.png)

The two ribbons are quite similar. Both the Old Testament (OT) tab and the New Testament (NT) tab offer these common functions:

- Find Any Verse
  - The \<find Any Verse\> button searches both OT and NT books, regardless of which tab it was launched from
  - The button shows up on both tabs.
- AV-Bible Settings
  - This button allows you to customize behavior.
  - The button shows up on both tabs.
- User Help
  - This button allows you to browse the help files of the AV-Bible Addin for Microsoft Word.
  - The button shows up on both tabs.

Other buttons are distinct between the two tabs:

- Old Testament tab
  - Insert Any Verse allows ***only*** OT book selections 
  - Quick access buttons to any book in the OT
- New Testament tab
  - Insert Any Verse allows ***only*** NT book selections 
  - Quick access buttons to any book in the NT

See User Help on either tab for additional guidance on app usage.

#### AV-Bible Removal:

Should you want to delete this application, this can be accomplished using the standard Microsoft Windows Control Panel:

- Add or remove programs

AV Bible writes to your user-specific AppData folder. If your username were "JohnDoe", this folder would be named"C:\Users\JohnDoe\AppData\Roaming\AV-Bible". This folder contains your search history, your macro definitions, and all AV-Bible program settings. That folder can be safely deleted if none of that information is of use to you.

Alternatively, that folder can be copied to new machines, should you want to capture it for an installations onto other computers.
