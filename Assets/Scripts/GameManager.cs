using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SoundAudioClip[] soundAudioClipsArray;
    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.SoundType sound;
        public AudioClip audioClip;
    }
    void Start()
    {
        // Initialize the SoundManager with the audio clips from soundAudioClipsArray
        InitializeSoundManager();
    }

    void InitializeSoundManager()
    {
        AudioClip[] audioClips = new AudioClip[soundAudioClipsArray.Length];
        for (int i = 0; i < soundAudioClipsArray.Length; i++)
        {
            audioClips[i] = soundAudioClipsArray[i].audioClip;
        }
        SoundManager.Initialize(audioClips);
    }

}