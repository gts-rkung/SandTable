using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    Global global;
    Camera mainCam;
    LineRenderer lineRenderer;
    Vector3 prevPosition = new Vector3(0f, 0f, -99f);
    [SerializeField] float segLength = 0.02f;
    [SerializeField] int maxPoints = 5120;

    // Start is called before the first frame update
    void Start()
    {
        global = Global.singletonInstance;
        Debug.Assert(global, "global not found");
        mainCam = Camera.main;
        Debug.Assert(mainCam, "mainCam not found");
        lineRenderer = GetComponent<LineRenderer>();
        Debug.Assert(lineRenderer, "LineRenderer not found");
        lineRenderer.positionCount = 1;
        var pp = new Vector3(global.magnet.transform.position.x, global.magnet.transform.position.y, 0f);
        lineRenderer.SetPosition(0, pp);
        prevPosition = pp;
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer.positionCount >= maxPoints)
        {
            return;
        }
        var pp = new Vector3(global.magnet.transform.position.x, global.magnet.transform.position.y, 0f);
        if (Vector3.Distance(prevPosition, pp) >= segLength)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, pp);
            prevPosition = pp;
        }
    }

    void DrawByMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // mouse click down
            var mp = Input.mousePosition;
            var wp = mainCam.ScreenToWorldPoint(new Vector3(mp.x, mp.y, -mainCam.transform.position.z));
            lineRenderer.positionCount = 1;
            //print("m " + mp + " w " + wp);
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, wp);
            prevPosition = wp;
        }
        else if (Input.GetMouseButton(0))
        {
            // mouse hold
            var mp = Input.mousePosition;
            var wp = mainCam.ScreenToWorldPoint(new Vector3(mp.x, mp.y, -mainCam.transform.position.z));
            if (lineRenderer.positionCount < 1 ||
                Vector3.Distance(prevPosition, wp) > 0.05f)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, wp);
                prevPosition = wp;
            }
        }
    }

    public int NumberOfPoints()
    {
        return lineRenderer.positionCount;
    }
}
