## Background

Near the end of the last century, Google pioneered the modern search interface by employing an elegantly simple "search box". This was an evolution away from the complex interfaces that preceded it. Still, it becomes problematic when we want to search for multiple terms, unless we expect to merely "match every term".

Search-for-Truth (S4T) query language provides a concise yet comprehensive syntax for searching and studying the scriptures, configuring, and controlling the AV-Bible application. It gets its name from the Bereans found in Acts 17:11-12, along with those Christians mentioned in 2 Thessalonians 12-13, and Paul's instruction to Timothy in II Timothy 2:15.  God instructs us to "study" his word, and S4T can streamline the mechanics of that activity (only the soul and spirit can actively study the scriptures). In any event, S4T grammar supports Boolean operations such as AND, OR, and NOT. Great care has been taken to support the construction of complex queries. Greater care has been taken to maintain a clear and concise syntax.

S4T is consistent with itself. S4T always favors simplicity over versatility. Avoiding complexity of syntax, makes the grammar easier to explain. S4T also avoids nuance (we chose not to imitate "i" before "e" except after "c"). Without complexity and nuance, S4T is easy to learn, easy to type, and easy to remember.  In fact, search expressions often look no different than they would appear today in a Google or Bing search. Incidentally, S4T is a dialect of Quelle and conforms to the Quelle Specification.

### Overview of S4T Grammar

There are two types of S4T statements

| Statement Type       | Syntax                                                       |
| -------------------- | ------------------------------------------------------------ |
| Selection Statement  | Combines search criteria and scoping filters for tailored verse selection.<br />Configuration settings can also be combined and incorporated into the selection criteria. |
| Imperative Statement | single action for configuration and/or application control (cannot be combined with other actions) |

#### Selection Criteria (includes search operations)

Selection Statement contains Selection Criteria, followed by an <u>optional</u> Directive:

The selection criteria controls how verses are selected. It is made up of one to three blocks. The ordering of blocks is partly prescribed. When present, the expression block must be in the initial position. The scoping block and/or the settings-block follow the expression block (when expressed). So long as scoping clauses are grouped into a single block <u>and</u> the settings clauses are grouped into a single block, those two blocks can be in either order (so long as they are listed after the expression block when it is expressed).

- Search Expression Block
- Settings Block
- Scoping Block

| Block Position                         | Block Type                  | Hashtag Utilization Level |
| -------------------------------------- | --------------------------- | ------------------------- |
| ***initial***                          | **Search Expression Block** | full utilization          |
| *after expression block when provided* | **Settings Block**          | partial utilization       |
| *after expression block when provided* | **Scoping Block**           | partial utilization       |

An optional directive can be issued following the selection criteria.  Only zero or one directives can be issued within a  statement:

- Macro Directive

- Export Directive

To be clear, a macro cannot be created for a statement that exports selection/search results to a file. Directives must be instigated separately.  The syntax for these directives is straightforward

| Directive Type                                               | Directive Syntax *(follows the Selection Criteria)*          |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| Macro (*apply* tag to macro)                                 | ***\|\| tag***                                               |
| Export Block (*export* results of selection criteria to a file) | ***> filepath*** or<br />***>> filepath*** or<br />***?> filepath*** or <br />***:= format*** *(reserved for internal use only)* or <br />***:: format*** *(reserved for internal use only)* |

#### Imperative Statements

Non-selection statements instigate configuration changes or application control. These statements always begin with **@**. They are executed individually and cannot be combined with any other actions. Imperatives that begin with \@ cannot be combined with search expressions. They are always discrete/singleton commands.

#### Configuration Statements

S4T supports three categories of configuration. These are described more completely in Section 2.

| Configuration Targets | Configuration Actions        |
| --------------------- | ---------------------------- |
| User Settings         | \@set, \@get, \@clear, \@use |
| User Macros           | \@view, \@delete             |
| User History          | \@view, \@delete             |

#### Control Statements

S4T has [up to] five control statements. These are described more completely in Section 3.

| Description of Action    | Control Actions     | Parameter   | Availability                             |
| ------------------------ | ------------------- | ----------- | ---------------------------------------- |
| Application/Topical Help | \@help              | ***topic*** | available in both editions               |
| Application Termination  | \@exit              | -           | available in both editions               |
| Data Backup and Restore  | \@backup, \@restore | -           | available only in "AV Bible"             |
| Data Migration           | \@migrate           | -           | available only in "AV-Bible for Windows" |

***Note:*** There are two editions of AV-Bible, one edition is available on the Microsoft Store, called "AV-Bible for Windows". A second, more full-featured edition, is found at the AV-Bible project home on GitHub and simply called "AV Bible". Both editions currently only run on Windows, name differentiation exists merely to help you recognize which edition that you are running. More information can be found in Section 3 of this help document.

## Section 1 - Selection/Search

From a linguistic standpoint, all S4T statements are issued in the imperative. The subject of the verb is always "you understood". As the user, you are commanding the application what to do. Some statements have additional guiding parameters. These parameters instruct what the command is to act upon.

Consider these two examples of S4T statements (first Configuration, followed by Search):

\@lexicon.searh = KJV

"Moses"

Notice that both statements above are single actions.  We should have a way to express both of these in a single command. And this is why selection criteria contains multiple blocks.  The previous two actions into a single compound statement, issue this command:

"Moses" lexicon.search = KJV

It should be noted that these two imperatives, while similar, do not have identical effects:

- *@search.lexicon = KJV*
- *+search.lexicon = KJV*

The former is a configuration imperative: it changes the lexicon setting for all future searches. Whereas, the latter is merely a temporary block assigment: it only affects Selection Statement of which it is a part. Subsequent searches are unaffected by block assignments (they are always temprorary). There are times when a user will want a setting to persist, and other times when she does not. S4T permits the user to choose.

