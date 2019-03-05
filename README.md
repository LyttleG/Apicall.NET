# Apicall.NET
Calling API functions using .NET

You will find 1 solution developped in Visual Studio 2017

This solution contains 2 projects :

1 - ApicallNET: this is the core proxy DLL written in C# using generics.
    This DLL has a dependency with the C Apicall.dll, see below (*)
    
2- Test: this is a test project given with some examples

Compatibility is set to .NET 4.0, but you can compile it for version 3.0 as well it should work

(*) Each projects folders contains a directory named DLL that holds a file named Apicall.dll
Apicall.dll is the low level generic interface that allow you to invoke C standard libraries, STDCALL as well as in CDECL format
Apicall.dll was compiled using GCC version 5.1.0 (tdm-1)

Give a try, compile the .NET solution, and see what you can do between .NET and WIN32 DLL functions

HTH,

Gérôme GUILLEMIN

## How to use Apicall class
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
