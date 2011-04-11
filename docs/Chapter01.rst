Extending RavenDB Management Studio
*********************************************

.. contents:: In this chapter...
  :depth: 3

RavenDB Management Studio (or more simply, Studio) has a number of extension points where developers can augment its functionailty.

* knowledge requirements
* types of plugins
* installing a plugin
* building a task
* building an explorer item
* best practices
* * common patterns
* * debugging a plugin
* reference
* * bits that you can make use of in Raven.Studio.dll
* quick start

What do you need to know?
============================

Developing plugins for Studio is easy to do. However, there are a set of technologies that you need to be familiar with.

* Silverlight
* Managed Extensibility Framework (or MEF)
* `Caliburn Micro<http://caliburnmicro.codeplex.com>`_
* RavenDB's Silverlight API
* Task Parallel Library for Silverlight

Don't let this list discourage you. As long as you know the basics of C# and Silverlight, 
we'll cover everything else you'll need to know to build plugins.

Silverlight
^^^^^^^^^^^^

Studio is developed using Silverlight 4. Silverlight is Microsoft's platform for building _Rich Internet Applications_. 
Resources for learning Silverlight are plentiful. If you are completely new to Silverlight, then we recommend:

* Getting Started with Silverlight 
* Tekpub's Mastering Silverlight 4

This guide assumes basic Silverlight and C# skills.

Managed Extensibility Framework
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

MEF is a library for composing applications at runtime. It was developed by Microsoft primarily supporting
extensions for Visual Studio 2010. In addition, MEF is built into Silverlight. These characteristics make it an ideal 
choice for developing plugins for Studio.

We'll cover the basics of MEF that are required to build plugins for Studio.

Caliburn Micro
^^^^^^^^^^^^^^

Caliburn Micro is an open source framework that facilitates building applications in Silverlight. It is particularly
useful when using architectural patterns such as Model-View-ViewModel (of MVVM). Studio itself is built using Caliburn Micro.

There is very little about Caliburn Micro that you _need_ to know in order to develop plugins. We'll cover those basics for you.

If you are interested in learning more about Caliburn Micro and how it can reduce your development time, vist the 
`project site<http://caliburnmicro.codeplex.com>`_.

RavenDB's Silverlight API
^^^^^^^^^^^^^^^^^^^^^^^^^
RavenDB ships with an API Silverlight. This API is found in the Silverlight assembly *Raven.Client.Silverlight.dll* and it is independent
from Studio. The API itself is documented elsewhere and is designed to be easily discoverable. We'll refer to this as the _client_ API.

In addition, Studio has an API for plugin development built on top of the client API. The _plugin_ API is found in the Silverlight
assembly *Raven.Studio.dll* and most of the classes we'll use will be in the *Raven.Studio.Plugins* namespace.

Task Parallel Library
^^^^^^^^^^^^^^^^^^^^^
Both the client API and Studio make heavy use of Microsoft's Task Parallel Library. This library is built into .NET 4, but it is not
a native part of Silverlight 4. However, Microsoft has provided this library for Silverlight developers as part of the Async CTP.
//TODO: what is the official name of the CTP?
The library is designed to simplify asynchronous programming. It is found in the Silverlight assembly *AsyncCtpLibrary_Silverlight.dll*.

Its usage with respect to RavendB can be identified by the class *System.Threading.Tasks.Task* and by the extension method *ContinueWith*.

Types of Plugins
============================

At the time of this writing, there are two places to extend Studio. More extension points are planned for the future.

* Explorer Items
* Tasks

Explorer Items
^^^^^^^^^^^^^^

When a database is selected in Studio, you are presented with the Database Explorer screen as shown in figure 1.1.

.. figure::  _static/DatabaseExplorer.png

  Figure 1.1 - The Database Explorer

The Database Explorer is a collection of high level features. The features are exposed as a menu of options on the left-hand side.
The menu options are referred to as _Explorer Items_. When you click on an Explorer Item in the menu, the corresponding screen is
opened in the right-hand pane. New Explorer Items can be added to the menu using plugins.

There is no limitation on the sort of functionality that can be implemented inside an Explorer Item.

The intention of the Database Explorer is to allow a user to quickly access frequently used functionality. Extending the Database 
Explorer should be done sparingly.

Tasks
^^^^^^^^^^^^^^

One of the default Explorer Items is *Tasks*. As its name implies, the Tasks item is intended to be a collection of utilities for 
managing a database. For example, Studio ships with an Export Task and an Import Task.

There is no limitation on the sort of functionality that can be implemented inside a Task.

For most functionality, a Task will be the desired location to extend Studio.

Building a Plugin
============================

The process of building Tasks and Explorer Items is nearly identical. We'll walk through the steps for constructing a new task,
but we'll also point out the differences that you would want to address when developing an Explorer Item.

Creating a Plugin Project
^^^^^^^^^^^^^^^^^^^^^^^^^

Open Visual Studio 2010, a create a new Silverlight Application.

* File | New | Project
* In the New Project dialog, select Silverlight Application and click Ok. You can locate the project template using the search bar in the 
upper right corner of the dialog.
* In the New Silverlight Application Dialog, uncheck _Host the Silverlight application in a Web site_ and make sure that
Silverlight 4 is selected under Options then click Ok.

Be sure to select a Silverlight Application and not a Silverlight Class Library. The Silverlight Appliccation will produce a _xap_ file 
when compiled. Xap files are the unit of deployment for Silverlight application and for Studio plugins as well.

Referencing Assemblies
^^^^^^^^^^^^^^^^^^^^^^

There is a minimum set of assemblies that you will need to reference in order to build a plugin for Studio. It is important that these
assemblies match the versions that are included with Studio. The assemblies are:

* AsyncCtpLibrary_Silverlight.dll
* Caliburn.Micro.dll
* Newtonsoft.Json.Silverlight.dll
* Raven.Client.Silverlight.dll
* Raven.Studio.dll
* System.Windows.Controls.Toolkit.dll