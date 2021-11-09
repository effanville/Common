# Readme

The build setup is designed to enable the building of the entire solution,
following by running all tests for the solution.

Once this has happened, projects that are desired to be build and exported
can be, as well as nuget packages for projects.

The packages that are published as nuget packages also have git tags added
with the relevant version information.

## Configuration.

Various configurations can be set in the configuration.cs file.

Here one sets the `SolutionName`, as well as the folder and project names 
for the projects to publish `(string FolderName, string ProjectName)[] ExecutablePublishProjects`,
and the corresponding data for projects to publish as nuget packages
`(string FolderName, string ProjectName)[] NugetPackageProjects`.

Default values for other parameters can be specified.

## Issues

For unknown reasons, the publish task seems to not work with projects with multiple TargetFrameworks set.