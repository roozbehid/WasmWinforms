using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace WasmWinforms.Build.Tasks
{
    public class BuildWasmTask : Task
    {
        const string AssemblyExtension = ".bin";

        [Required]
        public string Assembly { get; set; }
        [Required]
        public string OutDir { get; set; }
        public string ReferencePath { get; set; }
        [Required]
        public string NugetContentPath { get; set; }

        bool ok = false;

        public override bool Execute()
        {
            try
            {
                ok = true;
                Log.LogMessage("-----BuildWasm Started------");
                System.Diagnostics.Debugger.Launch();
                InstallSdk();
                GetBcl();
                CreateDist();
                DeleteOldAssemblies();
                RunPackager();
                return ok;
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex);
                Log.LogErrorFromException(ex);
                return false;
            }
        }

        string sdkPath;

        void InstallSdk()
        {
            sdkPath = NugetContentPath;
        }

        string bclPath;
        Dictionary<string, string> bclAssemblies;

        void GetBcl()
        {
            bclPath = Path.Combine(sdkPath, "wasm-bcl", "wasm");
            var reals = Directory.GetFiles(bclPath, "*.dll");
            var facades = Directory.GetFiles(Path.Combine(bclPath, "Facades"), "*.dll");
            var allFiles = reals.Concat(facades);
            bclAssemblies = allFiles.ToDictionary(x => Path.GetFileName(x));
        }

        string distPath;
        string managedPath;

        void CreateDist()
        {
            var outputPath = Path.GetFullPath(OutDir);
            distPath = Path.Combine(outputPath, "dist");
            managedPath = Path.Combine(distPath, "managed");
            Directory.CreateDirectory(managedPath);
        }

        void DeleteOldAssemblies()
        {
            foreach (var dll in Directory.GetFiles(managedPath, "*.dll"))
            {
                File.Delete(dll);
            }
            foreach (var dll in Directory.GetFiles(managedPath, "*" + AssemblyExtension))
            {
                File.Delete(dll);
            }
        }

        void RunPackager()
        {
            string options = $"--copy=always --out=\"{managedPath.TrimEnd('\\')}\" --search-path=\"{OutDir.TrimEnd('\\')}\" {Assembly}";
            ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(sdkPath, "packager.exe"), options);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
            startInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;


            var process = new Process { StartInfo = startInfo };
            process.Start();

            string cv_error = null;
            Thread et = new Thread(() => { cv_error = process.StandardError.ReadToEnd(); });
            et.Start();

            string cv_out = null;
            Thread ot = new Thread(() => { cv_out = process.StandardOutput.ReadToEnd(); });
            ot.Start();

            process.WaitForExit();
            et.Join();
            string output = cv_error;// process.StandardError.ReadToEnd();
            ok = (process.ExitCode == 0);
            if (!ok)
                Log.LogError(cv_error);
            else
                Log.LogMessage(cv_out);

            process.Close();
        }

    }
}
