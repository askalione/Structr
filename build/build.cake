using System.Text.RegularExpressions;

var target = Argument("target", "Default");

var configuration = 
    HasArgument("Configuration") 
        ? Argument<string>("Configuration") 
        : EnvironmentVariable("Configuration") ?? "Debug";

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

Task("Clean-Artifacts")
    .Does(() =>
    {
        if (DirectoryExists(artifactsDirectory))
	    {
		    DeleteDirectory(artifactsDirectory, new DeleteDirectorySettings { Recursive = true });
	    }
        
    });

Task("Clean")
	.IsDependentOn("Clean-Artifacts")
	.Does(() => 
	{
		foreach (var project in srcProjects)
	    {
		    DotNetClean(project.FullPath);
	    }
	});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        var settings = new DotNetRestoreSettings
        {
            Force = true,
            Interactive = true
        };
        DotNetRestore(solutionFile.FullPath);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var settings = new DotNetBuildSettings
        {
            Configuration = configuration,
            VersionSuffix = versionSuffix
        };

        var projects = srcProjects
            .Concat(sampleProjects)
            .Concat(testProjects);

        foreach(var project in projects) 
        {
            DotNetBuild(project.FullPath, settings);
        }
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetTestSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true,
        };

        foreach(var project in testProjects) 
        {
            DotNetTest(project.FullPath, settings);
        }
    });

Task("Pack")
    .IsDependentOn("Test")
	.Does(() =>
    {
	    var settings = new DotNetPackSettings
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
		    DotNetPack(project.FullPath, settings);
	    }
    });

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);