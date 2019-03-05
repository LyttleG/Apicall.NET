//using System;

//namespace ApicallNET
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            int iRet = 0;
//            const int MAX_COMPUTER_NAME = 15;
//            const int MAX_PATH = 255;

//            for (int i = 0; i < 1; i++)
//            {
//                Console.WriteLine($"-------------------------------------------------------------------------- {i}");

//                using (var p = new Apicall())
//                {
//                    iRet = p.Invoke("kernel32", "SleepEx", 2, 1);
//                    p.Trace($"Invoke::SleepEx == '{p.GetValue(0).ToString()}'");
//                    if (iRet == 0)
//                        p.Trace($"Invoke::SleepEx == '{iRet}'");
//                    p.Trace("----------------------------------------------------------------------------------");
//                }

//                using (var p = new Apicall())
//                {
//                    string buffer = new String('\0', MAX_PATH + 1);
//                    iRet = p.Invoke("kernel32", "GetTempPathA", MAX_PATH, buffer);
//                    if (iRet != 0)
//                        p.Trace($"Invoke::GetTempPathA == '{p.GetValue(1).ToString().Substring(0, iRet)}'");
//                    p.Trace("----------------------------------------------------------------------------------");
//                }

//                using (var p = new Apicall(Unicode: true))
//                {
//                    string buffer = new String('x', MAX_COMPUTER_NAME);
//                    iRet = p.Invoke("kernel32", "GetComputerNameW", buffer, p.AddressOf(MAX_COMPUTER_NAME + 1));
//                    p.Trace($"Invoke::GetComputerNameW == '{p.GetValue(0).ToString()}'");
//                }
//            }

//            Console.Write("\nType any key to continue...");
//            Console.ReadKey();
//        }
//    }
//}

