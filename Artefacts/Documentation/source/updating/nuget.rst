Updating dependencies via nuget
===============================

In Visual Studio, open the Package Manager Console, and run:

.. code:: powershell

    Update-Package -Safe

This will update all packages to their latest compatible version. You
can update a specific package by providing the package name.

If any of the assemblies that have been updated are strongly signed, you
would need to add assembly binding redirects to your applications config
files (including the test projects). Luckily this can be done easily
with nuget, in the Package Manager Console select each project you want
to add assembly binding redirects to and run:

.. code:: powershell

    foreach ($proj in get-project -all) {Add-BindingRedirect}

