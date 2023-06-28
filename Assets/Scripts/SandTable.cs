using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandTable : MonoBehaviour
{
    [SerializeField] float allowedRange = 2.5f;
    RgbLights rgbLights;
    IncomeBooks incomeBooks;
    [SerializeField] Material martialNormal;
    [SerializeField] Material martialHover;
    [SerializeField] MeshRenderer[] meshRenderers;

    // Start is called before the first frame update
    void Start()
    {
        rgbLights = FindObjectOfType<RgbLights>();
        Debug.Assert(rgbLights, "rgbLights not found");
        incomeBooks = FindObjectOfType<IncomeBooks>();
        Debug.Assert(incomeBooks, "rgbincomeBooksights not found");
        Debug.Assert(martialNormal, "martialNormal not assigned");
        Debug.Assert(martialHover, "martialHover not assigned");
        Debug.Assert(meshRenderers.Length == 2, "meshRenderers length must be 2");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool AddMergable(Mergable mobj)
    {
        //print("st add " + Vector3.Distance(transform.position, mobj.transform.position));
        if (WithInDropRange(mobj.transform.position) ||
            incomeBooks.WithInDropRange(mobj.transform.position))
        {
            //print("in range");
            if (mobj.name.StartsWith("Mergable LED"))
            {
                return rgbLights.AddModule(mobj);
            }
            else if (mobj.name.StartsWith("Mergable Book"))
            {
                return incomeBooks.AddModule(mobj);
            }
        }
        return false;
    }

    bool WithInDropRange(Vector3 pos)
    {
        return Vector3.Distance(transform.position, pos) < allowedRange;
    }

    public void Hover(Vector3 pos)
    {
        if (WithInDropRange(pos) || incomeBooks.WithInDropRange(pos))
        {
            foreach (var m in meshRenderers)
            {
                m.material = martialHover;
            }
        }
        else
        {
            foreach (var m in meshRenderers)
            {
                m.material = martialNormal;
            }
        }
    }

    public void NotHover()
    {
        foreach (var m in meshRenderers)
        {
            m.material = martialNormal;
        }
    }
}
