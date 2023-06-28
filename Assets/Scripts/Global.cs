using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Global : MonoBehaviour
{
    public static Global singletonInstance = null;
    [Header("----Generic----")]
    public bool inited;
    int saveVer = 1;
    public int money = 0;
    public int incomeLevel = 0;
    public int lightsLevel = 0; 
    UnityEvent uiRefreshEvent = new UnityEvent();
    public UnityEvent UiRefreshEvent
    {
        get
        {
            return uiRefreshEvent;
        }
    }
    public int sceneIndex;
    public string sceneName;
    public int incomeBase = 8;
    public int incomeLevelRatio = 3;
    public int unboxCount = 0;
    public float touchSpeedRatio = 4f;
    public float earnInterval = 4f;
    public int priceBase = 50;
    public int priceIncrement = 25;
    public int amountToNextLevel = 4096;
    [Header("----Demo And Debug----")]
    public bool moneyCheat;
    public bool viewGears;
    public bool isReplay;
    [Header("----Items In Scene----")]
    public Light mainDirectLight;
    public Camera followCamera;
    public Gear magnet;

    void Awake()
    {
        singletonInstance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        /*
#if !(DEVELOPMENT_BUILD || UNITY_EDITOR)  // disable debug log if not in development build or editor
        Debug.unityLogger.logEnabled = false;
#endif
        */
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneName = SceneManager.GetActiveScene().name;

        AdjustCameraFieldOfView();

        Debug.Assert(mainDirectLight, "mainDirectLight not assigned");
        Debug.Assert(followCamera, "followCamera not assigned");
        Debug.Assert(magnet, "magnet not assigned");

        if (PlayerPrefs.GetInt("saveVer", 1) != saveVer)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("saveVer", saveVer);
        }
        //money = PlayerPrefs.GetInt("money", 0);
        if (moneyCheat)
        {
            money = 99999;
        }
        //incomeLevel = PlayerPrefs.GetInt("incomeLevel", 0);
        //unboxCount = PlayerPrefs.GetInt("unboxCount", 0);

        /**** override for demo purpose ***/
        /*
        rowLevel = 1;
        speedLevel = 1;
        money = 200000;
        horsesOwned = "1";
        horseCheat = false;
        */
        /**** end ***/

        inited = true;
    }

    void AdjustCameraFieldOfView()
    {
        /* Auto adjust camera field of view according screen ratio
            9:20 = 0.45 -> FOV 60
            9:16 = 0.5625 -> FOV 51
        */
        float ratio = (float)Screen.width / (float)Screen.height;
        float fov = (ratio - 0.45f) / (0.5625f - 0.45f) * (51f - 60f) + 60f;
        fov = Mathf.Clamp(fov, 51f, 60f);
        print("Set main camera field of view to " + fov);
        Camera.main.fieldOfView = fov;

        /* for recording in landscape */
        /*
        if (Screen.width > Screen.height)
        {
            print("Landscape detected. Adjust cameral rotation and FOV");
            if (Camera.main.transform.eulerAngles.x > 16f)
            {
                Camera.main.transform.eulerAngles = new Vector3(16f, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
            }
            Camera.main.fieldOfView = 29f;
        }
        */
    }

    public void DelayLoadNextScene(float delay = 8f)
    {
        StartCoroutine(DelayLoad());

        IEnumerator DelayLoad()
        {
            yield return new WaitForSeconds(delay);
            int next = sceneIndex + 1;
            if (next >= SceneManager.sceneCountInBuildSettings)
            {
                next = 0;
            }
            SceneManager.LoadScene(next);
        }
    }

    // useful function that returns either 1 or -1
    static public int RandomNegative()
    {
        return Random.Range(0, 2) == 0 ? -1 : 1;
    }
}
