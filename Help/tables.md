### STATEMENT TYPES

| Statement Type      | Syntax                                                       |
| ------------------- | ------------------------------------------------------------ |
| Selection Criteria  | Combines search criteria and scoping filters for tailored verse selection.<br/>Configuration settings can also be combined and incorporated into the selection criteria. |
| Discrete Imperative | single action for configuration and/or application control (cannot be combined with other actions) |

### BLOCK POSITIONS

| Block Position                         | Block Type                  | Hashtag Utilization Level |
| -------------------------------------- | --------------------------- | ------------------------- |
| ***initial***                          | **Search Expression Block** | full utilization          |
| *after expression block when provided* | **Settings Block**          | partial utilization       |
| *after expression block when provided* | **Scoping Block**           | partial utilization       |

### DIRECTIVE TYPES
| Directive Type                                               | Directive Syntax *(follows the Selection Criteria)*          |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| Macro (*apply* tag to macro)                                 | ***\|\| tag***                                               |
| Export Block (*export* results of selection criteria to a file) | ***> filepath*** or<br/>***>> filepath*** or<br/>***=> filepath*** |

### CONFIGURATION TARGETS
| Configuration Targets | Configuration Actions           |
| --------------------- | ------------------------------- |
| User Settings         | \@set, \@get, \@clear, \@absorb |
| User Macros           | \@view, \@delete                |
| User History          | \@view, \@delete                |

### CONTROL TARGETS
| Control Targets   | Control Actions | Optional Parameter | Description                         |
| ----------------- | --------------- | ------------------ | ----------------------------------- |
| Usage Information | \@help          | topic              | Help with S4T syntax and usage      |
| System Control    | \@exit          | -                  | Exit the application or interpreter |

### IMPLICIT ACTIONS
| Action   | Type             | Position | Action Syntax                            | Repeatable Action                                     |
| -------- | ---------------- | -------- | ---------------------------------------- | ----------------------------------------------------- |
| *find*   | Expression Block | initial  | search expression or ***#id***           | **no**                                                |
| *use*    | Expression Block | initial  | ***#tag*** or ***#id ***                 | **no**: only one macro is permitted per block         |
| *assign* | Settings Block   | initial  | ***+setting = value***                   | yes (e.g. ***+format=md +lexicon=kjv +span=verse*** ) |
| *use*    | Settings Block   | initial  | ***+ #tag*** or<br/>***+ #id***          | **no**: only one macro is permitted per block         |
| *filter* | Scoping Block    | post     | ***< scope***                            | yes (e.g. ***< Genesis 3 < Revelation 1-3***)         |
| *use*    | Scoping Block    | post     | **<** ***#tag***  or<br/>**<** ***#id*** | **no**: only one macro is permitted per block         |

### MACRO DIRECTIVE
| Macro Directive *(follows the Selection Criteria)* | Syntax for applying tag to create a macro |
| -------------------------------------------------- | ----------------------------------------- |
| *apply*                                            | ***\|\| tag***                            |

### EXPORT DIRECTIVE
| Export Directive  *(follows the Selection Criteria)* | Create file      | Create or Overwrite file | Create or Append File |
| ---------------------------------------------------- | ---------------- | :----------------------- | :-------------------- |
| *export*                                             | **>** *filename* | **=>** *filename*        | **>>** *filename*     |

### MACRO Statement
| Macro Statement                      | Utilization level         | Explanation                                             |
| ------------------------------------ | ------------------------- | ------------------------------------------------------- |
| #eternal-power-romans                | full macro utilization    | no conflicts                                            |
| #eternal-power-romans +all=current   | partial macro utilization | explicit settings replace any settings defined in macro |
| #eternal-power-romans < Acts         | partial macro utilization | explicit filter replaces any filters defined in macro   |
| #eternal-power-romans + span=7 < Acts | partial macro utilization | only the search expression is utilized from the macro   |

### EXAMPLES
| Statement          | Utilization level         | Explanation                                             |
| ------------------ | ------------------------- | ------------------------------------------------------- |
| #5                 | full macro utilization    | no conflicts                                            |
| #5 +all=current    | partial macro utilization | explicit settings replace any settings defined in macro |
| #5 < Acts          | partial macro utilization | explicit filter replaces any filters defined in macro   |
| #5 + span=7 < Acts | partial macro utilization | only the search expression is utilized from the macro   |

