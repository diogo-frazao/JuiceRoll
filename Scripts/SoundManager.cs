using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private AudioClip[] squashSounds;

    private AudioSource myAudioSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        myAudioSource = GetComponent<AudioSource>();
    }


    public void PlayRandomFoodSmash()
    {
        int randomSquashIndex = Random.Range(0, squashSounds.Length - 1);
        myAudioSource.clip = squashSounds[randomSquashIndex];
        myAudioSource.Play();
    }
}
