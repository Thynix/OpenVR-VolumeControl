using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Plugins.SystemVolumePlugin;
using UnityEngine.Assertions;

// Links the system volume to a LinearMapping.
public class SystemVolumeLink : MonoBehaviour
{
    private float Volume
    {
        get
        {
            // If multiple threads could run this at once, to prevent rate
            // limiting race conditions this check and setting allowGetTime must
            // be atomic.
            if (Time.time < allowGetTime)
                return desiredVolume;

            allowGetTime = Time.time + minimumGetPeriod;

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
            // limiting race conditions this check and setting allowSetTime must
            // be atomic.
            if (Time.time < allowSetTime)
                return;

            allowSetTime = Time.time + minimumSetPeriod;

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

    [Tooltip("Maximum system volume get calls per second")]
    public float getPerSecond = 20;
    private float minimumGetPeriod;

    [Tooltip("Maximum system volume set calls per second")]
    public float setPerSecond = 20;
    private float minimumSetPeriod;

    private float lastKnownVolume;
    private float desiredVolume;

    // Time at or after which a call to get the system volume is allowed. Used for rate limiting.
    private float allowGetTime;

    // Time at or after which a call to set the system volume is allowed. Used for rate limiting.
    private float allowSetTime;

    private void Update()
    {
        Volume = linearMapping.value;
    }

    public int FromScalar()
    {
        // Using Volume here can cause flickering for some values at the system
        // volume get update framerate. Always using desiredVolume avoids that
        // flickering.
        // TODO: Is this avoidable?
        return FromScalar(desiredVolume);
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
        Assert.IsTrue(getPerSecond > 0, "getPerSecond must be positive");
        Assert.IsTrue(setPerSecond > 0, "setPerSecond must be positive");

        // Seconds between calls is more useful during runtime, but calls per
        // second is easier to set.
        minimumGetPeriod = 1 / getPerSecond;
        minimumSetPeriod = 1 / setPerSecond;

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
