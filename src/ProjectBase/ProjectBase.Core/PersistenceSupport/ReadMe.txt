PersistenceSupport is meant to provide "just enough" persistence related support for domain objects 
to work with any persistence layer.

Arguably, one or more classes in this namespace - particularly "PersistentObject.cs" - could be 
placed into the ProjectBase.Data layer.  But doing so would require that a domain layer depend on 
that assembly, thereby opening up Pandora's box to the introduction of persistence specific code
directly in the domain layer...and we just can't have that, now can we.  Furthermore, 
PersistentObject.cs has no direct dependencies on a specific persistence mechanism and, thereby, 
may respectably reside in the core layer.