using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelBall : MonoBehaviour
{
    [SerializeField] ParticleSystem dustVfx;
    Global global;
    Vector3 initPos;
    TouchInput touchInput;
    SoundAndMusic soundAndMusic;

    // Start is called before the first frame update
    void Start()
    {
        global = FindObjectOfType<Global>();
        Debug.Assert(global, "global not found");
        Debug.Assert(dustVfx, "dustVfx not assigned");
        initPos = transform.position;
        touchInput = FindObjectOfType<TouchInput>();
        Debug.Assert(touchInput, "touchInput not found");
        soundAndMusic = FindObjectOfType<SoundAndMusic>();
        Debug.Assert(soundAndMusic, "soundAndMusic not found");
    }

    // Update is called once per frame
    void Update()
    {
        var mp = global.magnet.transform.position;
        transform.position = new Vector3(-mp.y, initPos.y, mp.x - 100f);
        if (touchInput.touched && !touchInput.isDraggingSomething)
        {
            if (dustVfx.isStopped)
            {
                dustVfx.Play();
            }
            soundAndMusic.PlayGearSound();
        }
        else
        {
            if (dustVfx.isPlaying)
            {
                dustVfx.Stop();
            }
            soundAndMusic.StopGearSound();
        }
    }
}
