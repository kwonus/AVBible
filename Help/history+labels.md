## HISTORY & LABELING - AV-Bible Help

### Reviewing Statements & Defining macros with labels

| Verb         | Action Type | Syntax Category           | Parameters                                                   |
| ------------ | ----------- | ------------------------- | ------------------------------------------------------------ |
| *invoke*     | implicit    | <u>HISTORY</u> & LABELING | ***$**id* or ***#**id*                                       |
| *invoke*     | implicit    | HISTORY & <u>LABELING</u> | ***$**label* or ***#**label*                                 |
| *apply*      | implicit    | HISTORY & <u>LABELING</u> | **\|\|** ***$**label*<br/>\<or\><br/> **\|\|** ****#**label** |
| **@delete**  | explicit    | HISTORY & <u>LABELING</u> | ***$**label* or ***#**label*                                 |
| **@review**  | explicit    | HISTORY & <u>LABELING</u> | **optional:** ***$**label* or ***#**label* or wildcard<br/>**optional:** < yyyy/mm/dd<br/>**optional:** > yyyy/mm/dd |
| **@absorb**  | explicit    | CONTROL                   | **permitted:** ***$**label* or ***#**label* or  ***$**id* or  ***#**id* |
| **@history** | explicit    | <u>HISTORY</u> & LABELING | **optional:** ***$**id* or ***$**id*<br/>**optional:** < yyyy/mm/dd<br/>**optional:** > yyyy/mm/dd<br/>**optional:** < id<br/>**optional:** > id<br/>**optional:** -reset<br>**optional:** -reset \< *id*<br/>**optional:** -reset \> *id*<br/>**optional:** -reset < yyyy/mm/dd<br/>**optional:** -reset > yyyy/mm/dd |

There are two types of macros:

- **FULL** macros that save the search expression and the settings & filters
- **PARTIAL** macros that save only settings & filters

Full-macros are defined with the dollar sign ($); partial macros are defined with the (#). Partial macros are useful, as they provide more liberal utilization of previously labelled statements. Quelle search syntax allows only a single search expression: this limits the application of full macros. partial-macro invocations are not restricted in that manner.  Full macro can be demoted to a partial macro by using a # instead of $ upon invocation. Partial macros can be promoted to full macros, by redefining adding a search expression and redefining the macro. 

In this section, we will examine Quelle syntax, and how macros can be used to label statements for subsequent invocation.  By applying a label to a statement, a shorthand mechanism is created for subsequent invocation. This gives rise to two new definitions:

1. Labeling a statement (or defining a macro)

2. Invoking a labeled statement (running a macro)

Macro labels cannot contain punctuation: only letters, numbers, hyphens, and underscores are permitted. However, macros are identified with a prefix: it is either a pound (#) or dollar ($).


Let’s say we want to name the search example from the previous section; We’ll call it *eternal-power*. To accomplish this, we can apply a label to the statement below. This produces a full macro:

%span=7 %similarity=85 eternal power < Romans || $eternal-power-romans

It’s that simple, now instead of typing the entire statement, we can utilize the macro by referencing our previously applied label. Here is how the macro can be invoked:

$eternal-power-romans

Labeled statements also support compounding. However, statements can only ever have a single search expression. Therefore, this macro invocation is disallowed (as there would be two search expressions in the same segment):

$eternal-power godhead

However, the control variables and filters that were in the macro can still be leveraged by demoting the label to a partial macro. We can execute this to accomplish the search that was disallowed above:

\#eternal-power-romans eternal power godhead

Alternatively, the macro can be redefined (his example also release the syntax for promoting a partial macro to a full macro:

\#eternal-power-romans eternal power godhead || $godhead-romans

There are a few restrictions on macro definitions:

1. The statement cannot represent an explicit action:
   - Only implicit actions are permitted in a labeled statement.
2.  definition must represent a valid Quelle statement:
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

@review $another-macro

If the user wanted to remove this definition, the @delete action is used.  Here is an example:

@delete $another-macro

If you want the same settings to be persisted to your current session that were in place during macro definition, the @absorb command will persist all settings for the macro into your current session

@absorb $my-favorite-settings-macro 

**NOTES:**

- @absorb also works with command history.
- The two built-in specialized macro invocations ( $\* and $0 ) cannot be deleted or overwritten.

**COMMAND HISTORY** 

*@history* reveals your previous activity.  To show the last ten searches, type:

*@history*

To reveal the last three searches, type:

*@history* 3

To reveal the last twenty searches, type:

*@history* 20 

To reveal activity using date ranges, type any of:

*@history* > 2023/07/04

*@history* < 2023/07/04

*@history* > 2023/07/04 < 2024/07/04 

All ranges are <u>not</u> inclusive. Therefore, commands on July 4th would never be included in the results above. Incidentally, date ranges also work on the @review command.

**INVOKE**

The *invoke* command works for command-history works exactly the same way as it does for macros.  After issuing a *@history* command, the user might receive a response as follows.

*@history*

1>  @set %span = 7

2>  @set %similarity=85

3> eternal power

And the invoke command can re-invoke any command listed.

$3

would be shorthand to re-invoke the search specified as:

eternal power

Again, *invoking* command from your command history is *invoked* just like a macro. Moreover, as with macros, control settings are persisted within your command history to provide invocation determinism. That means that control settings that were in place during the original command are utilized for the invocation.

##### Invoking a command remembers all settings, except when multiple macros are compounded:

Command history captures all settings. We have already discussed macro-determinism. Invoking commands by their review numbers behave exactly like macros. In other words, invoking command history never persists changes into your environment, unless you explicitly request such behavior with the @absorb command.

**RESETTING COMMAND HISTORY**

The @history command can be used to delete <u>all</u> command history.

To clear all command history:

@history -reset

Date [less-than / greater-than] parameters can limit the reset command. Alternatively, Id [less-than / greater-than] parameters can limit the reset command.

