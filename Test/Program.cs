using ApicallNET;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MAX_PATH = 255; const int MAX_COMPUTER_NAME = 15; int ret = 0;

            for (int i = 1; i <= 1; i++)
            {
                Console.WriteLine($"----------------------------------- {i:00000000} -------------------------------------");

                using (var p = new Apicall()) // => Detects if Unicode or if Default encoding will be set
                {
                    ret = p.Invoke("user32", "MessageBoxW", 0 /* replace '0' with '0xBAD' to get an error */, "Et voilà le résultat !", "Title", 3 | 64);
                    if (p.Error)
                        p.Trace($">> Invoke::MessageBoxW error message: '{p.GetErrorMessage()}' [code: {p.GetErrorCode}]  ");
                    else
                        p.Trace($">> Invoke::MessageBoxW returned: '{ret}'");

                    for (int j = 0; j < p.ParamCount; j++) p.Trace($">> Parameter[{j}] == '{p.ParamValue(j)}'");
                    p.Trace("----------------------------------------------------------------------------------");
                }

                using (var p = new Apicall()) // => Detects if Unicode or if Default encoding will be set
                {
                    ret = p.Invoke("kernel32", "GetTempPathA", MAX_PATH, new String('\0', MAX_PATH + 1));
                    p.Trace($">> Invoke::GetTempPathA == '{p.ParamValue(1, ret)}'");

                    ret = p.Invoke("kernel32", "GetComputerNameA", new String('x', MAX_COMPUTER_NAME), p.AddressOf(MAX_COMPUTER_NAME + 1));
                    p.Trace($">> Invoke::GetComputerNameA == '{p.ParamValue(0, ret == 1 ? MAX_COMPUTER_NAME : 0)}'");
                    p.Trace("----------------------------------------------------------------------------------");
                }

                using (var p = new Apicall(Unicode: true)) // => Unicode flag is set
                {
                    ret = p.Invoke("kernel32", "GetTempPathW", MAX_PATH, new String('\0', MAX_PATH + 1));
                    p.Trace($">> Invoke::GetTempPathW == '{p.ParamValue(1, ret)}'");

                    ret = p.Invoke("kernel32", "GetComputerNameW", new String('\0', MAX_COMPUTER_NAME), p.AddressOf(MAX_COMPUTER_NAME + 1));
                    p.Trace($">> Invoke::GetComputerNameW == '{p.ParamValue(0, ret == 1 ? MAX_COMPUTER_NAME : 0)}'");
                    p.Trace("----------------------------------------------------------------------------------");
                }
            }

            Console.Write("\nType any key to continue...");
            Console.ReadKey();
        }
    }
}
