using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
namespace BHOP
{
    class Program
    {
        public static int aLocalPlayer = 0x00D27AAC;
        public static int oFlags = 0x104;
        public static int aJump = 0x051DEE88;

        public static string process = "csgo";
        public static int bClient;
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int GetAsyncKeyState(int vKey);
        static void Main(string[] args)
        {
            VAMemory vam = new VAMemory(process);

            if (GetModuleAddy())
            {
                int fjump = bClient + aJump;

                aLocalPlayer = bClient + aLocalPlayer;

                int LocalPlayer = vam.ReadInt32((IntPtr)aLocalPlayer);

                int aFlags = LocalPlayer + oFlags;

                while (true)
                {
                    while (GetAsyncKeyState(32) > 0)
                    {
                        int Flags = vam.ReadInt32((IntPtr)aFlags);
                        if(Flags == 257)
                        {
                            vam.WriteInt32((IntPtr)fjump, 5);
                            Thread.Sleep(10);
                            vam.WriteInt32((IntPtr)fjump, 4);
                        }
                    }
                }
            }

            
        }
        static bool GetModuleAddy()
        {
            try
            {
                Process[] p = Process.GetProcessesByName(process);
                if (p.Length > 0)
                {
                    foreach (ProcessModule m in p[0].Modules)
                    {
                        if (m.ModuleName == "client_panorama.dll")
                        {
                            bClient = (int)m.BaseAddress;
                            return true;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
