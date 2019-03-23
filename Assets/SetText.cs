using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

// Set the text on the parent object to the source value.
public class SetText : MonoBehaviour
{
    private Text output;
    public VolumeMapping source;

    // Start is called before the first frame update
    void Start()
    {
        output = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // Values are from 0.0 to 1.0; this computation matches that of
        // HapticRack with 100 teeth.
        output.text = $"{Mathf.RoundToInt(source.Volume * 100 - 0.5f):d}";
    }
}