using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mergable : MonoBehaviour
{
    public Mergable upperLevelMergable;
    public int currentSlot = -1;
    [SerializeField] ParticleSystem vfx;
    Grids grids;
    SandTable sandTable;
    Camera mainCam;
    TouchInput touchInput;
    bool isDragged;
    Vector3 positionBeforeDragging;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(vfx, "vfx not assigned");
        vfx.Play();
        grids = FindObjectOfType<Grids>();
        Debug.Assert(grids, "grids not found");
        sandTable = FindObjectOfType<SandTable>();
        Debug.Assert(sandTable, "sandTable not found");
        mainCam = Camera.main;
        Debug.Assert(mainCam, "mainCam not found");
        touchInput = FindObjectOfType<TouchInput>();
        Debug.Assert(touchInput, "touchInput not found");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDrag()
    {
        if (!isDragged)
        {
            isDragged = touchInput.isDraggingSomething = true;
            positionBeforeDragging = transform.position;
        }
        var mp = Input.mousePosition;
        Ray ray = mainCam.ScreenPointToRay(new Vector3(mp.x, mp.y, mainCam.transform.position.y));
        float distance;
        grids.rayCastPlane.Raycast(ray, out distance);
        var rp = ray.GetPoint(distance);
        //print(" mp " + mp + " rp " + rp);
        transform.position = new Vector3(rp.x, 0f, rp.z);
        grids.Hover(transform.position);
        sandTable.Hover(transform.position);
    }

    void OnMouseEnter()
    {
        /*if (currentSlot >= 0)
        {
            grids.MouseEnterSlot(currentSlot);
        }*/
    }

    void OnMouseUp()
    {
        /*if (currentSlot >= 0)
        {
            grids.MouseExitSlot(currentSlot);
        }*/
        isDragged = touchInput.isDraggingSomething = false;
        grids.NotHover();
        sandTable.NotHover();
        if (!grids.MoveMergable(this))
        {
            if (!sandTable.AddMergable(this))
            {
                transform.position = positionBeforeDragging;
            }
        }
    }
}
