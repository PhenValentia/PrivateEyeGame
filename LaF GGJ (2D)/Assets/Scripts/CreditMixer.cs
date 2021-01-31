using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditMixer : MonoBehaviour // Entirely Writen by Phen Valentia (Nicholas Salter)
{
    // Audio files
    public AudioSource NRoomBackgroundMusic;
    public AudioSource ERoomBackgroundMusic;

    // Audio
    public float currentAudioMax = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] AMs = GetComponents<AudioSource>();
        NRoomBackgroundMusic = AMs[0];
        ERoomBackgroundMusic = AMs[1];
        

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
