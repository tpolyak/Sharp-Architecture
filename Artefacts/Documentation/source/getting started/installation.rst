.. _installation:

Installation
============

Using NuGet
-----------

**ASP.NET MVC**

Install following packages:

* `SharpArch.Web.Mvc <https://www.nuget.org/packages/SharpArch.Web.Mvc/>`_
* `SharpArch.Web.Mvc.Castle <https://www.nuget.org/packages/SharpArch.Web.Mvc.Castle/>`_ if Castle.Windsor is your container of choice.

See TradisBank.Web.Mvc/CastleWindsor/ and TradisBank.Web.Mvc/Global.asax.cs for more details on how configure and initialize IoC and S.


**ASP.NET WebAPI**

* `SharpArch.Web.Http <https://www.nuget.org/packages/SharpArch.Web.Http/>`_
* `SharpArch.Web.Http.Castle <https://www.nuget.org/packages/SharpArch.Web.Http.Castle/>`_ if Castle.Windsor is your container of choice.


Deploy Sharp Architecture with Templify
---------------------------------------

**Templify is not supported anymore.**


	Download and install `Templify <http://opensource.endjin.com/templify/>`_. Make sure to install the
	latest Sharp Architecture package by going to the Sharp Architecture
	`Downloads <http://sharparchitecture.github.io/downloads.htm>`_ page and downloading the Templify zip required, currently there are templates for NHibernate and RavenDB.

	Extract the contents and run the \*.cmd file to install the package.

	Next, select the folder you wish to deploy Sharp Architecture. In our
	case, let's create a folder and name the project, IceCreamYouScreamCorp.
	After Templify has finished deploying the package, go ahead and launch
	the solution in Visual Studio.

