using System;
using Unity.VisualScripting;
using UnityEngine;

public enum SoundType
{
    Player,
    Level,
    Effects
}

public enum LevelMusic
{
    MainMenu,
    Level,
    GameOver
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;

    [SerializeField] private MusicList[] musicList;

    private static AudioManager instance;
    private AudioSource audioSource;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }

    public static void PlayMusic(LevelMusic music, float volume = 1)
    {
        AudioClip[] clips = instance.musicList[(int)music].Music;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] soundNames = Enum.GetNames(typeof(SoundType));
        string[] musicNames = Enum.GetNames(typeof(LevelMusic));
        
        Array.Resize(ref soundList, soundNames.Length);
        Array.Resize(ref musicList, musicNames.Length);

        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = soundNames[i];
        }

        for (int i = 0; i < musicList.Length; i++)
        {
            musicList[i].name = musicNames[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => soundList; }
    public string name;
    [SerializeField] private AudioClip[] soundList;
}

[Serializable]
public struct MusicList
{
    public AudioClip[] Music { get => musicList; }
    public string name;
    [SerializeField] private AudioClip[] musicList;
}
