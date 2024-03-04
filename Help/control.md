### CONTROL - AV-Bible Help

### Commands that control persistent Settings

| Verb       | Action Type | Syntax Category | Usage                  | Shorthand & alternate usages |
| ---------- | :---------: | --------------- | ---------------------- | ---------------------------- |
| **@set**   |  explicit   | CONTROL         | @set *setting* = value | @*setting* = value           |
| **@clear** |  explicit   | CONTROL         | @clear *setting*       | @clear ALL                   |
| **@get**   |  explicit   | CONTROL         | @get *setting*         | @*setting* @VERSION / @ALL   |

#### Examples

| command           | **explanation**                                              | shorthand equivalent |
| ----------------- | ------------------------------------------------------------ | -------------------- |
| **@set** span = 8 | Assign a control setting                                     | @span = 8            |
| **@get** span     | get a control setting                                        | @span                |
| **@clear** span   | Clear: restore setting to the default; for span, this is *verse* |                      |

Otherwise, when multiple clauses contain the same setting, only the last setting in the list is preserved.  Example:

%format=md %format=default %format=text

@get format

The @get format command would return text.  We call this: "last assignment wins". However, there is one caveat to this precedence order: regardless of where in the statement a macro or history invocation is provided within a statement, it never has precedence over a setting that is actually visible within the statement.

Finally, there is a bit more to say about the %similarity setting, because it actually has three components. If we issue this command, it affects similarity in two distinct ways:

@similarity=85 

That command is a concise way of setting two values. It is equivalent to this command

@similarity=word:85 lemma:85

That is to say, similarity is operative for the lexical word and also the lemma of the word. While not discussed previously, these two similarities thresholds need not be identical. These commands are also valid:

@similarity = word: 85 lemma: 95

@similarity = word : 85

@similarity = word: none lemma: exact

@similarity = lemma:none

In all, AVX-Quelle manifests five control names. Each allows all three actions: ***set***, ***clear***, and ***@get*** verbs. Table 12-4 lists all settings available in AVX-Quelle. AVX-Quelle can support two distinct orthographies [i.e. Contemporary Modern English (avx/modern), and/or Early Modern English (avx/kjv).

| Setting    | Meaning                                                      | Values                                                       | Default Value |
| ---------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------- |
| span       | proximity distance limit                                     | 0 to 999 or verse                                            | 0 [verse]     |
| lexicon    | the lexicon to be used for the searching                     | av/avx/dual (kjv/modern/both)                                | dual (both)   |
| display    | the lexicon to be used for display/rendering                 | av/avx/dual (kjv/modern/both)                                | dual (both)   |
| format     | format of results on export                                  | text, markdown, html, json, yaml                             | text          |
| similarity | fuzzy phonetics matching threshold is between 1 and 99<br/>0 or *none* means: do not match on phonetics (use text only)<br/>100 or *exact* means that an *exact* phonetics match is expected | 33 to 99 [fuzzy] **or** ...<br>0 **or** *none*<br>100 **or** *exact* | 0 (none)      |
| VERSION    | Not really a true setting: it works with the @get command to retrieve the revision number of the Quelle grammar supported by AV-Engine. This value is read-only. | 2.w.xyz                                                      | n/a           |
| ALL        | Not really a true setting: it works with the @clear command to reset all variables above to their default values. It is only a valid option for the @clear command. | n/a                                                          | n/a           |

The *@get* command fetches these values. The *@get* command requires a single argument. Examples are below:

*@get* span

@get format

All settings can be cleared using an explicit command:

@clear ALL

**Scope of Settings**

It should be noted that there is a distinction between **@set** and and implicit **assign** syntax. The first form is and explicit command and is persistent (it affects all subsequent statements). Contrariwise, an implicit **assign** affects only the single statement wherewith it is executed. We refer to this as persistence vs assignment.
