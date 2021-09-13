using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private float volume = 0.5f;
    private AudioSource audioSource;


    private void Awake()
    {
        volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
    }
    public void IncreaseVolume()
    {
        volume = Mathf.Clamp(volume + 0.1f, 0, 1);
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void decreaseVolume()
    {
        volume = Mathf.Clamp(volume - 0.1f, 0, 1);
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public float GetVolume()
    {
        return volume;
    }
}
