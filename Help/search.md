### SEARCH - AV-Bible Help

### Quickstart

IMPLICIT SEARCH TOKENS

| Verb     | Action Type | Syntax Category | Required Parameters     |
| -------- | :---------: | :-------------- | ----------------------- |
| *find*   |  implicit   | SEARCH          | *search spec*           |
| *filter* |  implicit   | SEARCH          | **<** *domain*          |
| *assign* |  implicit   | CONTROL         | **%name** **=** *value* |

### examples

Consider this proximity search where the search using Quelle syntax:

*%span=7  Moses Aaron*

Quelle syntax can define the lexicon by also supplying an additional CONTROL action:

*%span=7 %lexicon=KJV  Moses Aaron*

The statement above has two CONTROL actions and one SEARCH action

Next, consider a search to find Moses or Aaron:

*Moses|Aaron*

The order in which the search terms are provided is insignificant. Additionally, the type-case is insignificant. 

Of course, there are times when word order is significant. Accordingly, searching for explicit strings can be accomplished using double-quotes as follows:

*"Moses said ... Aaron"*

These constructs can even be combined. For example:

*"Moses said ... Aaron|Miriam"*

The search criteria above is equivalent to this search:

*"Moses said ... Aaron" + "Moses said ... Miriam"*

In all cases, “...” means “followed by”, but the ellipsis allows other words to appear between "said" and "Aaron". Likewise, it allows words to appear between "said" and "Miriam". 

Quelle is designed to be intuitive. It provides the ability to invoke Boolean logic for term-matching and/or linguistic feature-matching. As we saw above, the pipe symbol ( | ) can be used to invoke an *OR* condition In effect, this invokes Boolean multiplication on the terms and features that compose the expression.

The ampersand symbol can similarly be used to represent *AND* conditions upon terms. As an example. the English language contains words that can sometimes as a noun , and other times as some other part-of-speech. To determine if the bible text contains the word "part" where it is used as a verb, we can issue this command:

"part&/verb/"

The SDK, provided by Digital-AV, has marked each word of the bible text for part-of-speech. With Quelle's rich syntax, this type of search is easy and intuitive.

Of course, part-of-speech expressions can also be used independently of an AND condition, as follows:

%span = 6  "/noun/ ... home"

That search would find phrases where a noun appeared within a span of six words, preceding the word "home"

**Valid statement syntax, but no results:**

this&that

/noun/ & /verb/

Both of the statements above are valid, but will not match any results. Search statements attempt to match actual words in  the actual bible text. A word cannot be "this" **and** "that". Likewise, an individual word in a sentence does not operate as a /noun/ **and** a /verb/.

**Negating search-terms Example:**

Consider a query for all passages that contain a word beginning with "Lord", followed by any word that is neither a verb nor an adverb:

%span = 15 "Lord\* -/v/ & -/adv/"

this|that

/noun/ | /verb/

### FILTERING SEARCH RESULTS

Sometimes we want to constrain the domain of where we are searching. Say that I want to find mentions of the serpent in Genesis. I can search only Genesis by executing this search:

serpent < Genesis

If I also want to search in Genesis and Revelation, this works:

serpent < Genesis < Revelation

Filters do not allow spaces, but they do allow Chapter and Verse specifications. To search for the serpent in Genesis Chapter 3, we can do this:

serpent < Genesis:3

And books that contain spaces are supported by eliminating the spaces. For example, this is a valid command:

vanity < SongOfSolomon < 1Corinthians

Abbreviations are also supported:

vanity < sos < 1co

### Search-Related Control Settings

| Verb       | Action Type | Syntax Category | Usage                   | Shorthand & alternate usages   |
| ---------- | :---------: | --------------- | ----------------------- | ------------------------------ |
| **@set**   |  explicit   | CONTROL         | @set %*setting* = value | @*setting* = value             |
| **@clear** |  explicit   | CONTROL         | @clear %*setting*       | @clear ALL                     |
| **@get**   |  explicit   | CONTROL         | @get %*setting*         | @get VERSION / @get ALL / @get |

#### Examples:

| command            | **explanation**                                              | shorthand equivalent |
| ------------------ | ------------------------------------------------------------ | -------------------- |
| **@set** %span = 8 | Assign a control setting                                     | @span = 8            |
| **@get** %span     | get a control setting                                        |                      |
| **@clear** %span   | Clear: restore setting to the default; for span, this is *verse* |                      |

