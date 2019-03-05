# Apicall.NET
Calling API functions using .NET

You will find 1 solution developped in Visual Studio 2017

This solution contains 2 projects :

1 - ApicallNET: this is the core proxy DLL written in C# using generics.
    This DLL has a dependency with the C Apicall.dll, see below (*)
    
2- Test: this is a test project given with some examples

Compatibility is set to .NET 4.0, but you can compile it for version 3.0 as well it should work

(*) Each projects folders contains a directory named DLL that holds a file named Apicall.dll
Apicall.dll is the low level generic interface that allow you to invoke C standard libraries, STDCALL as well in CDECL format
Apicall.dll was compiled using GCC version 5.1.0 (tdm-1)

Give a try, compile the .NET solution, and see what you can do between .NET and WIN32 DLL functions

HTH,

Gérôme GUILLEMIN
