using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI total;
    [SerializeField] TMPro.TextMeshProUGUI earning0;
    [SerializeField] TMPro.TextMeshProUGUI bigEarning;
    [SerializeField] float bigEarningSpeed = 1000f;
    [SerializeField] float bigEarningStay = 1f;
    float bigEarningTime;
    List<TMPro.TextMeshProUGUI> earningList = new List<TMPro.TextMeshProUGUI>();
    Vector3 earningInitPos;
    [SerializeField] int earningShitXRange = 30;
    Vector3 bigEarningInitPos;
    Global global;
    ProgressToNextLevel progressToNextLevel;
    IncomeBooks incomeBooks;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        global = FindObjectOfType<Global>();
        Debug.Assert(global, "global not found");
        yield return new WaitUntil(() => global.inited);
        Debug.Assert(total, "total not assigned");
        total.text = global.money.ToString();
        Debug.Assert(earning0, "earning0 not assigned");
        Debug.Assert(bigEarning, "bigEarning not assigned");
        progressToNextLevel = FindObjectOfType<ProgressToNextLevel>(true);
        Debug.Assert(progressToNextLevel, "progressToNextLevel not found");
        incomeBooks = FindObjectOfType<IncomeBooks>();
        Debug.Assert(incomeBooks, "incomeBooks not found");
        //earningInitPos = earning0.transform.position;
        earningInitPos = Camera.main.WorldToScreenPoint(incomeBooks.transform.position) +
            new Vector3(0, 100, 0);
        earning0.transform.position = earningInitPos;
        bigEarningInitPos = bigEarning.transform.position;
        for (int i = 1; i < 10; i++)
        {
            var clone = Instantiate(earning0, earningInitPos, Quaternion.identity, transform);
            clone.name = "Earning (" + i + ")";
            earningList.Add(clone);
        }
        global.UiRefreshEvent.AddListener(Refresh);
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var e in earningList)
        {
            if (e.gameObject.activeSelf)
            {
                e.transform.position += new Vector3(0f, 100f * Time.deltaTime, 0f);
                e.color -= new Color(0f, 0f, 0f, 1f * Time.deltaTime);
                if (e.color.a <= 0f)
                {
                    e.gameObject.SetActive(false);
                }
            }
        }
        if (bigEarning.gameObject.activeSelf)
        {
            bigEarningTime += Time.deltaTime;
            if (bigEarningTime > bigEarningStay)
            {
                bigEarning.transform.position = Vector3.MoveTowards(bigEarning.transform.position,
                    total.transform.position,
                    bigEarningSpeed * Time.deltaTime);
            }
            if (Mathf.Abs(bigEarning.transform.position.y - total.transform.position.y) < 1f)
            {
                bigEarning.gameObject.SetActive(false);
                global.money += 1000;
                global.UiRefreshEvent.Invoke();
            }
        }
    }

    public void Earn()
    {
        foreach (var e in earningList)
        {
            if (!e.gameObject.activeSelf)
            {
                e.transform.position = earningInitPos + new Vector3(Random.Range(-earningShitXRange, earningShitXRange), 0f, 0f);
                e.color = Color.white;
                int amount = global.incomeBase + global.incomeLevelRatio * global.incomeLevel;
                e.text = "+" + amount.ToString();
                e.gameObject.SetActive(true);
                global.money += amount;
                PlayerPrefs.SetInt("money", global.money);
                progressToNextLevel.Add();
                global.UiRefreshEvent.Invoke();
                return;
            }
        }
    }

    public void Refresh()
    {
        total.text = global.money.ToString();
    }
}
