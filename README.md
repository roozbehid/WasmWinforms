# WasmWinforms
C# Winforms can now work in your browser!
Wasm.Winforms is a nuget package that you can install and use your codes unchanged and run it on your browser!

# Try it online
- https://webassembly.z19.web.core.windows.net/ some random controls doing nothing!
- https://webassembly.z19.web.core.windows.net/Calculator/ demo compiled unchanged from [here](https://code.msdn.microsoft.com/windowsdesktop/Simple-Calculator-54ec8e4a)

# How to use
- Create a Winforms application from Visual Studio. (Note: Winforms are full .Net applications)
- Create an empty .Net Core App from Visual Studio. Add all files from full .net application directory into it. (For example, add Form1.cs,Form1.Designer.cs, Form1.resx, Program.cs .You can add those files as links too.)
- Install latest version of Winforms.Wasm nuget from nuget.org.
- Build your project. 
- You can debug Win32 (32bit version) of your code in Visual Studio. (Make sure you have 32bit version of dotnet installed! (it should be in Program Files (x86)\dotnet\dotnet.exe)
- Looking at your OUTPUT DIRECTORY\dist\ you will find your index.html to run!

# How it works
This project uses mono to run your C# IL code. 
Also coming with this distribution are System.Drawing and System.Windows.Forms dlls.
Winforms implementation requires many C libraries that I was also ported into WebAssembly. You can find out about them looking into their submodules!

# How to compile/build
This project uses another amazing ( :D ) project of mine: [GCCBuild](https://github.com/roozbehid/dotnet-vcxproj) which enables you to use any compiler to compile Visual Studio C/C++ projects in any OS.
Meaning you can have Visual Studio C/C++ project files and use them to compile both for Windows/Linux and also WebAssembly. You need to have Emscripten installed, activated and already accessible from your path.
First from Visual Studio build switch to Release-x86 then build solution, then do the same for WasmRel-x86. 
This way you have all libraries compiled for Win32 and also WebAssembly.
Then in your `\Output\bin\x86\Release` folder you can issue a nuget pack and use that nuget.

When you build an application using this nuget, look for `dist` folder which contains your index.html.
Using Win32 compiled libraries are way easier to debug and run your application.

Please note that compiling mono-wasm project should only be done in WasmRel or WasmDbg targets and it would take a minute or two to complete!

# Why is it so slow/buggy?
You need to download good amount of files to run it. All dlls and big .wasm and .js files have to be downloaded.
File issues and submit pull request so it will be faster/less buggy!
There are many many bugs and this project/product is not at all recommended for production. It is pre-alpha phase!

# We have many better UI frameworks for browsers and web, why use Winforms?!
Well that is a really good question that you have to answer!
For me it was a fun project. But many companies are still using old programs and they would like to use them as-is in a browser, so it would open tons of new oppurtunities for those programs!

# Things to do - You can pick one and contribute!
- Update mono Winforms to latest version
- Update mono version being used. Mono is rapidly changing/improving their WebAssembly technology....
- Use AOT mono
- Find bugs in libgdiplus that causes memory leaks
- Improvements to amazing Microwindows project and make it in-sync

