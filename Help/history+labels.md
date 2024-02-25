## HISTORY & LABELING - AV-Bible Help

### Labeling & Reviewing Statements for subsequent utilization

| Verb        | Action Type | Syntax Category | Parameters                                                   |
| ----------- | ----------- | --------------- | ------------------------------------------------------------ |
| *use*       | implicit    | LABELING        | ***$label*** <u>or</u> ***#label***                          |
| *apply*     | implicit    | LABELING        | **\|\|** ***$label***<br/><u>or</u><br/>**\|\|** ***#label*** |
| **@delete** | explicit    | LABELING        | *label*                                                      |
| **@review** | explicit    | LABELING        | **optional:** *label* <u>or</u> wildcard <u>or</u> -labels   |
| **@absorb** | explicit    | CONTROL         | **permitted:** *label*                                       |

**TABLE 4** -- **Labeling and reviewing labeled statements**

Labeled statements are also called macros. There are two types of macros:

- **FULL** macros that save the search expression and the settings & filters
- **PARTIAL** macros that save only settings & filters

Full-macros are defined with the dollar sign ($); partial macros are defined with the (#). Partial macros are useful, as they provide more liberal utilization of previously labelled statements. Quelle search syntax allows only a single search expression: this limits the application of full macros. partial-macro invocations are not restricted in that manner.  Full macro can be demoted to a partial macro by using a # instead of $ upon invocation. Partial macros can be promoted to full macros, by redefining adding a search expression and redefining the macro. 

In this section, we will examine Quelle syntax, and how macros can be used to label statements for subsequent invocation.  By applying a label to a statement, a shorthand mechanism is created for subsequent invocation. This gives rise to two new definitions:

1. Labeling a statement (or defining a macro)

2. Invoking a labeled statement (running a macro)

Macro labels cannot contain punctuation: only letters, numbers, hyphens, and underscores are permitted. However, macros are identified with a prefix: it is either a pound (#) or dollar ($).


Let’s say we want to name the search example from the previous section; We’ll call it *eternal-power*. To accomplish this, we can apply a label to the statement below. This produces a full macro:

%span=7 %similarity=85 eternal power < Romans || $eternal-power-romans

It’s that simple, now instead of typing the entire statement, we can utilize the macro by referencing our previously applied label. Here is how the macro can be utilized:

$eternal-power-romans

Labeled statements also support compounding. However, statements can only ever have a single search expression. Therefore, this macro invocation is disallowed (as there would be two search expressions in the same segment):

$eternal-power godhead

However, the control variables and filters that were in the macro can still be leveraged by demoting the label to a partial macro. We can execute this to accomplish the search that was disallowed above:

#eternal-power-romans eternal power godhead

Alternatively, the macro can be redefined (his example also release the syntax for promoting a partial macro to a full macro:

#eternal-power-romans eternal power godhead || $godhead-romans

There are a few restrictions on macro definitions:

1. The statement cannot represent an explicit action:
   - Only implicit actions are permitted in a labeled statement.
2. definition must represent a valid Quelle statement:
   - The syntax is verified prior to applying the statement label.
3. Macro definitions apply per segment
   - when + is used to concatenate searches, the macro applies only to the single expression immediately to its left

Finally, any macros referenced within a macro definition are expanded prior to applying the new label. Therefore, subsequent redefinition of a previously referenced macro invocation has no effect upon the initial macro reference. We call this macro-determinism.  Quelle determinism assures that all control settings are captured at the time that the label is applied to the macro. This further assures that the same search results are returned each time the macro is referenced. Here is an example.

@set %span = 2

in beginning || in_beginning

@set %span = 3

$in_beginning [1] < genesis:1:1

***result:*** none

However, if the user desires the current settings to be used instead, a specialized macro invocation ( #\* ) represents all currently persisted settings; just include it as the last element of the expression (as show below). 

$in_beginning #\* < genesis:1:1

***result:*** Gen 1:1 In the beginning, God created ...

Similarly, another specialized invocation ( $0 ) represents default values for all settings. As with all settings, even in the case of these specialized invocations, the last setting per expression wins.

##### Additional explicit macro commands:

Two additional explicit commands exist whereby a macro can be manipulated. We saw above how they can be defined and referenced. There are two additional ways commands that operate on macros: expansion and deletion.  In the last macro definition above where we created  $another-macro, the user could review an expansion by issuing this command:

@review another-macro

If the user wanted to remove this definition, the @delete action is used.  Here is an example:

@delete another-macro

If you want the same settings to be persisted to your current session that were in place during macro definition, the @absorb command will persist all settings for the macro into your current session

@absorb my-favorite-settings-macro 

**NOTES:**

- @absorb also works with command history.
- The two built-in specialized macro invocations ( $\* and $0 ) cannot be deleted or overwritten.

### Reviewing History for subsequent utilization

| Verb        | Action Type | Syntax Category | Parameters                                                   |
| ----------- | ----------- | --------------- | ------------------------------------------------------------ |
| *use*       | implicit    | HISTORY         | ***$id*** or ***#id***                                       |
| **@invoke** | explicit    | HISTORY         | ***@id***                                                    |
| **@delete** | explicit    | HISTORY         | **required:** -history <u>or</u> FROM <u>and/or</u> until UNTIL<br/>**optional FROM parameter :** *from* *id* <u>or</u> *from* yyyy/mm/dd<br/>**optional UNTIL parameter :** *until* *id* <u>or</u> *until* yyyy/mm/dd |
| **@review** | explicit    | HISTORY         | **required:** *id* <u>or</u> -history <u>or</u> FROM <u>and/or</u> until UNTIL<br/>**optional FROM parameter :** *from* *id* <u>or</u> *from* yyyy/mm/dd<br/>**optional UNTIL parameter :** *until* *id* <u>or</u> *until* yyyy/mm/dd |
| **@absorb** | explicit    | CONTROL         | **permitted:** *id*                                          |

**TABLE 5** -- **Reviewing statement history**

**COMMAND HISTORY** 

*@review* allows you to see your previous activity.  To show the last ten searches, type:

*@review* -history

To reveal all history up until now, type:

@review until now

To reveal all searches since January 1, 2024, type:

*@review* from 2024/1/1

To reveal for the single month of January 2024:

*@review* from 2024/1/1 until 2024/1/31

To reveal all history since id:5 [inclusive]:

*@review* from 5

All ranges are inclusive. 

**Invocation & Utilization**

The *use* command works for command-history works exactly the same way as it does for macros.  After issuing a *@history* command, the user might receive a response as follows.

*@review*

1>  @set %span = 7

2>  @set %similarity=85

3> eternal power

And the use command can utilize any command listed.

$3

would be shorthand to for the search specified as:

eternal power

Again, *utilizing* a command from your command history is *used* just like a macro. Moreover, as with macros, control settings are persisted within your command history to provide invocation determinism. That means that control settings that were in place during the original command are utilized for the invocation.

##### Invoking a command remembers all settings, except when multiple macros are compounded:

Command history captures all settings. We have already discussed macro-determinism. Invoking commands by their review numbers behave exactly like macros. In other words, invoking command history never persists changes into your environment, unless you explicitly request such behavior with the @absorb command.

**RESETTING COMMAND HISTORY**

The @delete command can be used to remove <u>all</u> command history.

To remove all command history:

@delete -history -all

FROM / UNTIL parameters can limit the scope of the @delete command.

