using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace IntegrationTests.Utilities
{
    public class TestFolderCreator : IDisposable
    {
        string AbsoluteFolderName;
        public string FolderName { get; private set; }

        public void CreateFolder()
        {
            //TODO: Maybe use test name as shown in 
            //https://stackoverflow.com/questions/31212224/how-to-get-xunit-test-method-name-in-base-class-even-before-the-control-goes-to/31212346#31212346

            FolderName = "Test_" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");
            AbsoluteFolderName = Directory.GetCurrentDirectory() + "/" + FolderName;

            Directory.CreateDirectory(AbsoluteFolderName);
        }

        public void Dispose()
        {
            Directory.Delete(AbsoluteFolderName, true);
        }
    }
}
