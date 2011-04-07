Sample Plugins for Raven DB Studio

http://www.ravendb.net/
https://groups.google.com/forum/#!forum/ravendb

Rough Notes
 * all the files in SharedLibs should come from the build of Raven Studio that you are using.
 * assembly references everything in ShareLibs should have Copy Local set to False. These assemblies exist in Studio, and there is no point in uploading them with the plugin.
 * Once the plugin is built, simply drop the xap file in the plugins directory of your Raven server. The location of the directory is configurable, but by default it is \Plugins\.