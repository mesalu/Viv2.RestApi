# Viv2.RestApi
Subproject of viv2, this repository covers the ASP.NET based REST API that viv2-frontend-clients will use to interface with data generated by other viv2 components.


## Overall design / Architecture

I'm still learning about architectures in this domain. However I was aiming for a hexagonal implemenation. I think I've missed on a few key parts, but it should be close enough to be recognizable/navigatable. 

## Components

- AppInterface: ASP.NET scaffolding, services HTTPS requests for data.
- Core: Main buisness logic of the application. Defines the necessary abstractions to operate and is not dependent on any single implementation.
- Infrastructure: Implementation of supporting abstractions.
- Tests: (woefully behind) Unittests.
