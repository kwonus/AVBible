### I. Background

AV-Bible uses Quelle, IPA: [kɛl], search syntax.

The complete Quelle specification can be found here:
https://github.com/kwonus/Quelle/blob/master/Quelle.md

However, AV-Bible implements only a subset of Quelle syntax as described in the following section of this document.

### II. Quick Primer on Quelle SEARCH syntax

Here is an example search using Quelle syntax:

God created earth

Next, consider a search to find that God created heaven or earth:

God created earth|heaven

The order in which the search terms are provided is insignificant. Additionally, the type-case is insignificant.

Of course, there are times when word order is significant. Accordingly, searching for explicit strings can be accomplished using double-quotes as follows:

“God created ... Earth”

These constructs can even be combined. For example:

”God created ... Heaven|Earth”

The search criteria above is equivalent to this search:

“God created ... Heaven” + “God created ... Earth”

In all cases, “...” means “followed by”, but the ellipsis allows other words to appear between created and heaven. Likewise, it allows words to appear between created and Earth.

Quelle is designed to be intuitive. It provides the ability to invoke Boolean logic on how term matching should be performed. As we saw earlier, parenthesis can be used to invoke Boolean multiplication upon the terms that compose a search expression. For instance, there are situations where the exact word within a phrase is not precisely known. For example, when searching the KJV bible, one might not recall which form of the second person pronoun was used in an otherwise familiar passage. Attempting to locate the serpent’s words to Eve in Genesis, one might execute a search such as:

you|thou|ye shall not surely die

This statement uses Boolean multiplication and is equivalent to this lengthier statement:

you shall not surely die + thou shall not surely die + ye shall not surely die

The example above also reveals how multiple search actions can be strung together to form a compound search: logically speaking, each action is OR’ed together; this implies that any of the three matches is acceptable. Using parenthetical terms produces more concise search statements.

More SEARCH Examples:

Consider a query for all passages that contain God AND created, but NOT containing earth AND NOT containing heaven:

created GOD ; -- Heaven Earth

(this could be read as: find the words

created AND God, but NOT Heaven AND NOT Earth)

The simplest form to find ALL of three words:

in the beginning

It should be noted that such a statement would find either of these strings in the text:

in the beginning

the beginning of summer in

If a specific string should be match, this can be stated explicitly:

"in the beginning"

If you are unsure what article should match, you could issue this statement:

"in a|the|that beginning"

Boolean multiplication would match only these strings of text:

in a beginning

in the beginning

in that beginning

If you are unsure which words might separate a phrase, you could issue this statement:

"in ... beginning … heaven and earth"

With this ellipsis in the find statement, it would match this string of text:

in the beginning, God created heaven and earth

If you are unsure about word order within a phrase, square brackets can be used:

find "in the beginning … [earth heaven]"

With this ellipsis and the final two bracketed terms, it would also match this string of text:

in a beginning, God created heaven and earth