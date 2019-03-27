using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Plugins.SystemVolumePlugin
{
    public class SystemVolumePlugin : MonoBehaviour
    {
        [DllImport ("SystemVolumePlugin")]
        public static extern float GetVolume();

        [DllImport ("SystemVolumePlugin")]
        public static extern int SetVolume(float volume);

        [DllImport ("SystemVolumePlugin")]
        public static extern int InitializeVolume();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LogDelegate(string str);

        [DllImport ("SystemVolumePlugin")]
        public static extern void SetLoggingCallback(IntPtr func);
    }
}