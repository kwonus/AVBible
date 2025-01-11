
### Commands that control persistent Settings

| Action     | Parameters                   | shorthand invocation |
| ---------- | ---------------------------- | -------------------- |
| **@clear** | *setting* or ALL             | n/a                  |
| **@get**   | *setting* or ALL or revision | n/a                  |
| **@set**   | *setting* **=** *value*      | *@setting* = value   |
| **@use**   | ***tag***                    | n/a                  |

#### Examples

| command           | **explanation**                                              | shorthand equivalent |
| ----------------- | ------------------------------------------------------------ | -------------------- |
| **@set** span = 8 | Assign a control setting                                     | **@span** = 8        |
| **@get** span     | get a control setting                                        | n/a                  |
| **@clear** span   | restore setting to its default value;<br/>for span, this is *verse* | n/a                  |

**@get** format

The *@get* format command would return text.  We call this: "last assignment wins". However, there is one caveat to this precedence order: regardless of where in the statement a hashtag *utilization* is provided within a statement, it never has precedence over a setting that is actually visible within the statement.

Finally, there is a bit more to say about the similarity setting, because it actually has two components. If we issue this command, it affects similarity in two distinct ways:

**@similarity** = 85% 

That command is a concise way of setting two values. It is equivalent to these two commands

**@similarity.word** = 85%

**@similarity.lemma** = 85%

That is to say, similarity is operative for the lexical word and also the lemma of the word. These two similarities thresholds need not be identical. These commands are also valid:

**@similarity.word** = 100%

**@similarity.word** = off

**@similarity.lemma** = 100%

**@similarity.lemma** = off

**@similarity** = 100%

**@similarity** = off

In all, AVX-Quelle manifests six user-controlled settings. Each allows all three actions: ***set***, ***@clear***, and ***@get*** verbs. There are two additional settings that are effectively read-only, albeit, ALL does work with *@clear* Table 2.3.d lists all settings .

| Setting Name     | Shorthand | Meaning                                                      | Values                                        | Default Value |
| ---------------- | --------- | ------------------------------------------------------------ | --------------------------------------------- | ------------- |
| span             | -         | proximity distance limit (can be "verse" or number of words) | verse or<br/> 1 to 999                        | verse         |
| lexicon          | -         | Streamlined syntax for setting lexicon.search<br/> and lexicon.render to the same value | av or avx or dual<br/>(kjv or modern or both) | n/a           |
| lexicon.search   | search    | the lexicon to be used for searching                         | av or avx or dual<br/>(kjv or modern or both) | dual / both   |
| lexicon.render   | render    | the lexicon to be used for display/rendering                 | av/avx (kjv/modern)                           | av / kjv      |
| format           | -         | format of results on output                                  | see Table 7                                   | text / utf8   |
| similarity       | -         | Streamlined syntax for setting similarity.word<br/>and similarity.lemma to the same value<br/>Phonetics matching threshold is between 33% and 100%. 100% represents an exact sounds-alike match. Any percentage less than 100, represents a fuzzy sounds-similar match <br/>Similarity matching can be completely disabled by setting this value to off | 33% to 100% **or** *off*                      | off           |
| similarity.word  | word      | fuzzy phonetics matching as described above, but this prefix only affects similarity matching on the word. | 33% to 100% **or** *off*                      | off           |
| similarity.lemma | lemma     | fuzzy phonetics matching as described above, but this prefix only affects similarity matching on the lemma. | 33% to 100% **or** *off*                      | off           |
| revision         | -         | Not really a true setting: it works with the *@get* command to retrieve the revision number of the S4T grammar supported by AV-Engine. This value is read-only. | 4.x.yz                                        | n/a           |
| ALL              | -         | ALL is an aggregate setting: it works with the *@clear* command to reset all variables above to their default values. It is used with *@get* to fetch all settings. | n/a                                           | n/a           |

The *@get* command fetches these values. The *@get* command requires a single argument. Examples are below:

*@get* span

*@get* format

All settings can be cleared using an explicit command:

*@clear* ALL

**Persistence of Settings**

It should be noted that there is a distinction between **@set** and and ***assign*** actions. The first action is an application configuration-imperative, and it is persistent (it affects all subsequent statements). Contrariwise, the ***assign*** action affects only the single statement wherein it is executed. We refer to this distinction as *persistence* vs *assignment*.

