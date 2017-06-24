//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// CONSTANTS
//////////////////////////////////////////////////////////////////////

// var ManagedNuGetPackageVersion = "2.1.0";
// var ManagedNuGetPackage = string.Format("https://www.nuget.org/api/v2/package/Xamarin.Controls.SignaturePad/{0}", ManagedNuGetPackageVersion);

// var EmbeddinatorMacPkgVersion = "0.2.0.79";
// var EmbeddinatorMacPkg = string.Format("https://dl.xamarin.com/embeddinator/Xamarin.Embeddinator-4000-{0}.pkg", EmbeddinatorMacPkgVersion);

// var objcgen = string.Format("externals/embeddinator-objc/Library/Frameworks/Xamarin.Embeddinator-4000.framework/Versions/{0}/bin/objcgen.exe", EmbeddinatorMacPkgVersion);

// var managediOSAssembly = "externals/managed/lib/Xamarin.iOS/SignaturePad.dll";
// var managedAndroidAssembly = "externals/managed/lib/MonoAndroid/SignaturePad.dll";

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
});

Task("Build")
    // .IsDependentOn("Externals")
    .Does(() =>
{
    // build the managed library
    MSBuild("managed/managed.sln");

    // run embeddinator
    EnsureDirectoryExists("generated");
    var libs = 
        "managed/ManagediOSLibrary/bin/Debug/ManagediOSLibrary.dll " +
        "managed/ManagedLibrary/bin/Debug/netstandard1.0/ManagedLibrary.dll ";
    StartProcess(
        MakeAbsolute((FilePath)"externals/embeddinator-objc/objcgen/bin/Debug/objcgen.exe"),
        "--compile --debug --out=generated/objc --platform=iOS --target=framework " + libs);
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
