using System.IO;

namespace ML.Infrastructure
{
    public static class FileHelper
    {
        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo dataRoot = new FileInfo(typeof(FileHelper).Assembly.Location);
            string assemblyFolderPath = dataRoot.Directory.FullName;
            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
