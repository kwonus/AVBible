## OUTPUT - AV-Bible Help

### EXPLICIT PRINT/EXPORT DIRECTIVE

| Verb   | Action Type | Syntax Category | 1st Parameter                   | 2nd Parameter    | Alternate<br/>*(overwrite)* | Alternate<br/>*(append)* |
| ------ | :---------: | --------------- | ------------------------------- | ---------------- | :-------------------------- | :----------------------- |
| @print |  explicit   | OUTPUT          | book:chapter<br/>example: gen:1 | **>** *filename* | **=>** *filename*           | **>>** *filename*        |

These commands would print all verses in Genesis:1 to a file named gen-1.html in my Documents folder, with the export format set to HTML.

@set %format=html

@print Genesis:1  > "C:\users\myusername\Documents\gen-1.html"

The > operator expects the file to not already exist. If it already exists, there are two options:

=> [overwrite the file]

\>\> [append to an existing file]

**Export Format Settings:**

| **Markdown**                             | **Text** (UTF8)                       | HTML             | JSON             | YAML           |
| ---------------------------------------- | ------------------------------------- | ---------------- | ---------------- | -------------- |
| *%format = md* <br/>*%format = markdown* | *%format = text*<br/>*%format = utf8* | *%format = html* | *%format = json* | %formal = yaml |


