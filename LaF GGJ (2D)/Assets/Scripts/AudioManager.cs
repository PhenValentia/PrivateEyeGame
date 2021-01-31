using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour // Writen by Phen Valentia (Nicholas Salter) and Trang
{
    // Audio files
    public AudioSource NRoomBackgroundMusic;
    public AudioSource ERoomBackgroundMusic;

    // Audio
    public float currentAudioMax = 0.1f;

    void Start()
    {
        // Find AudioSource Attached to GameManager
        AudioSource[] AMs = GetComponents<AudioSource>();
        NRoomBackgroundMusic = AMs[0];
        ERoomBackgroundMusic = AMs[1];
        currentAudioMax = NRoomBackgroundMusic.volume;
        NRoomBackgroundMusic.Play();
        ERoomBackgroundMusic.Play();
        NRoomBackgroundMusic.volume = currentAudioMax;
        ERoomBackgroundMusic.volume = 0f;
        NRoomBackgroundMusic.loop = true;
        ERoomBackgroundMusic.loop = true;
    }

    // Play n-room background music
    public void playNRoomBackgroundMusic()
    {
        // Stop Previous Music from Playing
        NRoomBackgroundMusic.Stop();
        ERoomBackgroundMusic.Stop();
        // Load in New Music
        NRoomBackgroundMusic.clip = Resources.Load<AudioClip>("AudioSources/MainRoomN");
        // Start New Music
        NRoomBackgroundMusic.Play();
    }

    // Play e-room background music
    public void playERoomBackgroundMusic()
    {
        // Stop Previous Music from Playing
        NRoomBackgroundMusic.Stop();
        ERoomBackgroundMusic.Stop();
        // Load in New Music
        ERoomBackgroundMusic.clip = Resources.Load<AudioClip>("AudioSources/MainRoomE");
        // Start New Music
        ERoomBackgroundMusic.Play();
    }

    public void playEvil()
    {
        // Stop Previous Music from Playing
        NRoomBackgroundMusic.Stop();
        ERoomBackgroundMusic.Stop();
        // Load in New Music
        ERoomBackgroundMusic.clip = Resources.Load<AudioClip>("AudioSources/End");
        // Start New Music
        ERoomBackgroundMusic.Play();
    }

    public void playEnd()
    {
        // Stop Previous Music from Playing
        NRoomBackgroundMusic.Stop();
        ERoomBackgroundMusic.Stop();
        NRoomBackgroundMusic.loop = false;
        // Load in New Music
        NRoomBackgroundMusic.clip = Resources.Load<AudioClip>("AudioSources/Stinger");
        ERoomBackgroundMusic.clip = Resources.Load<AudioClip>("AudioSources/MenuMusic");
        // Start New Music
        NRoomBackgroundMusic.Play();
        ERoomBackgroundMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}