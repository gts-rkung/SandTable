using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Height2NormalMap;

public class GenNormalMap : MonoBehaviour
{
    [SerializeField] bool gaussianBlur = false;
    [SerializeField] float gaussianBlurSampleFactor = 1f;
    [SerializeField] float normalMapBumpEffect = 0.5f;
    public RenderTexture inputRenderTexture;
    public RenderTexture intermediateRenderTexture;
    public RenderTexture destRenderTexture;
    //NormalMapGenerator generator;
    GaussianBlurFilter gaussianBlurFilter;
    SobelNormalMapFilter sobelNormalMapFilter;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(inputRenderTexture, "inputRenderTexture not assigned");
        Debug.Assert(intermediateRenderTexture, "intermediateRenderTexture not assigned");
        Debug.Assert(destRenderTexture, "destRenderTexture not assigned");
        
        gaussianBlurFilter = new GaussianBlurFilter()
            {
                iteration = 1,
                sampleFactor = gaussianBlurSampleFactor
            };
        sobelNormalMapFilter = new SobelNormalMapFilter()
            {
                bumpEffect = normalMapBumpEffect
            };
        StartCoroutine(RefreshLoop());
    }

    void Apply()
    {
        sobelNormalMapFilter.bumpEffect = normalMapBumpEffect;
        if (gaussianBlur)
        {
            gaussianBlurFilter.sampleFactor = gaussianBlurSampleFactor;
            gaussianBlurFilter.Apply(inputRenderTexture, intermediateRenderTexture);
            sobelNormalMapFilter.Apply(intermediateRenderTexture, destRenderTexture);
        }
        else
        {
            sobelNormalMapFilter.Apply(inputRenderTexture, destRenderTexture);
        }
    }

    IEnumerator RefreshLoop()
    {
        while (true)
        {
#if UNITY_EDITOR
            yield return null;
#else
            yield return null;
            yield return null;
#endif
            Apply();
        }
    }
}
