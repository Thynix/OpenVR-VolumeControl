using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Plugins.SystemVolumePlugin;

// Links the system volume to a LinearMapping.
public class SystemVolumeLink : MonoBehaviour
{
    private float Volume
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
            var ret = SystemVolumePlugin.GetVolume();
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

            var ret = SystemVolumePlugin.SetVolume(FilterScalar(desiredVolume));
            if (ret != 0)
            {
                Debug.LogError("set failed");
                return;
            }

            lastKnownVolume = desiredVolume;
        }
    }

    public LinearMapping linearMapping;

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
        Volume = linearMapping.value;
    }

    public int FromScalar()
    {
        return FromScalar(Volume);
    }

    // Map a volume from [0.0, 1.0] to [0, 100]. This allows consistency between
    // changes in the rendered volume and haptics.
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
        SystemVolumePlugin.LogDelegate logDelegate = Log;

        SystemVolumePlugin.SetLoggingCallback(Marshal.GetFunctionPointerForDelegate(logDelegate));

        var ret = SystemVolumePlugin.InitializeVolume();
        if (ret != 0)
        {
            Debug.LogErrorFormat("Volume initialization failed with code {0}", ret);
        }

        if (linearMapping == null)
        {
            linearMapping = GetComponent<LinearMapping>();
        }

        // Allow get and set immediately after startup by behaving as though
        // previous instances occured before startup instead of the default 0.
        previousGet = -2 * minimumGetPeriod;
        previousSet = -2 * minimumSetPeriod;

        var initialVolume = Volume;
        linearMapping.value = initialVolume;
        desiredVolume = initialVolume;
        Debug.LogFormat("Starting volume is {0:f3}", initialVolume);
    }

    private static void Log(string str)
    {
        Debug.LogErrorFormat("SystemVolumePlugin: {0}", str);
    }
}