#### QuickStart

Consider this proximity search (find Moses and Aaron within a single span of seven words):

*Moses Aaron  +span = 7*

S4T syntax can specify the lexicon to search, by also supplying temporary settings:

*Moses Aaron +span = 7  +lexicon.search = KJV*

The statement above has assigns two settings in the context of search. The search criteria, with the settings means that both Aaron and Moses are required to appear within 7 words of each other, but both names must be present to constitute a match.

Next, consider a search to find Moses <u>or</u> Aaron:

*Moses|Aaron*

The order in which the search terms are provided is insignificant. Additionally, the type-case is insignificant. And either name found would constitute a match.

Of course, there are times when word order is significant. Accordingly, searching for explicit strings can be accomplished using double-quotes as follows:

*"Moses said ... Aaron"*

These constructs can even be combined. For example:

*"Moses said ... Aaron|Miriam"*

In all cases, “...” means “followed by”, but the ellipsis allows other words to appear between "said" and "Aaron". Likewise, it allows words to appear between "said" and "Miriam".

S4T is designed to be intuitive. It provides the ability to invoke Boolean logic for term-matching and/or linguistic feature-matching. As we saw above, the pipe symbol ( | ) can be used to invoke an *OR* condition.

### 1.1 - Selection Criteria

As we saw in the overview, there three blocks that compose Selection Criteria:

- Expression Block Components

- *find expression*
- *complete hashtag utilization*
- Settings Block Components

- *assign setting*

- *partial hashtag utilization*
- Scoping Block

- *filter directives*
- *partial hashtag utilization*

| Action    | Type             | Position | Action Syntax                  | Repeatable Action                                     |
| --------- | ---------------- | -------- | ------------------------------ | ----------------------------------------------------- |
| *find*    | Expression Block | initial  | search expression or ***#id*** | **no**                                                |
| *utilize* | Expression Block | initial  | ***#tag***                     | **no**: only one hashtag is permitted per block       |
| *assign*  | Settings Block   | initial  | ***+setting = value***         | yes (e.g. ***+format=md +lexicon=kjv +span=verse*** ) |
| *utilize* | Settings Block   | initial  | ***+ #tag***                   | **no**: only one macro is permitted per block         |
| *filter*  | Scoping Block    | post     | ***< scope***                  | yes (e.g. ***< Genesis 3 < Revelation 1-3***)         |
| *utilize* | Scoping Block    | post     | **<** ***#tag*** | **no**: only one macro is permitted per block |

**Table 1-1** - Summary of actions expressible in the Selection Criteria segment of a Selection/Search imperative statement

Two mutually exclusive optional directives can be issued following the selection criteria. 

#### 1.1.1 - Search Expression Block

The ampersand symbol can similarly be used to represent *AND* conditions upon terms. As an example. the English language contains words that can sometimes as a noun , and other times as some other part-of-speech. To determine if the bible text contains the word "part" where it is used as a verb, we can issue this command:

"part&/verb/"

The SDK, provided by Digital-AV, has marked each word of the bible text for part-of-speech. With the rich syntax of S4T, this type of search is easy and intuitive.

Of course, part-of-speech expressions can also be used independently of an AND condition, as follows:

"/noun/ ... home" + span = 6

That search would find phrases where a noun appeared within a span of six words, preceding the word "home"

**Valid statement syntax, but no results:**

this&that

/noun/ & /verb/

Both of the statements above are valid, but will not match any results. Search statements attempt to match actual words in  the actual bible text. A word cannot be "this" **and** "that". Likewise, an individual word in a sentence does not operate as a /noun/ **and** a /verb/ at the same time.

**Negating search-terms Example:**

Consider a query for all passages that contain a word beginning with "Lord", followed by any word that is neither a verb nor an adverb:

"Lord\* -/v/ & -/adv/" + span = 15

#### 1.1.2 - Settings Block

When the same setting appears more than once, only the last setting in the list is preserved.  Example:

+format=md  +format=text

The assignment would be to text.  We call this: "last assignment wins".

Finally, there is a bit more to say about the similarity setting, because it actually has three components. If we issue this command, it affects similarity in two distinct ways:

+similarity = 85%

That command is a concise way of setting two values. It is equivalent to this command

+similarity.word=85%  +similarity.lemma=85%

That is to say, similarity is operative for the lexical word and also the lemma of the word. While not discussed previously, these two similarities thresholds need not be identical. These commands are also valid:

+similarity.word=85%  +similarity.lemma=95%

+similarity.word=85%

+similarity.word=off  +similarity.lemma=100%

+similarity.lemma=off

the lexicon controls operate in a similar manner:

+lexicon=KJV 

That command is a concise way of setting two values. It is equivalent to this command

+lexicon.search=KJV  +lexicon.render=KJV

That is to say, lexicon is operative for searching and rendering. Like the similarity setting, the lexicon setting can also diverge between search and render parts. A common lexicon setting might be:

+lexicon.search=both +lexicon.render=KJV

That setting would search both the KJV (aka AV) lexicon and a modernized lexicon (aka AVX), but verse rendering would only be in KJV.

#### 1.1.3 - Scoping Block

Sometimes we want to limit the scope of our search. Say that I want to find mentions of the serpent in Genesis. I can search only Genesis by executing this search:

serpent < Genesis

If I also want to search in Genesis and Revelation, this works:

serpent < Genesis < Revelation

Filters also allow Chapter and Verse specifications. To search for the serpent in Genesis Chapter 3, we can do this:

serpent < Genesis 3

Abbreviations are also supported:

vanity < sos < 1co



