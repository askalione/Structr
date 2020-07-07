using System.Text.RegularExpressions;

var target = Argument("target", "Default");

var configuration = 
    HasArgument("Configuration") 
        ? Argument<string>("Configuration") 
        : EnvironmentVariable("Configuration") ?? "Release";

var buildNumber =
    HasArgument("BuildNumber") ? Argument<int>("BuildNumber") :
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number :
    EnvironmentVariable("BuildNumber") != null ? int.Parse(EnvironmentVariable("BuildNumber")) : 0;

var branch = 
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Repository.Branch : (string)null;

string versionSuffix = null;
if(string.IsNullOrWhiteSpace(branch) == false && branch != "master")
{
    versionSuffix = $"-dev.{buildNumber}";

    var match = Regex.Match(branch, "release\\/\\d+\\.\\d+\\.\\d+\\-?(.*)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    if(match.Success)
        versionSuffix = string.IsNullOrWhiteSpace(match.Groups[1].Value) == false
            ? $"-{match.Groups[1].Value}.{buildNumber}"
            : $".{buildNumber}";
}

var solutionFile = GetFiles("../*.sln").First();
var srcProjects = GetFiles("../src/**/*.csproj");
var sampleProjects = GetFiles("../samples/**/*.csproj");
var testProjects = GetFiles("../tests/**/*.csproj");

var artifactsDirectory = Directory("./artifacts");

Task("Clean-Outputs")
    .Does(() =>
    {
        if (DirectoryExists(artifactsDirectory))
	    {
		    DeleteDirectory(artifactsDirectory, true);
	    }
        
    });

Task("Clean")
	.IsDependentOn("Clean-Outputs")
	.Does(() => 
	{
		DotNetBuild(solutionFile, settings => settings
			.SetConfiguration(configuration)
			.WithTarget("Clean")
			.SetVerbosity(Verbosity.Minimal));
	});


Task("NuGet-Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore(solutionFile);
    });

 Task("Build")
    .IsDependentOn("NuGet-Restore")
    .Does(() =>
    {
        var settings = new DotNetCoreBuildSettings
            {
                Configuration = configuration,
                VersionSuffix = versionSuffix
            };

        var projects = srcProjects
            .Concat(sampleProjects)
            .Concat(testProjects);

        foreach(var project in projects)
            DotNetCoreBuild(project.FullPath, settings);
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true,
        };

        foreach(var project in testProjects)
            DotNetCoreTest(project.FullPath, settings);
    });

Task("Pack")
    .IsDependentOn("Test")
	.Does(() =>
    {
	    var settings = new DotNetCorePackSettings
	    {
            ArgumentCustomization = args => args.Append("/p:SymbolPackageFormat=snupkg"),
		    Configuration = configuration,
		    VersionSuffix = versionSuffix,
		    IncludeSymbols = true,
            NoRestore = true,
            NoBuild = true,
		    OutputDirectory = artifactsDirectory
	    };
	    foreach (var project in srcProjects)
	    {
		    DotNetCorePack(project.FullPath, settings);
	    }
    });

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);
