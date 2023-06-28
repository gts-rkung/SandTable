using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public float speed = 10f;
    //public int gears;
    //public float toParentRatio = -1f;
    //public float textureCameraDistance = -5f;
    //public float sandPlanScale = 2.4f;
    Global global;
    TouchInput touchInput;

    // Start is called before the first frame update
    void Start()
    {
        global = FindObjectOfType<Global>();
        Debug.Assert(global, "global not found");
        /*
        gears = (int)(transform.localScale.x * Mathf.PI / 0.3f);
        var parentGear = transform.parent?.GetComponent<Gear>();
        if (parentGear != null)
        {
            int pg = (int)(parentGear.transform.localScale.x * Mathf.PI / 0.3f);
            toParentRatio = (float)pg / (float)gears;
        }*/
        if (!global.viewGears)
        {
            var mr = GetComponent<MeshRenderer>();
            Debug.Assert(mr, "mr not found");
            mr.enabled = false;
        }
        touchInput = FindObjectOfType<TouchInput>();
        Debug.Assert(touchInput, "touchInput not found");
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        if (touchInput.touched && !touchInput.isDraggingSomething)
        {
            step *= global.touchSpeedRatio;
        }
        transform.Rotate(0f, 0f, step, Space.Self);
    }
}
