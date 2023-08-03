using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Witcher3_Multiplayer
{
    public static class langproc
    {
        public static TextBox LOGGERB;
        public static bool debug = true;
        public static double VersionCur = 1.0;
        public static Main MForm;
        public static SimpleOverlay OverlForm;
        public static bool IsHost = false, IsConnected = false;
        public static void LOG(string s)
        {
            Action t = () => { LOGGERB.Text += "[W3|INFO] " + s + Environment.NewLine; };
            if (LOGGERB.InvokeRequired)
                LOGGERB.Invoke(t);
            else
                t();
        }
        public static void ELOG(string s)
        {
            Action t = () => { LOGGERB.Text += "[W3|ERROR] " + s + Environment.NewLine; };
            if (LOGGERB.InvokeRequired)
                LOGGERB.Invoke(t);
            else
                t();
        }
        public static T BytesToStruct<T>(this byte[] data) where T : struct
        {
            var pData = GCHandle.Alloc(data, GCHandleType.Pinned);
            var result = (T)Marshal.PtrToStructure(pData.AddrOfPinnedObject(), typeof(T));
            pData.Free();
            return result;
        }

        public static byte[] StructToBytes<T>(this T data) where T : struct
        {
            var result = new byte[Marshal.SizeOf(typeof(T))];
            var pResult = GCHandle.Alloc(result, GCHandleType.Pinned);
            Marshal.StructureToPtr(data, pResult.AddrOfPinnedObject(), true);
            pResult.Free();
            return result;
        }
    }
}