The %similarity setting enables sounds-alike matching on search words. It has three components. If we issue this command, it affects similarity in two distinct ways:

@similarity=85 

That command is a concise way of setting two values. It is equivalent to this command

@similarity=word:85 lemma:85

That is to say, similarity is operative for the lexical word and also the lemma of the word. While not discussed previously, these two similarities thresholds need not be identical. These commands are also valid:

@similarity = word: 85 lemma: 95

@similarity = word : 85

@similarity = word: none lemma: exact

@similarity = lemma:none

In all, AVX-Quelle manifests five control names. Each allows all three actions: ***set***, ***clear***, and ***@get*** verbs. Table 12-4 lists all settings available in AVX-Quelle. AVX-Quelle can support two distinct orthographies [i.e. Contemporary Modern English (avx/modern), and/or Early Modern English (avx/kjv).

| Setting    | Meaning                                                      | Values                                                       | Default Value | Applicable to Search |
| ---------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------- | -------------------- |
| span       | proximity distance limit                                     | 0 to 999 or verse                                            | 0 [verse]     | yes                  |
| lexicon    | the lexicon to be used for the searching                     | av/avx/dual (kjv/modern/both)                                | dual (both)   | yes                  |
| display    | the lexicon to be used for display/rendering                 | av/avx/dual (kjv/modern/both)                                | dual (both)   | yes                  |
| format     | format of results on export                                  | text, markdown, html, json, yaml                             | text          | no                   |
| similarity | fuzzy phonetics matching threshold is between 1 and 99<br/>0 or *none* means: do not match on phonetics (use text only)<br/>100 or *exact* means that an *exact* phonetics match is expected | 33 to 99 [fuzzy] **or** ...<br>0 **or** *none*<br>100 **or** *exact* | 0 (none)      | yes                  |

**Scope of Settings**

It should be noted that there is a distinction between **@set** and and implicit **assign** syntax. The first form is and explicit command and is persistent (it affects all subsequent statements). Contrariwise, an implicit **assign** affects only the single statement wherewith it is executed. We refer to this as persistence vs assignment.

### Terminology

**Syntax Categories:** Each syntax category defines rules by which verbs can be expressed in the statement. 

**Actions:** Actions are complete verb-clauses issued in the imperative [you-understood].  Many actions have one or more parameters.  But just like English, a verb phrase can be a single word with no explicit subject and no explicit object.  Consider this English sentence:

Go!

The subject of this sentence is "you understood".  Similarly, all Quelle verbs are issued without an explicit subject. The object of the verb in the one word sentence above is also unstated.  Quelle operates in an analogous manner.  Consider this English sentence:

Go Home!

Like the earlier example, the subject is "you understood".  The object this time is defined, and insists that "you" should go home.  Some verbs always have objects, others sometimes do, and still others never do. Quelle follows this same pattern and some Quelle verbs require direct-objects; and some do not.  In the various tables throughout this document, required and optional parameters are identified, These parameters represent the object of the verb within each respective table.

**Statement**: A statement is composed of one or more segments. When there is more than one segment, each segment contains a search expression. All search results are logically OR’ed together. It is recommended that statements with multiple segments be used sparingly as they represent a complex search pattern. In most situations, a single segment is sufficient to perform very powerful searches. This is because Quelle search expressions already offer powerfull searches with Boolean search logic on all search fragments.

**Expression**: Each segment has zero or one search expressions. A segment without a search expression is typically used to define a partial macro. If you are searching, your segment will contain a search expression. Expressions have fragments, and fragments have features. For an expression to match, all fragments must match (Logical AND). For a fragment to match, any feature must match (Logical OR). AND is represented by &. OR is represented by |.

**Unquoted SEARCH clauses:** an unquoted search clause contains one or more search fragments. If there is more than one fragment in the clause, then each fragment is logically AND’ed together.

**Quoted SEARCH clauses:** a quoted clause contains a single string of terms to search. An explicit match on the string is required. However, an ellipsis ( … ) can be used to indicate that other terms may silently intervene within the quoted string.

- It is called *quoted,* as the entire clause is sandwiched on both sides by double-quotes ( " )
- The absence of double-quotes means that the statement is unquoted

**Booleans and Negations:**

**and:** In Boolean logic, **and** means that all terms must be found. With Quelle, *and* is represented by terms that appear within an unquoted clause. 

**or:** In Boolean logic, **or** means that any term constitutes a match. With Quelle, *or* is represented by the plus (+) between SEARCH expressions. All search results are collated together as a union. 

**not:** In Boolean logic, means that the feature must not be found. With Quelle, *not* is represented by the hyphen ( **-** ) and applies to individual features within a fragment of a search expression. It is best used in conjunction with other features, because any non-match will be included in results. 

hyphen ( **-** ) means that any non-match satisfies the search condition. Used by itself, it would likely return every verse. Therefore, it should be used judiciously.

### Specialized Search tokens

The lexical search domain of AV-Bible includes all words in the original KJV text. It can also optionally search using a modernized lexicon of the KJV (e.g. hast and has; this is controllable with the %lexicon setting).

| Search Term        | Operator Type                           | Meaning                                                      | Maps To                                                      | Mask   |
| ------------------ | --------------------------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------ |
| un\*               | wildcard (example)                      | starts with: un                                              | all lexicon entries that start with "un"                     | 0x3FFF |
| \*ness             | wildcard (example)                      | ends with: ness                                              | all lexicon entries that end with "ness"                     | 0x3FFF |
| un\*ness           | wildcard (example)                      | starts with: un<br/>ends with: ness                          | all lexicon entries that start with "un", and end with "ness" | 0x3FFF |
| \*profit\*         | wildcard (example)                      | contains: profit                                             | all lexicon entries that contain both "profit"               | 0x3FFF |
| \*pro\*fit\*       | wildcard (example)                      | contains: pro and fit                                        | all lexicon entries that contain both "pro" and "fit" (in any order) | 0x3FFF |
| un\*profit*ness    | wildcard (example)                      | starts with: un<br/>contains: profit<br/>ends with: ness     | all lexicon entries that start with "un", contain "profit", and end with "ness" | 0x3FFF |
| un\*pro\*fit\*ness | wildcard (example)                      | starts with: un<br/>contains: pro and fit<br/>ends with: ness | all lexicon entries that start with "un", contain "pro" and "fit", and end with "ness" | 0x3FFF |
| ~ʃɛpɝd*            | phonetic wildcard (example)             | Tilde marks the wildcard as phonetic (wildcards never perform sounds-alike searching) | All lexical entries that start with the sound ʃɛpɝd (this would include shepherd, shepherds, shepherding...) |        |
| ~ʃɛpɝdz            | sounds-alike search using IPA (example) | Tilde marks the search term as phonetic (and if similarity is set between 33 and 99, search handles approximate matching) | This would match the lexical entry "shepherds" (and possibly similar terms, depending on similarity threshold) |        |
| \\is\\             | lemma                                   | search on all words that share the same lemma as is: be, is, are, art, ... | be is are art ...                                            | 0x3FFF |
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
| [type]             | named entity                            | Entities are recognized by MorphAdorner. They are also matched against Hitchcock's database. This functionality is experimental and considered BETA. | type=person man<br/>woman tribe city<br/>river mountain<br/>animal gemstone<br/>measurement any<br/>any_Hitchcock |        |
| \#FFFF             | PN+POS(12)                              | hexadecimal representation of bits for a PN+POS(12) value.   | See Digital-AV SDK                                           | uint16 |
| \#FFFFFFFF         | POS(32)                                 | hexadecimal representation of bits for a POS(32) value.      | See Digital-AV SDK                                           | uint32 |
| #string            | nupos-string                            | NUPOS string representing part-of-speech. This is the preferred syntax over POS(32), even though they are equivalent. NUPOS part-of-speech values have higher fidelity than the 16-bit PN+POS(12) representations. | See Part-of-Speech-for-Digital-AV.docx                       | uint32 |
| 99999:H            | Strongs Number                          | decimal Strongs number for the Hebrew word in the Old Testament | One of Strongs\[4\]                                          | 0x7FFF |
| 99999:G            | Strongs Number                          | decimal Strongs number for the Greek word in the New Testament | One of Strongs\[4\]                                          | 0x7FFF |

​	

