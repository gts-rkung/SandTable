using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAndMusic : MonoBehaviour
{
    //[SerializeField] AudioClip[] musicClips;
    [SerializeField] AudioSource gearAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(gearAudioSource, "gearAudioSource not assigned");
        //Debug.Assert(musicClips.Length <= 0, "musicClips not assigned");
    }

    public void PlayGearSound()
    {
        if (!gearAudioSource.isPlaying)
        {
            gearAudioSource.Play();
        }
    }

    public void StopGearSound()
    {
        if (gearAudioSource.isPlaying)
        {
            gearAudioSource.Stop();
        }
    }
}
