using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsLevel : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;
    Global global;
    RgbLights rgbLights;

    // Start is called before the first frame update
    void Start()
    {
        global = FindObjectOfType<Global>();
        Debug.Assert(global, "global not found");
        global.UiRefreshEvent.AddListener(Refresh);
        Debug.Assert(text, "text not assigned");
        Refresh();
    }

    void Refresh()
    {
        text.text = "Lights Lvl " + global.lightsLevel;
    }
}
