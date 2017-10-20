using System;
using System.IO;

namespace IntegrationTests.Utilities
{
    public static class ProductionCodePath
    {
        //Only use for Tests
        internal static string GetTravelTracker()
        {
            return GetSolutionPath() + "/TravelTracker";
        }

        static string GetSolutionPath()
        {
			var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());

			while (!directoryInfo.Name.Equals("TravelTracker"))
			{
				directoryInfo = directoryInfo.Parent;
			}

			return directoryInfo.FullName;
        }
    }
}
