using UnityEngine;
using UnityEngine.UI;

// Set the text on the parent object to the volume integer.
public class SetVolumeText : MonoBehaviour
{
    private Text output;
    public SystemVolumeLink source;

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