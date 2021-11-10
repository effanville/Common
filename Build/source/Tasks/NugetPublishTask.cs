using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Frosting;
using Cake.Git;
using Cake.Common.Xml;
using System;

namespace Build.Tasks
{
    [TaskName("NugetPublish")]
    [IsDependentOn(typeof(TestTask))]
    public sealed class NugetPublishTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var settings = new DotNetCorePackSettings()
            {
                OutputDirectory = context.NugetPublishLocation,
                Configuration = context.BuildConfiguration,
            };
            foreach (var project in context.NugetPackageProjectFilePaths())
            {
                context.DotNetCorePack(project.FilePath.FullPath, settings);
                var readedVersion = context.XmlPeek(project.FilePath.FullPath, "//Version");
                if (readedVersion == null)
                {
                    readedVersion = context.XmlPeek(project.FilePath.FullPath, "//VersionPrefix");
                }
                var currentVersion = new Version(readedVersion);
                context.GitTag(context.RepoDir, $"{project.ProjectName}/{currentVersion.Major}.{currentVersion.Minor}/{currentVersion.Major}.{currentVersion.Minor}.{currentVersion.Build}");
            }
        }
    }
}
