# Apicall.NET
Calling Win32 API functions using .NET 4.x

The Apicall.NET solution was developped using Visual Studio 2017.
The purpose of this library is to allow any .NET developper to call any Win32 API without declaring any prototype function.

## Content of the solution
1 - ApicallNET.dll: this is the core proxy library written in C# using generics.
    This DLL has a dependency with Apicall.dll, see below (*)
    
2- Test: this is a test project given with some examples

Compatibility is set to .NET 4.0, but you can compile it for version 3.0 as well it should work

```
(*) Each projects folders contains a directory named DLL that holds a file named Apicall.dll
Apicall.dll is a low level generic interface that allows you to invoke C standard libraries (STDCALL as well as in CDECL)
Apicall.dll was developped in pure C with a pinch os ASM code, then compiled using GCC version 5.1.0 (tdm-1)
```
Give a try, compile the .NET solution, and see what you can do with Win32 API from within .NET!

## How to use Apicall class
```
The following C# code will invoqke 'MessageBoxW' unicode function from 'User32.dll' (W == Unicode)
```
![alt text](/Test/Example.jpg?raw=true "Invoke MessageBoxW")

```
Here's the result:
```
![alt text](/Test/Output.png?raw=true "Invoke MessageBoxW")

```
See the C# source code located in Test/Program.cs for details 
```

## Built With
* [VS2017](https://visualstudio.microsoft.com/vs/) - Build smarter apps, fast using Visual Studio 2017
* [GCC51](https://gcc.gnu.org/) - GCC, the GNU Compiler Collection version 5.1.0 (tdm-1)

## Authors
* **Gérôme GUILLEMIN**

## License
This project is licensed under the LGPL v3 License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments
* Hat tip to anyone whose code was used
* Inspiration
* etc

HTH,

Gérôme GUILLEMIN
