Architecture
============

The project is divided into the following layers:

-  Tasks
-  Domain
-  Infrastructure
-  Presentation
-  Specs/Tests

Despite their divergent names, the layers and their functions should be
familiar to anyone with experience with the MVC pattern.

Tasks
-----

Previous known as "Application Services," the *Tasks Layer* serves to
tie together any non-business logic from a variety of third-party
services or persistence technologies. While setup and defining a service
such as Twitter would occur in the *Infrastructure Layer*, executing and
combining the results with, say, a local NHibernate database, would
occur in the *Tasks Layer*. The resulting viewModel or DTO would be sent
down to the *Presentation Layer*

Additional occupants
^^^^^^^^^^^^^^^^^^^^

-  CommandHandlers
-  Commands

Presentation
------------

The *Presentation Layer* contains the familiar MVC project with views,
viewmodels and controllers, along with application setup tasks. Previous
iterations of Sharp Architecture spun out controllers to a separate
application, but as of 2.0 this is no longer the case.

Additional occupants
^^^^^^^^^^^^^^^^^^^^

-  ViewModels
-  Query Objects
-  Controllers
-  Views

Infrastructure
--------------

The *Infrastructure Layer* setups up third party data sources, along
with items such as NHibernate maps. Previously, methods on a repository
would exist here, but are superseded by IQuery.

Domain
------

The *Domain Layer* is where business entities and other business logic
resides. The domain layer should be persistence ignorant, with any
persistence existing in the *Tasks Layer* or in the *Presentation Layer*
(for populating viewModels).

Additional occupants
^^^^^^^^^^^^^^^^^^^^

-  Contracts for Tasks
-  Contracts for IQuery

