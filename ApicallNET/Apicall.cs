using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ApicallNET
{
    /*
    * Title: Apicall.cs
    * Description: Call API by name implementation in mixing C and C#.
    * Requires: Apicall.dll (C DLL with sdtcall/cdecl API calling support)
    * 
    * Date:         Tuesday, February 26th 2019
    * Updated:      Tuesday, March 5th 2019   
    * Developed by: Gérôme GUILLEMIN 
    * Mailto:       gerome_71@yahoo.fr 
    * 
    * Comments: If you use this code, I require you to give me credits.
    */

    public class Apicall : IDisposable
    {
        #region DECLARATIONS
        [DllImport("Apicall.dll", SetLastError = true, /*CharSet = CharSet.Auto,*/ CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr ApiCall(string DllName, string FuncName, int nbParam, int[] ParamArray);
        private static readonly int MAX_GUID_LENGTH = 0x25;
        private static ArrayDatas ad = null;
        #endregion DECLARATIONS

        #region CONSTRUCTORS
        public Apicall() { ad = new ArrayDatas(); }
        public Apicall(bool Unicode) { ad = new ArrayDatas(Unicode); }
        #endregion CONSTRUCTORS

        #region PUBLIC METHODS
        /// <summary>
        /// Gets number of parameters
        /// </summary>
        public int ParamCount => ad.Datas.Length;
        /// <summary>
        /// Parameter by address (required for passing allocated unmanaged pointers!)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int AddressOf(int value) => PinObject(value, true);
        public int AddressOf(byte[] value) => PinObject(value, true);
        public void Trace(string strMessage) => Console.WriteLine(strMessage);

        /// <summary>
        /// Call any API C function (STDCALL and/or CDECL)
        /// </summary>
        /// <param name="DllName">Name of the DLL where the FuncName exists</param>
        /// <param name="FuncName">Name of the very function to invoke</param>
        /// <param name="FuncParameters">Parameters given to FuncName</param>
        /// <returns>True if the call succeeded</returns>
        public int Invoke(string DllName, string FuncName, params object[] FuncParameters)
        {
            try
            {
                if (ad.UnicodeFlag.HasValue)
                    ad = DoConvert(FuncParameters, ad.UnicodeFlag.Value); // Ansi(false) or Unicode(true) use of allocated strings
                else
                    ad = DoConvert(FuncParameters, FuncName.EndsWith("W") ? true : false); // Automatic pistol :)

                ad.PtrApiCall = ApiCall(DllName, FuncName, ad.Datas.Length, ad.Datas);

                return ad.PtrApiCall.ToInt32();
            }
            catch (Exception ex) { ad.Ex = ex; }

            return 0;
        }

        /// <summary>
        /// Retrieve the value of an Apicall parameter (after call only)
        /// </summary>
        /// <param name="number">Indice of the parameter to retrieve</param>
        /// <returns>Value of the parameter or null if it failed</returns>
        public object ParamValue(int number)
        {
            int found = -1;
            int value = -1;

            foreach (KeyValuePair<string, int> hash in ad.HashCodes)
            {
                if (Convert.ToInt32(hash.Key.Substring(MAX_GUID_LENGTH)) == ad.Datas[number])
                {
                    value = hash.Value;
                    break;
                }
            }

            if (value != -1 && ad.Datas.Length > number)
            {
                int hashCode = value;

                for (int i = 0; i < ad.GCHandles.Count; i++)
                {
                    var h = ad.GCHandles[i].Target == null ? -1 : ad.GCHandles[i].Target.GetHashCode();

                    if (h == hashCode)
                    {
                        found = i;
                        break;
                    }
                }

                if (found != -1)
                {
                    GCHandle ptrHandle = ad.GCHandles[found];

                    if (ptrHandle.IsAllocated)
                        return PtrToStr(ptrHandle);
                    else
                        return hashCode;
                }
            }

            return value;
        }
        #endregion PUBLIC METHODS

        #region PRIVATE METHODS
        private static int PinObject(object buffer, bool byRef)
        {
            int retValue = 0;

            try
            {
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                retValue = handle.AddrOfPinnedObject().ToInt32();

                if (byRef)
                {
                    if (handle.IsAllocated)
                        ad.GCHandles.Add(handle);

                    HashCodeSet(retValue, buffer.GetHashCode());
                }
            }
            catch (Exception ex) { ad.Ex = ex; }

            return retValue;
        }

        private static void HashCodeSet(int key, int hashCode)
        {
            var guid = Guid.NewGuid().ToString();
            ad.HashCodes.Add(string.Concat(guid, "¤", key), hashCode);
        }

        private static int StrPtr(object buffer)
        {
            int retValue = 0;

            if (buffer.GetType().Name.Equals("String"))
            {
                byte[] buf = ad.UnicodeFlag == true ? Encoding.Unicode.GetBytes(buffer.ToString()) : Encoding.Default.GetBytes(buffer.ToString());
                GCHandle handle = GCHandle.Alloc(buf, GCHandleType.Pinned);
                ad.GCHandles.Add(handle);
                retValue = handle.AddrOfPinnedObject().ToInt32();
                HashCodeSet(retValue, buf.GetHashCode());
            }
            else if (buffer.GetType().Name.Equals("Int32"))
            {
                retValue = Convert.ToInt32(buffer);
                HashCodeSet(retValue, retValue.GetHashCode());
            }
            else
            {
                retValue = 0;
                HashCodeSet(retValue, 0.GetHashCode());
            }

            return retValue;
        }

        private static string PtrToStr(object hwnd)
        {
            GCHandle handle = (((GCHandle)hwnd).IsAllocated) ? ((GCHandle)hwnd) : new GCHandle();
            string strValue = string.Empty;

            if (handle.IsAllocated)
            {
                if (handle.Target.GetType().Name.Equals("Byte[]"))
                {
                    strValue = ad.UnicodeFlag == true ? Encoding.Unicode.GetString((byte[])handle.Target) : Encoding.Default.GetString((byte[])handle.Target);
                }
                else if (handle.Target.GetType().Name.Equals("Int32"))
                    strValue = Convert.ToInt32(handle.Target).ToString();
                else
                    strValue = hwnd.ToString();
            }

            return strValue;
        }

        private static int GetGCHandleFromHashCode(int hashCode)
        {
            foreach (KeyValuePair<string, int> hash in ad.HashCodes)
            {
                if (hashCode == hash.Value)
                    return Convert.ToInt32(hash.Key.Substring(MAX_GUID_LENGTH));
            }

            return -1;
        }

        private static ArrayDatas DoConvert(IList arrList, bool Unicode)
        {
            int i = 0, ptrValue = 0;

            ad.Datas = new int[arrList.Count];
            ad.Types = new int[arrList.Count];
            ad.UnicodeFlag = Unicode;

            foreach (var el in arrList)
            {
                ptrValue = GetGCHandleFromHashCode(el.GetHashCode());
                if (ptrValue == -1)
                    ptrValue = StrPtr(el);

                ad.Datas[i] = ptrValue == -1 ? 0 : ptrValue;
                ad.Types[i] = el.GetType().Name.Equals("String") ? 1 : 0;
                i++;
            }

            return ad;
        }

        #endregion PRIVATE METHODS

        #region INTERNAL CLASS
        internal class ArrayDatas
        {
            public ArrayDatas(bool Unicode) { this.UnicodeFlag = Unicode; }
            public ArrayDatas() { }
            public int[] Types { get; set; } = new int[0];
            public int[] Datas { get; set; } = new int[0];
            public Dictionary<string, int> HashCodes { get; set; } = new Dictionary<string, int>();
            public List<GCHandle> GCHandles { get; set; } = new List<GCHandle>();
            public bool? UnicodeFlag { get; set; } = null;
            public IntPtr PtrApiCall { get; set; }
            public Exception Ex { get; set; }
        }
        #endregion INTERNAL CLASS

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                // Supprimer l'état managé (objets managés)
                if (disposing)
                {
                    if (ad.GCHandles != null)
                    {
                        foreach (var handle in ad.GCHandles)
                        {
                            if (handle.IsAllocated)
                                handle.Free();
                        }
                    }

                    ad.GCHandles.Clear();
                    ad.HashCodes.Clear();
                    disposedValue = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion IDisposable Support
    }
}
