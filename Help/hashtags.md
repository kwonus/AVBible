
There are two types of hashtags in S4T query language. Hashtags with text labels refer to macro definitions. Hashtags with numerics reference a search command previously executed in the command history of AV-Bible.

### Macro Hashtags *(text)*

As mentioned in the general overview, an optional directive can be issued that follows the selection criteria.  The Macro Directive can be used to tag a statement for subsequent execution. It has the following form:

| Directive Type               | Directive Syntax *(follows the Selection Criteria)* |
| ---------------------------- | --------------------------------------------------- |
| Macro (*apply* tag to macro) | ***\|\| tag***                                      |

We call a statement that has a tag applied, a "macro". Here is an example:

Macro tags must begin with a letter and cannot contain punctuation: only letters, numbers, hyphens, and underscores are permitted.


Let’s say we want to name our selection criteria for easier subsequent invocation; We’ll call it *eternal-power*. To accomplish this, we can apply a tag, as shown in the following example statement:

eternal power +span=7 +similarity=85% < Romans || eternal-power-romans

It’s that simple, now instead of typing the entire statement, we can utilize the macro with a simple hashtag. Here is how the macro is utilized:

#eternal-power-romans

### History Hashtags *(numerics)*

The hashtag utilization works for command-history works exactly the same way as it does for macros.  After issuing a *@view* command to show history, the user might receive a response as follows.

*@view 3*

3> eternal power

And implicit hashtag utilization allows he reissue any previous command.

#3

would be shorthand to for the search specified as:

"Jesus answered ... and"

### Viewing & Managing Macros

| Action             | Syntax                                                       |
| ------------------ | ------------------------------------------------------------ |
| **@view**          | *tag*                                                        |
| **@use**           | *tag*                                                        |
| **@delete**        | *tag*                                                        |
| **@macros**        | *wildcard*  ***<- OR ->***   ***from*** {DATE} <u>and/or</u> ***to*** {DATE} |
| **@macros delete** | *wildcard*  ***<- OR ->***   ***from*** {DATE} <u>and/or</u> ***to*** {DATE} |

**DATE PARAMETER:**<br/>{DATE}: any of:

- yyyy/mm/dd
- yyyy-mm-dd
- mm/dd/yyyy
- mm-dd-yyyy

##### Overview of macro commands:

Viewing and Deletion of macros and history are nearly identical for single hashtag references. In the case of macros, we supply the tag name or label as a parameter to the view command:

\@view another-macro

If the user wanted to remove this definition, the \@delete action is used:

\@delete another-macro

If you want the same settings to be persisted to your current session that were in place during macro definition, the \@use command will persist all settings for the macro into your current session

\@use my-favorite-settings-macro 

**NOTE:**

​       \@use also works with search history.

### Viewing & Managing Search History

| Verb                | Parameters                                                   |
| ------------------- | ------------------------------------------------------------ |
| **@view**           | *tag*                                                        |
| **@use**            | *tag*                                                        |
| **@delete**         | *tag*                                                        |
| **@history**        | *<u>optional:</u>*  ***from*** {DATE} <u>and/or</u> ***to*** {DATE} |
| **@history delete** | ***from*** {DATE} <u>and/or</u> ***to*** {DATE}              |

**DATE PARAMETER:**<br/>{DATE}: any of:

- yyyy/mm/dd
- yyyy-mm-dd
- mm/dd/yyyy
- mm-dd-yyyy

**SEARCH HISTORY** 

*@history* allows you to see your previous activity.  To show the last ten searches, type:

*@history*

To reveal all searches since January 1, 2024, type:

*@history* from 2024/1/1

To reveal for the single month of January 2024:

*@history* from 2024/1/1 to 2024/1/31

All ranges are inclusive. 

**DELETING COMMAND HISTORY**

The *@history* command can be used to remove <u>all</u> command history. To remove all command history:

*@history delete all*

FROM / TO parameters can limit the scope of the delete command:

*@history delete from 12/31/2023 to 4/17/2024*

To delete a single hashtag in history, it works nearly identically to macros. In the case of history, we supply the tag as a parameter to the view command:

\@view 3

If the user wanted to remove this item, the \@delete action is used:

\@delete 3

### Hashtag Utilization

Hashtag utilization behaves identically for history tags and macro tags.

In the selection block of a search expression, every aspect of the hashtag is utilized (Search, scope, and settings). This is called full hashtag utilization. There is one caveat however:

- If a settings block accompanies the search expression block, the settings block overrides that portion of the hashtag
- If a scoping block accompanies the search expression block, the scoping block overrides that portion of the hashtag

The other two block types also support hashtags. Those hashtags are always partial utilization. A settings block will only utilize the settings portion of the hashtag. A scoping block will only utilize the scoping portion of the hashtag.

A maximum of three hashtags can occur in the selection criteria. Each block can have zero or one hashtags. Here is an example with three hashtag utilizations in a single statement:

#eternal-power-romans + #3 < #4
