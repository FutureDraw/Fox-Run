using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Инициализтор громкости
/// </summary>
public class VolumeInit : MonoBehaviour
{
    public List<string> volumeParameters = new List<string> { "MasterVolume", "Music", "Effects" };
    public AudioMixer audioMixer;

    void Awake()
    {
        foreach (var param in volumeParameters)
        {
            float defaultValue = param == "Effects" ? 0f : -80f;
            float volumeValue = PlayerPrefs.GetFloat(param, defaultValue);
            audioMixer.SetFloat(param, volumeValue);
        }
    }
}
