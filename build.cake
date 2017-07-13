//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
});

Task("Externals")
    .Does(() =>
{
    // build embeddinator for objective-c
    NuGetRestore("externals/embeddinator-objc/generator.sln");
    MSBuild("externals/embeddinator-objc/objcgen/objcgen.csproj");

    // build embeddinator for java
    NuGetRestore("externals/embeddinator/build/MonoEmbeddinator4000.sln");
    MSBuild("externals/embeddinator/build/MonoEmbeddinator4000.sln");
});

Task("Build")
    .IsDependentOn("Externals")
    .Does(() =>
{
    EnsureDirectoryExists("generated");
    var output = MakeAbsolute((DirectoryPath)"generated");

    var embeddinator = MakeAbsolute((FilePath)"externals/embeddinator/build/lib/Debug/MonoEmbeddinator4000.exe");
    var objcgen = MakeAbsolute((FilePath)"externals/embeddinator-objc/objcgen/bin/Debug/objcgen.exe");

    var iOSLib = MakeAbsolute((FilePath)"managed/ManagediOSLibrary/bin/Debug/ManagediOSLibrary.dll");
    var androidLib = MakeAbsolute((FilePath)"managed/ManagedAndroidLibrary/bin/Debug/ManagedAndroidLibrary.dll");
    var managedLib = MakeAbsolute((FilePath)"managed/ManagedLibrary/bin/Debug/netstandard1.0/ManagedLibrary.dll");

    var objcLibs = string.Join(" ", new [] { iOSLib.FullPath, managedLib.FullPath });
    var javaLibs = string.Join(" ", new [] { androidLib.FullPath, managedLib.FullPath });

    // build the managed library
    NuGetRestore("managed/managed.sln");
    MSBuild("managed/managed.sln");

    // run embeddinator for objective-c
    StartProcess(objcgen, new ProcessSettings {
        Arguments = string.Format("--compile --debug --out={0}/objc --platform=iOS --target=framework {1}", output, objcLibs),
    });

    // run embeddinator for java
    StartProcess(embeddinator, new ProcessSettings {
        Arguments = string.Format("--debug --compile --out={0}/java --platform=Android --gen=Java {1}", output, javaLibs),
        WorkingDirectory = "externals/embeddinator"
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
