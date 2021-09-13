using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;
    private float volume = 0.5f;
    public enum Sound
    {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        GameOver,
    }
    private void Awake()
    {
        Instance = this;
        audioSource = gameObject.GetComponent<AudioSource>();

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();

        foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
        volume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
    }

    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(soundAudioClipDictionary[sound], volume);
    }

    public void IncreaseVolume()
    {
        volume = Mathf.Clamp(volume + 0.1f, 0, 1);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }
    public void decreaseVolume()
    {
        volume = Mathf.Clamp(volume - 0.1f, 0, 1);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }
    public float GetVolume()
    {
        return volume;
    }
}
