using System;
using System.Runtime.InteropServices;
using UnityEditor.PackageManager;
using UnityEngine;

// Maps a volume setting via a plugin.
public class VolumeMapping : MonoBehaviour
{
    // TODO: I couldn't get this to be public new float value when inheriting
    //       LinearMapping and have its functions be called.
    public float Volume
    {
        get
        {
            // If multiple threads could run this at once, to prevent rate
            // limiting race conditions this check and setting previousGet must
            // be atomic.
            if (Time.time < (previousGet + minimumGetPeriod))
                return desiredVolume;

            previousGet = Time.time;

            //Debug.Log("hit get");
            var ret = GetVolume();
            if (ret < 0)
            {
                Debug.LogError("get failed");
                return lastKnownVolume;
            }

            lastKnownVolume = ret;
            return lastKnownVolume;
        }
        set
        {
            desiredVolume = value;

            // If multiple threads could run this at once, to prevent rate
            // limiting race conditions this check and setting previousSet must
            // be atomic.
            if (Time.time < (previousSet + minimumSetPeriod))
                return;

            previousSet = Time.time;

            if (FromScalar(lastKnownVolume) == FromScalar(desiredVolume))
                return;

            //Debug.LogFormat("hit set {0:f3}", FilterScalar(desiredVolume));
            var ret = SetVolume(FilterScalar(desiredVolume));
            if (ret != 0)
            {
                Debug.LogError("set failed");
                return;
            }

            lastKnownVolume = desiredVolume;
        }
    }

    [Tooltip("Minimum seconds between volume get calls")]
    public float minimumGetPeriod = 1;

    [Tooltip("Minimum seconds between volume set calls")]
    public float minimumSetPeriod = 1;

    private float lastKnownVolume;
    private float desiredVolume;
    private float previousGet;
    private float previousSet;

    private void Update()
    {
        // Apply the last set attempt if it was declined but it's now been long
        // enough.
        Volume = desiredVolume;
    }

    public int FromScalar()
    {
        return FromScalar(Volume);
    }

    // Map a volume from [0.0, 1.0] to [0, 100]. This is done once for
    // consistency between rendered volume and haptics.
    private static int FromScalar(float scalarVolume)
    {
        return Mathf.RoundToInt(scalarVolume * 100 - 0.5f);
    }

    // Map a volume from [0.0, 1.0] to [0.0, 1.0] through the FromScalar
    // conversion. This is done for consistency between the rendered volume and
    // the volume requested of the volume plugin. Without this, it could have
    // additional decimal components.
    private static float FilterScalar(float scalarVolume)
    {
        return FromScalar(scalarVolume) / 100f;
    }

    private void Awake()
    {
        LogDelegate logDelegate = Log;

        SetLoggingCallback(Marshal.GetFunctionPointerForDelegate(logDelegate));

        var ret = InitializeVolume();
        if (ret != 0)
        {
            Debug.LogErrorFormat("Volume initialization failed with code {0}", ret);
        }
    }

    [DllImport ("SystemVolumePlugin")]
    private static extern float GetVolume();

    [DllImport ("SystemVolumePlugin")]
    private static extern int SetVolume(float volume);

    [DllImport ("SystemVolumePlugin")]
    private static extern int InitializeVolume();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void LogDelegate(string str);

    [DllImport ("SystemVolumePlugin")]
    private static extern void SetLoggingCallback(IntPtr func);

    private static void Log(string str)
    {
        Debug.LogErrorFormat("SystemVolumePlugin: {0}", str);
    }
}
