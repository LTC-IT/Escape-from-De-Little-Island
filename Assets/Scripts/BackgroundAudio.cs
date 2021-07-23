using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


public class BackgroundAudio : MonoBehaviour
{
    AudioSource music;
    public AudioClip backMusic;
    void Start()
    {
        music = GetComponent<AudioSource>();
        music.Play();
    }

}