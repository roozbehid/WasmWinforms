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
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

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
                GenerateHTMLFile();
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

        void GenerateHTMLFile()
        {
            string AssName = "";
            string ClassName = "Program";
            string BaseName = "Main";

            using (Stream str = File.OpenRead(Assembly))
            using (PEReader reader = new PEReader(str))
            {
                var metadata = reader.GetMetadataReader();
                var assembly = metadata.GetAssemblyDefinition();
                var methods = metadata.MethodDefinitions;
                foreach (var method in methods)
                    if (metadata.GetString(metadata.GetMethodDefinition(method.ToDebugInformationHandle().ToDefinitionHandle()).Name) == BaseName)
                    {
                        ClassName = metadata.GetString(metadata.GetTypeDefinition(metadata.GetMethodDefinition(method.ToDebugInformationHandle().ToDefinitionHandle()).GetDeclaringType()).Name);
                        AssName = metadata.GetString(metadata.GetTypeDefinition(metadata.GetMethodDefinition(method.ToDebugInformationHandle().ToDefinitionHandle()).GetDeclaringType()).Namespace);
                    }
            }
            
                string content = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
</head>
<body>
    <div id=""ooui-body"" class=""container-fluid"">
        <p id=""loading""><i class=""fa fa-refresh fa-spin"" style=""font-size:14px;margin-right:0.5em;""></i> Loading...</p>
    </div>
     <div >
      <canvas id=""canvas""></canvas>
    </div>
    <textarea id=""output"" rows=""8""></textarea>
    <script type=""text/javascript"">
        
        var App = {{
			init: function () {{
                Module.canvas = document.getElementById('canvas');
				// initialize geolocation sample
				BINDING.call_static_method(""[{Path.GetFileNameWithoutExtension(Assembly)}] {AssName}.{ClassName}:Main"", []);
			}}
		}}; 
    </script>
        <script type=""text/javascript"" src=""runtime.js""></script>
        <script async type=""text/javascript"" src=""mono.js""></script>
        <script async type=""text/javascript"" src=""mono-config.js""></script>
</body>
</html>
";
            File.WriteAllText(Path.Combine(managedPath, "index.html"), content);
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
