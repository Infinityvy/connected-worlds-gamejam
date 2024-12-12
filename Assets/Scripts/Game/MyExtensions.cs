using System.Collections;
using UnityEngine;

public static class MyExtensions
{

    public static Vector3 ConvertTo3DMovement(this Vector2 movement2D)
    {
        return new Vector3(movement2D.x, 0, movement2D.y);
    }

    public static void PlaySound(this AudioSource audioSource, string soundKey, float volume = 1f, float pitch = 1f)
    {
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.clip = AudioManager.getAudioClip(soundKey);
        audioSource.Play();
    }
    public static void PlaySoundIfReady(this AudioSource audioSource, string soundKey, float volume = 1f, float pitch = 1f) { if (!audioSource.isPlaying) audioSource.PlaySound(soundKey, volume, pitch); }

}