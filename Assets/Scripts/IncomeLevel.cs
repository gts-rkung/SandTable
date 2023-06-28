using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeLevel : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;
    Global global;

    // Start is called before the first frame update
    void Start()
    {
        global = Global.singletonInstance;
        Debug.Assert(global, "global not found");
        global.UiRefreshEvent.AddListener(Refresh);
        Debug.Assert(text, "text not assigned");
        Refresh();
    }

    void Refresh()
    {
        text.text = "Income Lvl " + global.incomeLevel;
    }
}
