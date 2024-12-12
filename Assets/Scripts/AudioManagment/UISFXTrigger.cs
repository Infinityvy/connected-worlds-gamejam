using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISFXTrigger : MonoBehaviour
{
    private UIAudioSource source;

    void Start()
    {
        source = GameObject.Find("UIAudioSource").GetComponent<UIAudioSource>();
    }

    public void playHover()
    {
        source.sfxSource.PlaySound("button_hover");
    }

    public void playClick()
    {
        source.sfxSource.PlaySoundIfReady("button_press_0");
    }

    public void playClickMaster()
    {
        source.masterSource.PlaySoundIfReady("button_press_1");
    }

    public void playClickMusic()
    {
        source.musicSource.PlaySoundIfReady("piano_0");
    }
}
