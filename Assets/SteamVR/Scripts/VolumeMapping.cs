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
            if (calledGetThisFrame)
                return volume;

            calledGetThisFrame = true;
            // TODO: Get volume from plugin.
            return volume;
        }
        set
        {
            if (calledSetThisFrame)
            {
                // TODO: Rate limit this message?
                Debug.LogWarning("Attempted to set volume too often.");
                return;
            }

            calledSetThisFrame = true;
            // TODO: Set volume with plugin.
            volume = value;
        }
    }

    private float volume = 0.42f;
    private bool calledGetThisFrame;
    private bool calledSetThisFrame;

    void Update()
    {
        // Prevent calling the system volume plugin more than once per frame.
        // TODO: Might want to have a more configurable rate limit?
        calledGetThisFrame = false;
        calledSetThisFrame = false;
    }
}
