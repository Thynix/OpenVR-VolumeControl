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
        output.text = source.FromScalar().ToString();
    }
}