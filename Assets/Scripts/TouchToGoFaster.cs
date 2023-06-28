using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToGoFaster : MonoBehaviour
{
    float idleTime;
    TMPro.TextMeshProUGUI text;
    [SerializeField] bool enableHint = true;
    [SerializeField] float timeToHint = 5f;
    [SerializeField] float firstTimeToHint = 3f;
    TouchInput touchInput;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        Debug.Assert(text, "text not found");
        text.enabled = false;
        idleTime = timeToHint - firstTimeToHint;
        touchInput = FindObjectOfType<TouchInput>();
        Debug.Assert(touchInput, "touchInput not found");
    }

    // Update is called once per frame
    void Update()
    {
        if (!enableHint)
        {
            return;
        }
        idleTime += Time.deltaTime;
        if (idleTime >= timeToHint)
        {
            text.enabled = true;
        }
        if (touchInput.touched && !touchInput.isDraggingSomething)
        {
            text.enabled = false;
            idleTime = 0f;
        }
    }
}
