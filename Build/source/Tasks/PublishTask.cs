using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Publish")]
    [IsDependentOn(typeof(TestTask))]
    public sealed class PublishTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            DirectoryPath outputDir = context.PublishLocation;

            var settings = new DotNetCorePublishSettings()
            {
                Framework = context.Framework,
                Configuration = context.BuildConfiguration,
                Runtime = context.Runtime,
                OutputDirectory = outputDir,
                SelfContained = true,
                NoBuild = true
            };

            if (context.DirectoryExists(outputDir))
            {
                context.CleanDirectory(outputDir);
            }
            foreach (FilePath file in context.ExecutableProjectFilePaths())
            {
                context.DotNetCorePublish(file.FullPath, settings);
            }
        }
    }
}
