using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum SoundType
    {
        PlayerShoot,
        ShotgunShoot,
        PistolShoot,
        AutoShoot,
        TurretShoot,
        PlayerMove,
        EnemyHit,
        EnemyDie,
        PlayerDie,
        ReloadSound,
        HealSound
    }

    private static AudioClip[] soundClips;

    public static void Initialize(AudioClip[] clips)
    {
        soundClips = clips;
    }

    public static void PlaySound(SoundType soundType)
    {
        if (soundClips == null || soundClips.Length == 0)
        {
            Debug.LogError("Sound clips not initialized.");
            return;
        }

        int soundIndex = (int)soundType;
        if (soundIndex < 0 ||  soundIndex >= soundClips.Length)
        {
            Debug.LogError("Invalid sound type.");
            return;
        }

        AudioSource.PlayClipAtPoint(soundClips[soundIndex], Camera.main.transform.position);
    }
}