using System.Collections.Generic;

using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build
{
    public class BuildContext : FrostingContext
    {
        #region InputParameters
        public string BuildConfiguration
        {
            get;
        }

        public string Framework
        {
            get;
        }

        public string Runtime
        {
            get;
        }

        public string RemoteDir
        {
            get;
        }

        #endregion

        public DirectoryPath RepoDir
        {
            get;
        }

        public DirectoryPath PublishLocation
        {
            get;
            set;
        }

        public DirectoryPath NugetPublishLocation
        {
            get;
            set;
        }

        public BuildContext(ICakeContext context)
            : base(context)
        {
            BuildConfiguration = context.Arguments.GetArgument("configuration") ?? Configurations.DefaultBuildConfiguration;
            Framework = context.Arguments.GetArgument("framework") ?? Configurations.DefaultFramework;
            string publishDir = context.Arguments.GetArgument("publishDir") ?? $"{Configurations.DefaultPublishDir}/{Configurations.SolutionName}/{Framework}";
            string nugetPublishDir = context.Arguments.GetArgument("publishDir") ?? $"{Configurations.DefaultPublishDir}/{Configurations.SolutionName}";
            Runtime = context.Arguments.GetArgument("runtime") ?? Configurations.DefaultRuntime;
            RemoteDir = context.Arguments.GetArgument("remote") ?? string.Empty;

            RepoDir = context.MakeAbsolute(context.Directory("../../"));
            PublishLocation = RepoDir + context.Directory($"{publishDir}");
            NugetPublishLocation = RepoDir + context.Directory($"{nugetPublishDir}");
        }

        public FilePath SolutionFilePath()
        {
            return RepoDir.CombineWithFilePath(Configurations.SolutionFileName);
        }

        public FilePath NugetConfigFilePath()
        {
            return RepoDir.CombineWithFilePath("nuget.config");
        }

        public FilePath[] ExecutableProjectFilePaths()
        {
            var filePaths = new List<FilePath>();
            foreach (var (folderName, projectName) in Configurations.ExecutablePublishProjects)
            {
                filePaths.Add(((DirectoryPath)(RepoDir + this.Directory($"{folderName}"))).CombineWithFilePath($"{projectName}.csproj"));
            }

            return filePaths.ToArray();
        }

        public (string ProjectName, FilePath FilePath)[] NugetPackageProjectFilePaths()
        {
            var filePaths = new List<(string, FilePath)>();
            foreach (var (folderName, projectName) in Configurations.NugetPackageProjects)
            {
                filePaths.Add((projectName, ((DirectoryPath)(RepoDir + this.Directory($"{folderName}"))).CombineWithFilePath($"{projectName}.csproj")));
            }

            return filePaths.ToArray();
        }
    }
}
