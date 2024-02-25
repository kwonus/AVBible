# SYSTEM - AV-Bible Help

AV-Bible exposes a declarative syntax for specifying search criteria. Beyond search, additional verbs provide a straightforward means to interact the search engine, by allowing you to save applicable settings. AV-Bible utilizes a variant of the Quelle command language. The Quelle specification is maintained by the same author as AV-Bible.

There are fourteen commands. These are organized into six syntax categories:

| Syntax Category | Implicit Commands *(search compatible)* | Explicit Commands              |
| --------------- | --------------------------------------- | ------------------------------ |
| **SEARCH**      | *find*,  *filter*                       | -                              |
| **CONTROL**     | *assign*                                | @set   @clear   @get   @absorb |
| **OUTPUT**      | -                                       | @print                         |
| **SYSTEM**      | -                                       | @help   @exit                  |
| **LABELING**    | *use*, *apply*                          | @delete   @review              |
| **HISTORY**     | *use*                                   | @delete   @review   @invoke    |

All search-compatible commands are implicit. All other commands are explicit. Explicit actions begin with the @ symbol, immediately followed by the explicit verb.  Implicit actions are inferred by the syntax of the command and are directly related to searching the bible with specific search criteria.

Help can be called up in the application using the help pull-down menu. Alternatively, help on any category can be invoked by requesting a help-topic by name in the command window of AV-Bible. The topic is the verb or the category for the command. All of these commands [examples] are valid requests:

@help SEARCH

@help find

@help assign

@help set

@help @set

@help

### Quick-Start for Searching AV-Bible

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

That search would find phrases where a noun appeared within a span of six words, preceding the word "home".	

