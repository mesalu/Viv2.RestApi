# Interfaces

this directory  contains interfaces necesasry for interacting with the Core.
A critical portion of the design pattern being employed by this project is that the
core remain agnostic to any and all implementation details not specific
to operational logic. To that end UseCaess are used to define what the core can do / be 
used for, by external scaffolding, and OutboundPorts are used for delivering and presenting
usecase response content.