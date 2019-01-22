# WasmWinforms
Winforms now work in your browser!
WasmWinforms is a nuget package that you can install and use your EXACT same code designed for winforms and you get can run it on your browser

# Try it online
Head over to https://webassembly.z19.web.core.windows.net/ to see a demo!

# How to use
- Create a Winforms application from Visual Studio. Note that winforms are full .Net applications.
- Create an empty .Net Core App and add files from full .net application into it. For example, by default those files will be Form1.cs,Form1.Designer.cs, Form1.resx, Program.cs
- Install latest version of WasmWinforms.Build.Tasks.
- Build your project. You can actually run your project and it will be using the exact same code to run it on your windows. The actual html will be generated in "dist" folder in your output!


# How it works
This project uses mono to run your C# IL code. Also coming with this distribution are System.Drawing and System.Windows.Forms. Winforms implementation required many other c libraries that I also ported all of them into WebAssembly. You can find out about them looking into their submodules!

# Why is it so slow/buggy?
File issues and submit pull request so it will be faster/less buggy!
There are many many bugs and this project/product is not at all recommended for production. It is pre-alpha phase!