### HISTORY and MACROS
| Action             | Syntax                                                       |
| ------------------ | ------------------------------------------------------------ |
| **@view**          | *tag*                                                        |
| **@absorb**        | *tag*                                                        |
| **@delete**        | *tag*                                                        |
| **@macros**        | *wildcard*  ***<- OR ->***   ***from*** {DATE} <u>and/or</u> ***to*** {DATE} |
| **@macros delete** | *wildcard*  ***<- OR ->***   ***from*** {DATE} <u>and/or</u> ***to*** {DATE} |



| Verb                | Parameters                                                   |
| ------------------- | ------------------------------------------------------------ |
| **@view**           | *id*                                                         |
| **@absorb**         | *id*                                                         |
| **@delete**         | *id*                                                         |
| **@history**        | *<u>optional:</u>*  ***from*** {DATE} <u>and/or</u> ***to*** {DATE} ***<- OR ->*** ***from*** *id* <u>and/or</u> ***to*** *id* |
| **@history delete** | ***from*** {DATE} <u>and/or</u> ***to*** {DATE} ***<- OR ->*** ***from*** *id* <u>and/or</u> ***to*** *id* |

### SETTINGS ACTIONS

| Action      | Parameters                   |
| ----------- | ---------------------------- |
| **@clear**  | *setting* or ALL             |
| **@get**    | *setting* or ALL or revision |
| **@set**    | *setting* **=** *value*      |
| **@absorb** | ***id*** or ***tag***        |

### EXPORT FORMATS
| Export Content Type | Command              | Synonym          |
| ------------------- | -------------------- | ---------------- |
| **Markdown**        | *@format = markdown* | *@format = md*   |
| **Text** (UTF8)     | *@format = text*     | *@format = utf8* |
| **HTML**            | *@format = html*     |                  |
| **JSON**            | *@format = json*     |                  |
| **YAML**            | *@format = yaml*     |                  |

### SETTINGS ACTIONS
| **example**       | **explanation**                                              | shorthand equivalent |
| ----------------- | ------------------------------------------------------------ | -------------------- |
| **@set** span = 8 | Assign a control setting                                     | **@span** = 8        |
| **@get** span     | get a control setting                                        | n/a                  |
| **@clear** span   | Clear the setting; restores the setting to its default value | n/a                  |

### SETTINGS (variables)
| Setting Name     | Shorthand | Meaning                                                      | Values                                        | Default Value |
| ---------------- | --------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------- |
| span             | -         | proximity distance limit (can be "verse" or number of words) | verse or<br/> 1 to 999                        | verse         |
| lexicon          | -         | Streamlined syntax for setting lexicon.search<br/> and lexicon.render to the same value | av or avx or dual<br/>(kjv or modern or both) | n/a           |
| lexicon.search   | search    | the lexicon to be used for searching                         | av or avx or dual<br/>(kjv or modern or both) | dual / both   |
| lexicon.render   | render    | the lexicon to be used for display/rendering                 | av/avx (kjv/modern)                           | av / kjv      |
| format           | -         | format of results on output                                  | see Table 7                                   | text / utf8   |
| similarity       | -         | Streamlined syntax for setting similarity.word<br/>and similarity.lemma to the same value<br/>Phonetics matching threshold is between 33% and 100%. 100% represents an exact sounds-alike match. Any percentage less than 100, represents a fuzzy sounds-similar match <br/>Similarity matching can be completely disabled by setting this value to off | 33% to 100% **or** *off*                      | off           |
| similarity.word  | word      | fuzzy phonetics matching as described above, but this prefix only affects similarity matching on the word. | 33% to 100% **or** *off*                      | off           |
| similarity.lemma | lemma     | fuzzy phonetics matching as described above, but this prefix only affects similarity matching on the lemma. | 33% to 100% **or** *off*                      | off           |
| revision         | -         | Not really a true setting: it works with the *@get* command to retrieve the revision number of the S4T grammar supported by AV-Engine. This value is read-only. | 4.x.yz                                        | n/a           |
| ALL              | -         | ALL is an aggregate setting: it works with the *@clear* command to reset all variables above to their default values. It is used with *@get* to fetch all settings. It can also be used in the settings block of a statement to override values to default or the currently saved values for situations where a macro is utilized. | n/a                                           | n/a           |
