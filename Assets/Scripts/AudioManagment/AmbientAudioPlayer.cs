using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientAudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    private int ambientSoundCount = 8;

    private float minSoundInterval = 5.0f;
    private float maxSoundInterval = 30.0f;

    private void Start()
    {
        StartCoroutine(playAmbientSound());
    }

    private IEnumerator playAmbientSound()
    {
        yield return new WaitForEndOfFrame();

        string soundKey = "ambient" + Random.Range(0, ambientSoundCount);
        float soundDuration = AudioManager.getAudioClip(soundKey).length;

        audioSource.PlaySound(soundKey);

        yield return new WaitForSeconds(soundDuration + Random.Range(minSoundInterval, maxSoundInterval));

        StartCoroutine(playAmbientSound());
    }
}
