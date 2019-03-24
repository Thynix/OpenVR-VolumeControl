using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

// Set the text on the parent object to the source value.
public class SetText : MonoBehaviour
{
    private Text output;
    public LinearMapping source;

    // Start is called before the first frame update
    void Start()
    {
        output = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Is this too expensive? Could refresh only on changes.
        // TODO: What are boxing allocations, anyway?
        var tooth = Mathf.RoundToInt(source.value * 100 - 0.5f);
        output.text = $"{tooth:d} ({source.value:f3})";
    }
}