### 1.2 - Macro Directive

| Macro Directive *(follows the Selection Criteria)* | Syntax for applying tag to create a macro |
| -------------------------------------------------- | ----------------------------------------- |
| *apply*                                            | ***\|\| tag***                            |

**Table 1-2** - Syntax summary for the *apply* action in the Macro Directive of a Selection/Search imperative statement.

Tagged statements are also called macros. All macros are defined with a hash-tag (#);

Macro tags cannot contain punctuation: only letters, numbers, hyphens, and underscores are permitted. However, macros are identified with a hashtag (#).


Let’s say we want to name the search example from the previous section; We’ll call it *eternal-power*. To accomplish this, we can apply a tag to the statement below:

eternal power < Romans  +span=7 similarity=85% || eternal-power-romans

It’s that simple, now instead of typing the entire statement, we can utilize the macro by referencing our previously applied tag. Here is how the macro is utilized:

#eternal-power-romans



### 1.3 - Export Directive

| Export Directive  *(follows the Selection Criteria)* | Create file       | Create or Overwrite file | Create or Append File |
| ---------------------------------------------------- | ----------------- | :----------------------- | :-------------------- |
| *export*                                             | **?>** *filename* | **>** *filename*         | **>>** *filename*     |

**Table 1-3** - Syntax summary for the *export* action in the Export Directive of a Selection/Search imperative statement.

This would export all verses in Genesis 1 from the most previous search as html

#in_beginning  > my-macro-output.html  +format=html

This would export all verses for the executed macro as markdown

#in_beginning  > my-macro-output.html  +format=markdown

Combining only with a scoping black , we could append Genesis chapter two, to an existing file:

< Genesis 2  >> C:\users\my-user-name\documents\existing-file.md  +format=markdown

Combining with a scoping black , we could replace the contents of an existing file with Genesis chapter three:

< Genesis 3  > C:\users\my-user-name\documents\existing-file.md  +format=markdown

If you instead want to prevent accidental overwrites of previous exports, this syntax only succeeds if the file does not already exist:

< Genesis 3  **?**> C:\users\my-user-name\documents\existing-file.md  +format=markdown

### 1.4 - Macro Utilization

The *utilize* action is supported in each of the three Selection Criteria blocks:

- Expression *(full hashtag utilization)*
- Settings *(partial hashtag utilization)*
- Scoping *(partial hashtag utilization)*

Each of the block types supports hashtag utilization. However, each block limited to, at most, a single utilization. The *utilization* references a hashtag (either for a macro, revealed by the @macros command; or for a previous command, revealed by the \@history imperative). As there are  a maximum of three blocks in the selection criteria, a statement could contain up to three *utilizations* (one per block).

The expression block supports full hashtag utilization.  In the earlier example:

#eternal-power-romans

All settings, filters, and search criteria are utilized (this is called full macro utilization, and it can only occur in expression blocks)

Expression block macros sometimes undergo demotion. A macro within the expression block is demoted into a partial macro when a provided block within the selection criteria conflicts with the macro definition. Consider these examples:

Recall that the macro definition: eternal power < Romans  +span=7 +similarity=85%  || eternal-power-romans

| Macro Statement                      | Utilization level         | Explanation                                           |
| ------------------------------------ | ------------------------- | ----------------------------------------------------- |
| #eternal-power-romans                | full macro utilization    | no conflicts                                          |
| #eternal-power-romans < Acts         | partial macro utilization | explicit filter replaces any filters defined in macro |
| #eternal-power-romans +span=7 < Acts | partial macro utilization | only the search expression is utilized from the macro |

**Table 1-4** - Macro utilization in a Search Expression

Outside of the expression block, partial macros *utilize* only the part of the macro that applies to the block type. For example, this clause utilizes only the settings defined within the macro.:

+ #eternal-power-romans

Likewise, in this example, this clause utilizes only the filters defined within the macro.

< #eternal-power-romans

Macro utilization within a block disallows all other entries within the block; macro utilization in a block is not compatible with any other entries in that same block.

Specifically, the following statements / clauses are not supported by S4T:

**NOT SUPPORTED:**  #eternal-power-romans without excuse

**NOT SUPPORTED:**  + #eternal-power-romans  +span=7

**NOT SUPPORTED:**  < #eternal-power-romans < Acts

It should be noted that any macros referenced within a macro definition are expanded prior to applying the new tag. Therefore, subsequent redefinition of a previously referenced macro invocation never affects existing macro definitions. We call this macro-determinism.  All control settings are captured at the time that the tag is applied to the macro. This further assures that the same search results are returned each time the macro is referenced. Here is an example.

\@set span = 2

in beginning || in_beginning

\@set span = 3

#in_beginning +span=1 < genesis 1

***result:*** none

### 1.5 - History Utilization

Just like macro utilization, history *utilization* is supported in each of the three Selection Criteria blocks:

- Expression *(full hashtag utilization)*
- Settings *(partial hashtag utilization)*
- Scoping *(partial hashtag utilization)*

As previously stated, each of the block types supports hashtag *utilization* . However, each block limited to, at most, one *utilization*. Hashtags can be discovered via the \@history command. As there are a maximum of three blocks in the selection criteria, there are a maximum of three utilizations per statement (one per block).

Only the expression block supports full hashtag utilization.

Expression block macros sometimes undergo demotion. A historic utilization within the expression block is demoted into a partial macro when a provided block within the selection criteria conflicts with the macro definition. Assume that this command is identified by the \@view command by id := 5:

"in ... beginning" +span=3 +similarity=85% < Genesis < John

| Statement         | Utilization level   | Explanation                                           |
| ----------------- | ------------------- | ----------------------------------------------------- |
| #5                | full utilization    | no conflicts                                          |
| #5 < Acts         | partial utilization | explicit filter replaces any filters defined in macro |
| #5 +span=7 < Acts | partial utilization | only the search expression is utilized from the macro |

**Table 1-5** - History utilization in a Search Expression

Outside of the expression block, partial *usage* applies by block type. For example, this clause utilizes only the settings defined where id = 5.

\+ #5

Likewise, in this example, this clause utilizes only the filters for id = 5.

< #5

Just like macros, utilization within a block disallows all other entries within the block; utilization in a block is not compatible with any other entries in that same block.

Specifically, the following statements / clauses are not supported by S4T:

**NOT SUPPORTED:**  #5 without excuse

**NOT SUPPORTED:**  + #5 + span = 7

**NOT SUPPORTED:**  < #5 < Acts

It should be noted that any historic id references are expanded prior to applying the new tags for macros. As mentioned in the previous section, we call this macro-determinism.  Therefore, even if an id is removed from the search history with the \@delete command, any macros that it were referenced within, continue to behave identically post-deletion.


## Section 2 - Configuration Statements

### 2.1 - Macros

| Action             | Syntax                                                       |
| ------------------ | ------------------------------------------------------------ |
| **@view**          | *tag*                                                        |
| **@use**           | *tag*                                                        |
| **@delete**        | *tag*                                                        |
| **@macros**        | *wildcard*  ***<- OR ->***   ***from*** {DATE} <u>and/or</u> ***to*** {DATE} |
| **@macros delete** | *wildcard*  ***<- OR ->
***   ***from*** {DATE} <u>and/or</u> ***to*** {DATE} |

**TABLE 2-1** -- **Viewing and deleting macros**

**DATE PARAMETER:**<br />{DATE}: any of:

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

### 2.2 - History

| Verb                | Parameters                                                   |
| ------------------- | ------------------------------------------------------------ |
| **@view**           | *tag*                                                        |
| **@use**            | *tag*                                                        |
| **@delete**         | *tag*                                                        |
| **@history**        | *<u>optional:</u>*  ***from*** {DATE} <u>and/or</u> ***to*** {DATE} |
| **@history delete** | ***from*** {DATE} <u>and/or</u> ***to*** {DATE}              |

**TABLE 2-2** -- **Viewing & deleting history**

**DATE PARAMETER:**<br />{DATE}: any of:

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

To reveal all history since tag:5 [inclusive]:

All ranges are inclusive.

**History Utilization**

The hashtag *utilization* works for command-history works exactly the same way as it does for macros.  After issuing a *@view* command, the user might receive a response as follows.

*@view 3*

3> eternal power

And the command can be re-invoked as follows:

#3

That would be an abbreviated way to execute a search specified as:

eternal power

Again, *utilizing* a command from your command history is *used* just like a macro. Moreover, as with macros, control settings are persisted within your command history to provide invocation determinism. That means that control settings that were in place during the original command are utilized for the invocation.

Command history captures all settings. We have already discussed macro-determinism. Invoking commands by their history tags behave exactly as invoking with macro tags. Specifically, invoking command history never persists changes into your environment, unless you explicitly request such behavior with the \@use command.

**RESETTING COMMAND HISTORY**

The *@history* command can be used to remove <u>all</u> command history. To remove all command history:

*@history delete all*

FROM / TO parameters can limit the scope of the delete command:

*@history delete from 12/31/2023 to 4/17/2024*

### 2.3 - Settings

| Action     | Parameters                   |
| ---------- | ---------------------------- |
| **@clear** | *setting* or ALL             |
| **@get**   | *setting* or ALL or revision |
| **@set**   | *setting* **=** *value*      |
| **@use**   | ***tag***                    |

**TABLE 2-3.a** - **Listing of additional CONTROL actions**



**Export Format Options:**

| Export Content Type | Command              | Synonym          |
| ------------------- | -------------------- | ---------------- |
| **Markdown**        | *@format = markdown* | *@format = md*   |
| **Text** (UTF8)     | *@format = text*     | *@format = utf8* |
| **HTML**            | *@format = html*     |                  |
| **YAML**            | *@format = yaml*     |                  |

**TABLE 2-3.b** - **@set** format command can be used to set the default content-formatting for use with the export verb



| **example**       | **explanation**                                              | shorthand equivalent |
| ----------------- | ------------------------------------------------------------ | -------------------- |
| **@set** span = 8 | Assign a control setting                                     | **@span** = 8        |
| **@get** span     | get a control setting                                        | n/a                  |
| **@clear** span   | Clear the setting; restores the setting to its default value | n/a                  |

**TABLE 2-3.c** - **set/clear/get** action operate on configuration settings

In all, AVX-Quelle manifests six user-controlled settings. Each allows all three actions: ***set***, ***@clear***, and ***@get*** verbs. There are two additional settings that are effectively read-only, albeit, ALL does work with *@clear* Table 2.3.d lists all settings .

| Setting Name     | Shorthand | Meaning                                                      | Values                                         | Default Value |
| ---------------- | --------- | ------------------------------------------------------------ | ---------------------------------------------- | ------------- |
| span             | -         | proximity distance limit (can be "verse" or number of words) | verse or<br /> 1 to 999                        | verse         |
| lexicon          | -         | Streamlined syntax for setting lexicon.search<br /> and lexicon.render to the same value | av or avx or dual<br />(kjv or modern or both) | n/a           |
| lexicon.search   | search    | the lexicon to be used for searching                         | av or avx or dual<br />(kjv or modern or both) | dual / both   |
| lexicon.render   | render    | the lexicon to be used for display/rendering                 | av/avx (kjv/modern)                            | av / kjv      |
| format           | -         | format of results on output                                  | see Table 7                                    | text / utf8   |
| similarity       | -         | Streamlined syntax for setting similarity.word<br />and similarity.lemma to the same value<br />Phonetics matching threshold is between 33% and 100%. 100% represents an exact sounds-alike match. Any percentage less than 100, represents a fuzzy sounds-similar match <br />Similarity matching can be completely disabled by setting this value to off | 33% to 100% **or** *off*                       | off           |
| similarity.word  | word      | fuzzy phonetics matching as described above, but this prefix only affects similarity matching on the word. | 33% to 100% **or** *off*                       | off           |
| similarity.lemma | lemma     | fuzzy phonetics matching as described above, but this prefix only affects similarity matching on the lemma. | 33% to 100% **or** *off*                       | off           |
| revision         | -         | Not really a true setting: it works with the *@get* command to retrieve the revision number of the S4T grammar supported by AV-Engine. This value is read-only. | 5.x.yz                                         | n/a           |
| ALL              | -         | ALL is an aggregate setting: it works with the *@clear* command to reset all variables above to their default values. It is used with *@get* to fetch all settings. | n/a                                            | n/a           |

**TABLE 2-3.d** - Summary of AV-Bible Settings

The *@get* command fetches these values. The *@get* command requires a single argument. Examples are below:

*@get* span

*@get* format

All settings can be cleared using an explicit command:

*@clear* ALL

**Persistence of Settings**

It should be noted that there is a distinction between **@set** and and ***assign*** actions. The first action is an application configuration-imperative, and it is persistent (it affects all subsequent statements). Contrariwise, the ***assign*** action affects only the single statement wherein it is executed. We refer to this distinction as *persistence* vs *assignment*.

### 2.4 - Miscellaneous Information

**QUERYING DRIVER FOR VERSION INFORMATION**

This command reveals the current Grammar revision of the Search-for-Truth (S4T) query language, implemented  in AV-Bible:

*@get* revision

---

In general, the S4T command processor can be thought of as a stateless server. The only exceptions of its stateless nature are:

1. non-default settings assigned using the **@set** command

2. defined macro tags.

3. command history



## Section 3 - Control Statements

As of this writing, there are two mechanisms available for installing AV-Bible. In effect, there are two editions.

- The Microsoft Store edition of AV-Bible (Window has "AV-Bible for Windows" in the title bar)

- Full-featured installation from the GitHub repository (AV Bible):

  https://github.com/kwonus/AV-Bible/tree/main/Installation/Instructions

### 3.1 - Program Help

To get general help, use this command:

*@help*

Or for specific topics:

*@help* find

*@help* set

*@help* export

etc ...

### 3.2 - Backing up history and macros

Available only in the The full-featured edition of AV-Bible (installation available on GitHub project site)

To perform a backup, type this command:

*@backup*

This command is useful for situations: like replacing an old computer with a new one. If you want to transfer your search history and macro tags to the new computer, create a backup on the old computer. The backup can be found in this folder:
C:\Users\your-username\AppData\AV-Bible

***Note 1:*** AppData is a hidden folder and can be accessed by explicitly typing the full path location in file-path box at the top of the Windows File Explorer. Also note that "your-username" need be replaced with your actual name on your computer (this is often your first-name or your first and last name). If you first browse to C:\Users, it's ussually obvious what your username is on your computer.

***Note 2:*** If you installed AV-Bible on your old computer via the Microsoft Store, data migration must be performed prior to your backup. See section 3.4 for data migration instructions. After data migration has been performed, the steps of backup and restore can be performed as document in section 3.3.

If you are unsure if you installed AV-Bible from the Microsoft Store, simply launch AV-Bible on your computer. If the title of the application is "AV-Bible for Windows", then it was installed via the Microsoft Store. Full featured applications have this in the title bar: "AV Bible". A full-featured installation provides additional capabilities, including backup and restore. Microsoft Store applications explicitly disallow writing files to you local computer, and thus prevents direct backup and restore. Follow the instructions in section 3.4 to perform data migration to the full-featured AV-Bible application.

### 3.3 - Restoring a previous backup 

Available only in the The full-featured edition of AV-Bible (installation available on GitHub project site)

To perform a restore, type this command:

*@restore*

This command is useful for situations: like replacing an old computer with a new one. After you have installed AV-Bible on your new computer, you can restore the back-up from your old computer to the new one. Follow these steps:

1. On old computer, run this command: \@backup inside of AV-Bible application

2. On old computer, locate the backup on the old computer. It can be found in this folder:
   C:\Users\your-username\AppData\AV-Bible

3. Copy these two files from the folder onto removable media:

   - backup.yaml 
   - settings.yaml

4. Install AV-Bible from the GitHub repository, using these instructions:

   https://github.com/kwonus/AV-Bible/tree/main/Installation/Instructions

5. Copy the two *.yaml files from the removable to this folder on the new computer:

   C:\Users\your-username\AppData\AV-Bible

6. On new computer, run this command:

   \@restore   (inside of the AV-Bible application)

***Note:*** AppData is a hidden folder and can be accessed by explicitly typing the full path location in file-path box at the top of the Windows File Explorer. Also note that "your-username" need be replaced with your actual name on your computer (this is often your first-name or your first and last name). If you first browse to C:\Users, it's ussually obvious what your username is on your computer.

### 3.4 - Data Migration

Available only in the The Microsoft Store edition of AV-Bible.

The \@migrate command is similar to the @backup command. As already mentioned above, there are two editions of AV-Bible. Your current edition supports either \@migrate or \@backup, but not both commands.

- The Microsoft Store edition of AV-Bible (Window has "AV-Bible for Windows" in the title bar)

  - this edition supports the @migrate command

- Full-featured installation from the GitHub repository (AV Bible):

  https://github.com/kwonus/AV-Bible/tree/main/Installation/Instructions

  - this edition supports the @backup command (see Section 3.2)


Data can be migrated from AV-Bible for Windows into the full-featured AV Bible installation. ***Do not remove*** your AV-Bible for Windows application, because removal of the Microsoft Store app simultaneously removes all application data (including history and macros). Instead, install the full-featured AV-Bible from GitHub, following these instructions:

1. Launch the Microsoft Store App edition of AV-Bible. It will show "AV-Bible for Windows" in the title bar. Leave this app running.

2. On the same computer with the existing Microsoft Store app, Install AV-Bible from the GitHub repository, using these instructions:

   https://github.com/kwonus/AV-Bible/tree/main/Installation/Instructions

   Be sure to select both of these options

   - Install AV-Bible Desktop Application
   - Install AV-Data-Manager

3. When prompted by the installation, be sure to finalize your installation by selecting:
   [x] Launch AV-Data-Manager.exe

4. Bring the Microsoft Store App, namely "AV-Bible for Windows" back to the top. At this point, after the installation, there might also be a second new installation running with "AV Bible" in the title bar. Do not use the newly installed app for this step)

5. Verify that you see "AV-Bible for Windows" in the title bar

6. Type this command:

   \@migrate

   If no errors appear, data migration has been initiated.

7. Close the window, titled "AV-Bible for Windows"

8. Next, launch your new installation of AV-Bible, and bring that window to the top. It should have "AV Bible" in the title bar.

9. run this command:

   \@restore   (inside of the AV-Bible application)

10. If no errors appear, data migration is complete and you have successfully migrated your previous history and macros into the new installation.

11. While not required, It is now safe to remove the redundant Microsoft Store application

### 3.5 - Exiting the Application

Type this to terminate the app:

*@exit*


## Section 4 - Glossary of S4T Terminology

**Actions:** Actions are complete verb-clauses issued in the imperative [you-understood].  Many actions have one or more parameters.  But just like English, a verb phrase can be a single word with no explicit subject and no explicit object.  Consider this English sentence:

Go!

The subject of this sentence is "you understood".  Similarly, all verbs are issued without an explicit subject. The object of the verb in the one word sentence above is also unstated.  S4T operates in an analogous manner.  Consider this English sentence:

Go Home!

Like the earlier example, the subject is "you understood".  The object this time is defined, and insists that "you" should go home.  Some verbs always have objects, others sometimes do, and still others never do. S4T follows this same pattern and some S4T verbs require direct-objects; and some do not.  In the various tables throughout this document, required and optional parameters are identified, These parameters represent the object of the verb within each respective table.

**Selection Criteria**: Selection what text to render is determined with a search expression, scoping filters, or both.

**Search Expression**: The Search Expression has fragments, and fragments have features. For an expression to match, all fragments must match (Logical AND). For a fragment to match, any feature must match (Logical OR). AND is represented by &. OR is represented by |.

**Unquoted SEARCH clauses:** an unquoted search clause contains one or more search fragments. If there is more than one fragment in the clause, then each fragment is logically AND’ed together.

**Quoted SEARCH clauses:** a quoted clause contains a single string of terms to search. An explicit match on the string is required. However, an ellipsis ( … ) can be used to indicate that other terms may silently intervene within the quoted string.

- It is called *quoted,* as the entire clause is sandwiched on both sides by double-quotes ( " )
- The absence of double-quotes means that the statement is unquoted

**Booleans and Negations:**

**and:** In Boolean logic, **and** means that all terms must be found. With S4T, **and** is represented by terms that appear within an unquoted clause. **And** logic is also available on each search-term by using the **&** operator.

**or:** In Boolean logic, **or** means that any term constitutes a match. With S4T, **and** is represented per each search-term by using the **|** operator.

**not:** In Boolean logic, means that the feature must not be found. With S4T, *not* is represented by the hyphen ( **-** ) and applies to individual features within a fragment of a search expression. It is best used in conjunction with other features, because any non-match will be included in results.

hyphen ( **-** ) means that any non-match satisfies the search condition. Used by itself, it would likely return every verse. Therefore, it should be used judiciously.

## Section 5 - Specialized Search tokens in AV-Bible

The lexical search domain of S4T includes all words in the original KJV text. It can also optionally search using a modernized lexicon of the KJV (e.g. hast and has; this is controllable with the search.lexicon setting).  The table below lists linguistic extensions available in S4T.

| Search Term        | Operator Type                           | Meaning                                                      | Maps To                                                      | Mask   |
| ------------------ | --------------------------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------ |
| un\*               | wildcard (example)                      | starts with: un                                              | all lexicon entries that start with "un"                     | 0x3FFF |
| \*ness             | wildcard (example)                      | ends with: ness                                              | all lexicon entries that end with "ness"                     | 0x3FFF |
| un\*ness           | wildcard (example)                      | starts with: un<br />ends with: ness                          | all lexicon entries that start with "un", and end with "ness" | 0x3FFF |
| \*profit\*         | wildcard (example)                      | contains: profit                                             | all lexicon entries that contain both "profit"               | 0x3FFF |
| \*pro\*fit\*       | wildcard (example)                      | contains: pro and fit                                        | all lexicon entries that contain both "pro" and "fit" (in any order) | 0x3FFF |
| un\*profit*ness    | wildcard (example)                      | starts with: un<br />contains: profit<br />ends with: ness     | all lexicon entries that start with "un", contain "profit", and end with "ness" | 0x3FFF |
| un\*pro\*fit\*ness | wildcard (example)                      | starts with: un<br />contains: pro and fit<br />ends with: ness | all lexicon entries that start with "un", contain "pro" and "fit", and end with "ness" | 0x3FFF |
| ~ʃɛpɝd*            | phonetic wildcard (example)             | Tilde marks the wildcard as phonetic (wildcards never perform sounds-alike searching) | All lexical entries that start with the sound ʃɛpɝd (this would include shepherd, shepherds, shepherding...) | TBD    |
| ~ʃɛpɝdz            | sounds-alike search using IPA (example) | Tilde marks the search term as phonetic (and if similarity is set between 33 and 99, search handles approximate matching) | This would match the lexical entry "shepherds" (and possibly similar terms, depending on similarity threshold) | TDB    |
| \(is)              | lemma                                   | search on all words that share the same lemma as is: be, is, are, art, ... | be is are art ...                                            | 0x3FFF |
| /noun/             | lexical marker                          | any word where part of speech is a noun                      | POS12::0x010                                                 | 0x0FF0 |
| /n/                | lexical marker                          | synonym for /noun/                                           | POS12::0x010                                                 | 0x0FF0 |
| /verb/             | lexical marker                          | any word where part of speech is a verb                      | POS12::0x100                                                 | 0x0FF0 |
| /v/                | lexical marker                          | synonym for /verb/                                           | POS12::0x100                                                 | 0x0FF0 |
| /pronoun/          | lexical marker                          | any word where part of speech is a pronoun                   | POS12::0x020                                                 | 0x0FF0 |
| /pn/               | lexical marker                          | synonym for /pronoun/                                        | POS12::0x020                                                 | 0x0FF0 |
| /adjective/        | lexical marker                          | any word where part of speech is an adjective                | POS12::0xF00                                                 | 0x0FFF |
| /adj/              | lexical marker                          | synonym for /adjective/                                      | POS12::0xF00                                                 | 0x0FFF |
| /adverb/           | lexical marker                          | any word where part of speech is an adverb                   | POS12::0xA00                                                 | 0x0FFF |
| /adv/              | lexical marker                          | synonym for /adverb/                                         | POS12::0xA00                                                 | 0x0FFF |
| /determiner/       | lexical marker                          | any word where part of speech is a determiner                | POS12::0xD00                                                 | 0x0FF0 |
| /det/              | lexical marker                          | synonym for /determiner/                                     | POS12::0xD00                                                 | 0x0FF0 |
| /preposition/      | lexical marker                          | any word where part of speech is a preposition               | POS12::0x400                                                 | 0x0FF0 |
| /prep/             | lexical marker                          | any word where part of speech is a preposition               | POS12::0x400                                                 | 0x0FF0 |
| /1p/               | lexical marker                          | any word where it is inflected for 1st person (pronouns and verbs) | POS12::0x100                                                 | 0x3000 |
| /2p/               | lexical marker                          | any word where it is inflected for 2nd person (pronouns and verbs) | POS12::0x200                                                 | 0x3000 |
| /3p/               | lexical marker                          | any word where it is inflected for 3rd person (pronouns, verbs, and nouns) | POS12::0x300                                                 | 0x3000 |
| /singular/         | lexical marker                          | any word that is known to be singular (pronouns, verbs, and nouns) | POS12::0x400                                                 | 0xC000 |
| /plural/           | lexical marker                          | any word that is known to be plural (pronouns, verbs, and nouns) | POS12::0x800                                                 | 0xC000 |
| /WH/               | lexical marker                          | any word that is a WH word (e.g., Who, What, When, Where, How) | POS12::0xC00                                                 | 0xC000 |
| /BoB/              | transition marker                       | any word where it is the first word of the book (e.g. first word in Genesis) | TRAN::0xE0                                                   | 0xF0   |
| /BoC/              | transition marker                       | any word where it is the first word of the chapter           | TRAN::0x60                                                   | 0xF0   |
| /BoV/              | transition marker                       | any word where it is the first word of the verse             | TRAN::0x20                                                   | 0xF0   |
| /EoB/              | transition marker                       | any word where it is the last word of the book (e.g. last word in revelation) | TRAN::0xF0                                                   | 0xF0   |
| /EoC/              | transition marker                       | any word where it is the last word of the chapter            | TRAN::0x70                                                   | 0xF0   |
| /EoV/              | transition marker                       | any word where it is the last word of the verse              | TRAN::0x30                                                   | 0xF0   |
| /Hsm/              | segment marker                          | Hard Segment Marker (end) ... one of \. \? \!                | TRAN::0x40                                                   | 0x07   |
| /Csm/              | segment marker                          | Core Segment Marker (end) ... \:                             | TRAN::0x20                                                   | 0x07   |
| /Rsm/              | segment marker                          | Real Segment Marker (end) ... one of \. \? \! \:             | TRAN::0x60                                                   | 0x07   |
| /Ssm/              | segment marker                          | Soft Segment Marker (end) ... one of \, \; \( \) --          | TRAN::0x10                                                   | 0x07   |
| /sm/               | segment marker                          | Any Segment Marker (end)  ... any of the above               | TRAN::!=0x00                                                 | 0x07   |
| /_/                | punctuation                             | any word that is immediately marked for clausal punctuation  | PUNC::!=0x00                                                 | 0xE0   |
| /!/                | punctuation                             | any word that is immediately followed by an exclamation mark | PUNC::0x80                                                   | 0xE0   |
| /?/                | punctuation                             | any word that is immediately followed by a question mark     | PUNC::0xC0                                                   | 0xE0   |
| /./                | punctuation                             | any word that is immediately followed by a period (declarative) | PUNC::0xE0                                                   | 0xE0   |
| /-/                | punctuation                             | any word that is immediately followed by a hyphen/dash       | PUNC::0xA0                                                   | 0xE0   |
| /;/                | punctuation                             | any word that is immediately followed by a semicolon         | PUNC::0x20                                                   | 0xE0   |
| /,/                | punctuation                             | any word that is immediately followed by a comma             | PUNC::0x40                                                   | 0xE0   |
| /:/                | punctuation                             | any word that is immediately followed by a colon (information follows) | PUNC::0x60                                                   | 0xE0   |
| /'/                | punctuation                             | any word that is possessive, marked with an apostrophe       | PUNC::0x10                                                   | 0x10   |
| /)/                | parenthetical text                      | any word that is immediately followed by a close parenthesis | PUNC::0x0C                                                   | 0x0C   |
| /(/                | parenthetical text                      | any word contained within parenthesis                        | PUNC::0x04                                                   | 0x04   |
| /Italics/          | text decoration                         | italicized words marked with this bit in punctuation byte    | PUNC::0x02                                                   | 0x02   |
| /Jesus/            | text decoration                         | words of Jesus marked with this bit in punctuation byte      | PUNC::0x01                                                   | 0x01   |
| /delta/            | lexicon                                 | [archaic] word can be transformed into modern American English |                                                              |        |
| [type]             | named entity                            | Entities are recognized by MorphAdorner. They are also matched against Hitchcock's database. This functionality is experimental and considered BETA. | type=person man<br />woman tribe city<br />river mountain<br />animal gemstone<br />measurement any<br />any_Hitchcock |        |
| \[FFFF\]           | PN+POS(12)                              | hexadecimal representation of bits for a PN+POS(12) value.   | See Digital-AV SDK                                           | uint16 |
| \[FFFFFFFF\]       | POS(32)                                 | hexadecimal representation of bits for a POS(32) value.      | See Digital-AV SDK                                           | uint32 |
| \[string\]         | nupos-string                            | NUPOS string representing part-of-speech. This is the preferred syntax over POS(32), even though they are equivalent. NUPOS part-of-speech values have higher fidelity than the 16-bit PN+POS(12) representations. | See Part-of-Speech-for-Digital-AV.docx                       | uint32 |
| 99999:H            | Strongs Number                          | decimal Strongs number for the Hebrew word in the Old Testament | One of Strongs\[4\]                                          | 0x7FFF |
| 99999:G            | Strongs Number                          | decimal Strongs number for the Greek word in the New Testament | One of Strongs\[4\]                                          | 0x7FFF |

## Section 6 - S4T conformance to the Quelle specification

Quelle specifies two possible implementation levels:

- Level 1 [basic search support]
- Level 2 [search support includes also searching on part-of-speech tags]

S4T is a Level 2 Quelle implementation with augmented search capabilities. S4T extends Quelle to include AVX-Framework-specific constructs.  These extensions provide additional specialized search features and the ability to manage two distinct lexicons for the biblical texts.

1. S4T represents the biblical text with two substantially similar, but distinct, lexicons. The search.lexicon setting can be specified by the user to control which lexicon is to be searched. Likewise, the render.lexicon setting is used to control which lexicon is used for displaying the biblical text. As an example, the KJV text of "thou art" would be modernized to "you are".

- AV/KJV *(a lexicon that faithfully represents the KJV bible; AV purists should select this setting)*

- AVX/Modern *(a lexicon that that has been modernized to appear more like contemporary English)*

- Dual/Both *(use both lexicons)*

The Dual/Both setting for lexicon.search indicates that searching should consider both lexicons. The The Dual/Both setting for lexicon.render indicates that results should be displayed for both renderings [whether this is side-by-side or in-parallel depends on the format and the application where the display-rendering occurs]. Left unspecified, the lexicon setting applies to lexicon.search and lexicon.render components.

2. S4T provides support for fuzzy-match-logic. The similarity setting can be specified by the user to control the similarity threshold for approximate matching. An exact lexical match is expected when similarity is set to *off*.

Phonetics matches are enabled when similarity is set between 33% and 100%. Similarity is calculated based upon the phonetic representation for the word.

The minimum permitted similarity threshold is 33%. Any similarity threshold between 1% and 32% produces a syntax error.

A similarity setting of 100% still uses phonetics, but expects an exact phonetic match (e.g. "there" and "their" are a 100% phonetic match).

AV-Bible uses the AV-1769 edition of the sacred text. It substantially agrees with the "Bearing Precious Seed" bibles, as published by local church ministries. The text itself has undergone review by Christian missionaries, pastors, and lay people since the mid-1990's. The original incarnation of the digitized AV-1769 text was implemented in the free PC/Windows app known as:

- AV-Bible - 1995 Edition for Windows 95 & Windows NT 3.5
- AV-Bible - 1997 Edition for Windows 95 & Windows NT 4.0
- AV-Bible - 1999 Edition for Windows 98 & Windows NT 4.0
- AV-Bible - 2000 Edition for Windows Me & Windows 2000
- AV-Bible - 2007 Edition for Windows XP
- AV-Bible - 2011 Edition for Windows Vista
- AV-Bible - 2021 Edition for Windows 10
- AV-Bible - 2024 Edition for Windows 11 (experimental release; initial release to support S4T)
- AV-Bible - 2025 Edition for Windows 11 (current release with minor maintenance updates from 2024 baseline)

Decades ago, AV-Bible (aka AV-1995, AV-1997, ... AV-2011), were found on internet bulletin boards and the now defunct bible.advocate.com website. More recent legacy versions are still available at the avbible.net website. Modern editions are distributed on the Microsoft store.

Please see https://Digital-AV.org for additional information about the SDK.

