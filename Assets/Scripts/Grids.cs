using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{
    [SerializeField] Transform[] cubes;
    [SerializeField] Color normalColor = new Color(.7176f, .8156f, .8235f);
    [SerializeField] Color hoverColor = new Color(1f, 1f, 1f);
    [SerializeField] Color invalidColor = new Color(.82f, .41f, .41f);
    public Plane rayCastPlane;
    public Mergable[] mergableSlots = new Mergable[6] { null, null, null, null, null, null };
    MeshRenderer[] cubeMeshRenderers = new MeshRenderer[6];
    Global global;

    // Start is called before the first frame update
    void Start()
    {
        global = Global.singletonInstance;
        Debug.Assert(global, "global not found");
        Debug.Assert(cubes.Length == 6, "must assign 6 cubes");
        for (int i = 0; i < 6; i++)
        {
            cubeMeshRenderers[i] = cubes[i].GetComponent<MeshRenderer>();
        }
        rayCastPlane = new Plane(Vector3.up, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMergable(Mergable obj)
    {
        for (int i = 5; i >= 0; i--)
        {
            if (mergableSlots[i] == null)
            {
                mergableSlots[i] = obj;
                obj.transform.position = new Vector3(cubes[i].position.x, 0f, cubes[i].position.z);
                obj.currentSlot = i;
                return;
            }
        }
    }

    public bool IsThereAnyFreeSlot()
    {
        foreach (var s in mergableSlots)
        {
            if (s == null)
            {
                return true;
            }
        }
        return false;
    }

    public bool MoveMergable(Mergable obj)
    {
        for (int i = 0; i < 6; i++)
        {
            var slotPos = new Vector3(cubes[i].position.x, 0f, cubes[i].position.z);
            if (Vector3.Distance(obj.transform.position, slotPos) < 0.3f)
            {
                if (i == obj.currentSlot)
                {
                    // same slot
                    return false;
                }
                else if (mergableSlots[i] == null)
                {
                    // found a free slot, move here
                    mergableSlots[obj.currentSlot] = null;
                    mergableSlots[i] = obj;
                    obj.currentSlot = i;
                    obj.transform.position = slotPos;
                    return true;
                }
                else // not free, there's another mergable
                {
                    if (obj.name == mergableSlots[i].name)
                    {
                        if (!obj.upperLevelMergable)
                        {
                            return false;
                        }
                        // name is the same, merge!"
                        mergableSlots[obj.currentSlot] = null;
                        var newobj = Instantiate(obj.upperLevelMergable);
                        newobj.name = newobj.name.Replace("(Clone)", "").Replace(" Variant", "");
                        Destroy(obj.gameObject);
                        Destroy(mergableSlots[i].gameObject);
                        mergableSlots[i] = newobj;
                        newobj.currentSlot = i;
                        newobj.transform.position = slotPos;
                        global.UiRefreshEvent.Invoke();
                    }
                }
            }
        }
        // moving failed
        return false;
    }

    public void Hover(Vector3 pos)
    {
        for (int i = 0; i < 6; i++)
        {
            var slotPos = new Vector3(cubes[i].position.x, 0f, cubes[i].position.z);
            if (Vector3.Distance(pos, slotPos) < 0.3f)
            {
                cubeMeshRenderers[i].material.color = hoverColor;
            }
            else
            {
                cubeMeshRenderers[i].material.color = normalColor;
            }
        }
    }

    public void NotHover()
    {
        for (int i = 0; i < 6; i++)
        {
            cubeMeshRenderers[i].material.color = normalColor;
        }
    }
}
