using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Witcher3_Multiplayer.ClientHost;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer
{
    internal static class Program
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _hookID = SetHook(_proc);
            Application.Run(new Main());
            UnhookWindowsHookEx(_hookID);
        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
        }
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                DataManager.KeyHandle((Keys)Marshal.ReadInt32(lParam));
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}