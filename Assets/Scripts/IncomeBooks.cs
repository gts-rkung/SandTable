using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeBooks : MonoBehaviour
{
    float prevAngle;
    Global global;
    TouchInput touchInput;
    Money money;
    RgbLights rgbLights;
    float time;
    [SerializeField] GameObject[] bookModules;
    [SerializeField] float allowedRange = 0.7f;
    [SerializeField] ParticleSystem vfx;

    // Start is called before the first frame update
    void Start()
    {
        global = FindObjectOfType<Global>();
        Debug.Assert(global, "global not found");
        Debug.Assert(vfx, "vfx not assigned");
        touchInput = FindObjectOfType<TouchInput>();
        Debug.Assert(touchInput, "touchInput not found");
        money = FindObjectOfType<Money>();
        Debug.Assert(money, "money not found");
        rgbLights = FindObjectOfType<RgbLights>();
        Debug.Assert(rgbLights, "rgbLights not found");
    }

    // Update is called once per frame
    void Update()
    {
        time += (touchInput.touched && !touchInput.isDraggingSomething) ?
            Time.deltaTime * global.touchSpeedRatio : Time.deltaTime;
        if (time > global.earnInterval)
        {
            money.Earn();
            time = 0f;
        }
    }

    public bool AddModule(Mergable mobj)
    {
        var oname = mobj.name;
        if (!oname.StartsWith("Mergable Book"))
        {
            return false;
        }
        int lvl = System.Convert.ToInt32(oname.Replace("Mergable Book Lvl", ""));
        if (lvl == global.incomeLevel + 1)
        {
            for (int i = 0; i < bookModules.Length; i++)
            {
                bookModules[i].SetActive(i == global.incomeLevel);
            }
            ++global.incomeLevel;
            PlayerPrefs.SetInt("incomeLevel", global.incomeLevel);
            PlayVfx();
            Destroy(mobj.gameObject);
            global.UiRefreshEvent.Invoke();
            return true;
        }
        return false;
    }

    public bool WithInDropRange(Vector3 pos)
    {
        return Vector3.Distance(transform.position, pos) < allowedRange;
    }

    public void PlayVfx()
    {
        vfx.Play();
    }
}
