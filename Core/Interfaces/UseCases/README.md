# Use Case Interfaces

This folder contains abstraction interfaces for Core related actions.


## ToDo:

I'd like to get enough C#-foo under my belt to condense a lot of
these generic-data-access interfaces into a simpler one that uses
LINQ-like property-expressions for specifying the data the invoker
wants, with the ability to specify exact-implementations for special cases.
(E.g. Core may not care about most user-property accesses, but being able
to provide a specific implementation to ensure that a property is loaded 
from infrastructure first, or the like, would be beneficial.)

For now though, I'll have to settle with a lot of code bloat.
Kind of a "get it working first, make it pretty later" scenario.
