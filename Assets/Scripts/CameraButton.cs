using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraButton : MonoBehaviour
{
    Button button;
    Global global;

    // Start is called before the first frame update
    void Start()
    {
        global = Global.singletonInstance;
        Debug.Assert(global, "global not found");
        button = GetComponent<Button>();
        Debug.Assert(button, "button not found");
        button.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        global.followCamera.gameObject.SetActive(!global.followCamera.gameObject.activeSelf);
    }
}
