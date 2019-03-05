using ApicallNET;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int iRet = 0;
            const int MAX_COMPUTER_NAME = 15;
            const int MAX_PATH = 255;

            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine($"---------------------------------------------------------------------------------- {i}");

                using (var p = new Apicall(Unicode: true))
                {
                    iRet = p.Invoke("user32", "MessageBoxW", 0, "Gérôme a testé", "Titre en français", 3 | 64);
                    if (iRet != 0)
                        p.Trace($"Invoke::MessageBoxW has returned: '{iRet}'");
                    for (int j = 0; j < p.ParamCount; j++)
                    {
                        p.Trace($"Invoke::MessageBoxW Parameter[{j}] == '{p.ParamValue(j).ToString()}'");
                    }
                    p.Trace("----------------------------------------------------------------------------------");
                }

                using (var p = new Apicall())
                {
                    iRet = p.Invoke("kernel32", "SleepEx", 2000, 1);
                    p.Trace($"Invoke::SleepEx Parameter[0] == '{p.ParamValue(0).ToString()}'");
                    p.Trace("----------------------------------------------------------------------------------");
                }

                using (var p = new Apicall())
                {
                    //string buffer = new String('\0', MAX_PATH + 1);
                    iRet = p.Invoke("kernel32", "GetTempPathA", MAX_PATH, new String('\0', MAX_PATH + 1));
                    if (iRet != 0)
                        p.Trace($"Invoke::GetTempPathA == '{p.ParamValue(1).ToString().Substring(0, iRet)}'");
                    p.Trace("----------------------------------------------------------------------------------");
                }

                using (var p = new Apicall())
                {
                    //string buffer = new String('x', MAX_COMPUTER_NAME);
                    iRet = p.Invoke("kernel32", "GetComputerNameW", new String('x', MAX_COMPUTER_NAME), p.AddressOf(MAX_COMPUTER_NAME + 1));
                    p.Trace($"Invoke::GetComputerNameW == '{p.ParamValue(0).ToString()}'");
                }
            }

            Console.Write("\nType any key to continue...");
            Console.ReadKey();
        }
    }
}



