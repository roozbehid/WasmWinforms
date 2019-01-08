using System;

namespace System.Windows.Forms
{
    public static class Environment
    {
        public static string StackTrace { get { return System.Environment.StackTrace; } }

        public static OSVersion OSVersion { get { return new OSVersion(); } }

        public static string NewLine { get { return System.Environment.NewLine; } }

        public static int TickCount { get { return System.Environment.TickCount; } }
        public static bool UserInteractive { get { return true; } }
        public static string UserDomainName { get { return ""; } }
        public static string UserName { get { return ""; } }

        public static string CurrentDirectory { get; set; }
        public static string MachineName { get; set; }

        public static string GetFolderPath(SpecialFolder f)
        {
            return "";
        }

        public enum SpecialFolder
        {
            Personal,
            ApplicationData,
            Desktop,
            CommonApplicationData,
            Recent,
            MyComputer,
            LocalApplicationData,
            DesktopDirectory
        }

        public static string GetEnvironmentVariable(string variable)
        {
            return System.Environment.GetEnvironmentVariable(variable);
        }

        public static string[] GetCommandLineArgs()
        {
            throw new NotImplementedException();
        }

        public static void Exit(int exitCode)
        {
            System.Environment.FailFast("exitCode:" + exitCode);
        }
    }

    public class OSVersion
    {
        public PlatformID Platform { get { return PlatformID.Win32NT; } }

        public Version Version { get { return new Version(10, 0); } }
    }
}