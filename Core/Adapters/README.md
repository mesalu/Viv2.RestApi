# Adapters

This directory contains interfaces that define adapters.
These services will be critical to the operation of Core, but 
are considered implementation details and will be implemented
externally (Likely in the `Infrastructure` project).

Adapters implement the logic behind executing actions that core 'exports'.

Ports can be considered 'where to send data', whereas adapters are 'what to do with actions'.

Examples of these services include Token minting, data access, etc.
