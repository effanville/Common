using Cake.Frosting;

namespace Build
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return new CakeHost()
                .UseContext<BuildContext>()
                .InstallTool(new System.Uri("nuget:?package=NuGet.CommandLine&version=5.8.1"))
                .Run(args);
        }
    }
}