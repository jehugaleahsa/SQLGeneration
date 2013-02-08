# SQLGeneration

Provides core classes for generating SQL at runtime.

Download using NuGet: [SQLGeneration](http://nuget.org/packages/SQLGeneration)

## Status Update
I recently released version 2.0. However, now I realize that was a bit premature. For one, I've not had time to properly unit test anything. Second, I haven't had time to update any of the documentation (what documentation?). Next, I *really* hated writing the code for generating the formatted SQL.

As it turns out, I am finding tons of bugs in this new code. It is way too easy to make simple mistakes that completely ruin the output. Not only that, but my brain has been turning to mush trying to generate output. I couldn't figure out how to pass data around between expressions in order to gain enough context to make decisions.

With the last release, I was building my SQL by navigating from the top of the parse results, checking to see if different sub-expressions were found by the parser and then spitting out whitespace where it made sense. But, this broke down really quickly. The code was very similar to how I generated text in the first version of this project.

Ideally, I should be able to do any type of formatting fairly easily. I plan on taking a little time to analyze different approaches and choose one that makes formatting easier.

So hold tight. The hope is that I will be able to find a better approach that will make it easy to spit out SQL or code or whatever in less time. I can tell there's a solution there... I just don't know what it is yet.
