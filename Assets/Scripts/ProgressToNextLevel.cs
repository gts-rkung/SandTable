using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressToNextLevel : MonoBehaviour
{
    [SerializeField] Image foreground;
    [SerializeField] TMPro.TextMeshProUGUI title;
    [SerializeField] Button nextButton;
    Global global;
    DrawLine drawLine;

    // Start is called before the first frame update
    void Start()
    {
        global = Global.singletonInstance;
        Debug.Assert(global, "global not found");
        Debug.Assert(foreground, "foreground not assigned");
        foreground.gameObject.SetActive(false);
        Debug.Assert(title, "title not assigned");
        title.text = SceneManager.GetActiveScene().name.ToUpper();
        Debug.Assert(nextButton, "nextButton not assigned");
        nextButton.onClick.AddListener(NextButtonClicked);
        foreground.fillAmount = 0f;
        drawLine = FindObjectOfType<DrawLine>();
        Debug.Assert(drawLine, "drawLine not found");
    }

    void Update()
    {
        foreground.fillAmount = (float)drawLine.NumberOfPoints() / (float)global.amountToNextLevel;
    }

    public void Add()
    {
        if (!foreground.gameObject.activeSelf)
        {
            if (global.lightsLevel + global.incomeLevel >= 5)
            {
                foreground.gameObject.SetActive(true);
                title.text = "NEXT LEVEL";
            }
            return;
        }
        if (drawLine.NumberOfPoints() >= global.amountToNextLevel)
        {
            nextButton.gameObject.SetActive(true);
        }
    }

    void NextButtonClicked()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        int s = global.sceneIndex + 1;
        if (s >= SceneManager.sceneCountInBuildSettings)
        {
            s = 0;
        }
        SceneManager.LoadScene(s);
    }
}
