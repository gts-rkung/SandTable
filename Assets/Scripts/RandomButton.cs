using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomButton : MonoBehaviour
{
    [SerializeField] Mergable gemLvl1;
    [SerializeField] Mergable ledLvl1;
    Button button;
    int price;
    [SerializeField] TMPro.TextMeshProUGUI priceText;
    Global global;
    Grids grids;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        global = FindObjectOfType<Global>();
        Debug.Assert(global, "global not found");
        yield return new WaitUntil(() => global.inited);
        global.UiRefreshEvent.AddListener(Refresh);
        Debug.Assert(gemLvl1, "gemLvl1 not found");
        button = GetComponent<Button>();
        Debug.Assert(button, "button not found");
        button.onClick.AddListener(ButtonClicked);
        Debug.Assert(priceText, "priceText not assigned");
        grids = FindObjectOfType<Grids>();
        Debug.Assert(grids, "grids not found");
        Refresh();
    }

    void ButtonClicked()
    {
        Mergable mobj;
        if (Random.Range(0, 2) == 0)
        //if (Random.value < 0.7f)
        {
            mobj = Instantiate(gemLvl1);
        }
        else
        {
            mobj = Instantiate(ledLvl1);
        }
        mobj.name = mobj.name.Replace("(Clone)", "").Replace(" Variant", "");
        grids.AddMergable(mobj);
        global.money -= price;
        global.unboxCount++;
        PlayerPrefs.SetInt("money", global.money);
        PlayerPrefs.SetInt("unboxCount", global.unboxCount);
        global.UiRefreshEvent.Invoke();
        if (global.followCamera.gameObject.activeSelf)
        {
            global.followCamera.gameObject.SetActive(false);
        }
    }

    void Refresh()
    {
        price = global.priceBase + global.priceIncrement * global.unboxCount;
        priceText.text = price.ToString();
        if (grids.IsThereAnyFreeSlot() &&
            global.money >= price)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
