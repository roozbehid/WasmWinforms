# WasmWinforms
Winforms now work in your browser!
WasmWinforms is a nuget package that you can install and use your EXACT same code designed for winforms and you get can run it on your browser

# Try it online
- https://webassembly.z19.web.core.windows.net/ some random controls doing nothing!
- https://webassembly.z19.web.core.windows.net/Calculator/ demo from https://code.msdn.microsoft.com/windowsdesktop/Simple-Calculator-54ec8e4a


# How to use
- Create a Winforms application from Visual Studio. Note that winforms are full .Net applications.
- Create an empty .Net Core App and add files from full .net application into it. For example, by default those files will be Form1.cs,Form1.Designer.cs, Form1.resx, Program.cs
- Install latest version of WasmWinforms.Build.Tasks.
- Build your project. You can actually run your project and it will be using the exact same code to run it on your windows. The actual html will be generated in "dist" folder in your output!

# How it works
This project uses mono to run your C# IL code. Also coming with this distribution are System.Drawing and System.Windows.Forms. Winforms implementation required many other c libraries that I also ported all of them into WebAssembly. You can find out about them looking into their submodules!

# How to compile/build
This project uses another amazing ( :D ) project of mine: [GCCBuild](https://github.com/roozbehid/dotnet-vcxproj) which enables you to use any compiler to compile C/C++ code.
Meaning you can have Visual Studio C/C++ project files and use them to compile both for Windows/Linux and also WebAssembly. You need to have Emscripten installed, activated and already accessible from your path.
First from Visual Studio build for Release-x86 then build for WasmRel-x86. This way you have all libraries compiled for Win32 and also WebAssembly.
Using Win32 compiled libraries are way easier to debug and run your applcation.

Please note that compiling mono-wasm should only be done in WasmRel or WasmDbg targets and it would take a minute or two to complete!

# Why is it so slow/buggy?
File issues and submit pull request so it will be faster/less buggy!
There are many many bugs and this project/product is not at all recommended for production. It is pre-alpha phase!

# We have many better UI frameworks for browsers and web, why use Winforms?!
Well that is a really good question. I dont know. It was fun doing this thing. Maybe call it some useless technology...