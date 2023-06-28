using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RgbLights : MonoBehaviour
{
    [SerializeField] float speed = 240f;
    [SerializeField] GameObject[] ledModules;
    [SerializeField] ParticleSystem vfx;
    Global global;
    SandTable sandTable;
    IncomeBooks incomeBooks;

    // Start is called before the first frame update
    void Start()
    {
        global = FindObjectOfType<Global>();
        Debug.Assert(global, "global not found");
        Debug.Assert(vfx, "vfx not assigned");
        sandTable = FindObjectOfType<SandTable>();
        Debug.Assert(sandTable, "sandTable not found");
        incomeBooks = FindObjectOfType<IncomeBooks>();
        Debug.Assert(incomeBooks, "incomeBooks not found");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }

    public bool AddModule(Mergable mobj)
    {
        var oname = mobj.name;
        if (!oname.StartsWith("Mergable LED"))
        {
            return false;
        }
        int lvl = System.Convert.ToInt32(oname.Replace("Mergable LED Lvl", ""));
        if (lvl == global.lightsLevel + 1)
        {
            ledModules[global.lightsLevel].SetActive(true);
            ++global.lightsLevel;
            PlayerPrefs.SetInt("lightsLevel", global.lightsLevel);
            PlayVfx();
            Destroy(mobj.gameObject);
            global.mainDirectLight.intensity = 0.7f;
            global.UiRefreshEvent.Invoke();
            return true;
        }
        return false;
    }

    public void PlayVfx()
    {
        vfx.Play();
    }
}
