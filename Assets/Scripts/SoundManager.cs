using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager SoundMan;
    AudioSource audioSource;
    public AudioClip[] SwordSwings;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SoundMan = this;
    }

    public void PlaySound(AudioClip[] clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip[Random.Range(0, SwordSwings.Length)];
            audioSource.PlayOneShot(audioSource.clip); 
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
