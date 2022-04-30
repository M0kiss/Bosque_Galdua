using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSFX : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void PlaySound(AudioClip clip)
    {
        //audioSource.PlayClipAtPoint(clip, new Vector3(0, 0, 0));
        float randomPitch = Random.Range(0.8f, 1.5f);
        audioSource.pitch = randomPitch;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
