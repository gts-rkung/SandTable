using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tabtale.TTPlugins; // CrazyLabs CLIK Plugin

public class ClikSdk : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    void Awake()
    {
        // Initialize CrazyLabs CLIK Plugin
        TTPCore.Setup();
    }
}
