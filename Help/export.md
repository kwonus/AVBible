
| Export Directive  *(follows Selection Criteria)* |   Create file    | Create or Overwrite file | Create or Append File |
| ------ | :---------: | --------------- | ------------------------------- |
| *export*                                         | **?>** *filename* | **>** *filename*        | **>>** *filename*     |

**Table 1-3** - Syntax summary for the *export* action in the Export Directive of a Selection/Search imperative statement.

This would export all verses in Genesis 1 from the most previous search as html

#in_beginning  +format=html  > my-macro-output.html

This would export all verses for the executed macro as markdown

#in_beginning  +format=markdown > my-macro-output.html

Combining only with a scoping black , we could append Genesis chapter two, to an existing file:

< Genesis 2  >> C:\users\my-user-name\documents\existing-file.md

Combining with a scoping black , we could replace the contents of an existing file with Genesis chapter three:

< Genesis 3  > C:\users\my-user-name\documents\existing-file.md

If you want to avoid accidental overwriting of previous exports, then you can do this:

< Genesis 3  **?**> C:\users\my-user-name\documents\existing-file.md

**Export Format Options:**

| Export Content Type | Command              | Synonym          |
| ------------------- | -------------------- | ---------------- |
| **Markdown**        | *@format = markdown* | *@format = md*   |
| **Text** (UTF8)     | *@format = text*     | *@format = utf8* |
| **HTML**            | *@format = html*     |                  |
| **YAML**            | *@format = yaml*     |                  |

See the **@set** format command by issuing this imperative:

***@help set***
