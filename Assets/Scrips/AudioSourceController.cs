using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    public List<AudioClip> AudioClips;

    private AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(int clipNum)
    {
        AudioSource.clip = AudioClips[clipNum];
        AudioSource.Play();
    }
}
