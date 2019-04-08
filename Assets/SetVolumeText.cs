using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

// Set the text on the parent object to the volume integer.
public class SetVolumeText : MonoBehaviour
{
    private Text output;
    public SystemVolumeLink source;
    public HapticRack hapticTrigger;

    // Start is called before the first frame update
    void Start()
    {
        output = GetComponent<Text>();
        UpdateText();

        hapticTrigger.onPulse.AddListener(UpdateText);
    }

    void UpdateText()
    {
        output.text = source.FromScalar().ToString();
    }
}