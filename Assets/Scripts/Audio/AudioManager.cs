using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio; // Necesario para el Mixer

public enum SoundType
{
    PlayerSteps,    // (Antes Player)
    UI_Click,       // (Antes Level)
    Mechanical      // (Antes Effects) - Para los tornillos
}

public enum LevelMusic
{
    MainMenu,
    Level_Introduction, // Narrativa
    Level_Gameplay,
    GameOver
}

[ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    // --- Singleton Seguro ---
    public static AudioManager Instance;

    [Header("Configuración del Mixer")]
    public AudioMixerGroup bgmGroup; // Arrastra aquí el grupo BGM del Mixer
    public AudioMixerGroup sfxGroup; // Arrastra aquí el grupo SFX del Mixer

    [Header("Fuentes de Audio (Internas)")]
    // Usamos 2 para música para poder hacer crossfade
    private AudioSource _musicSource1;
    private AudioSource _musicSource2;
    private AudioSource _sfxSource;
    private bool _isPlayingSource1 = true;

    [Header("Biblioteca de Audio")]
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private MusicList[] musicList;

    private void Awake()
    {
        // Lógica Singleton corregida
        if (Application.isPlaying)
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeSources(); // Crear los AudioSources por código
            }
            else
            {
                Destroy(gameObject); // Destruir el objeto duplicado, no la referencia
            }
        }
    }

    private void InitializeSources()
    {
        // Creamos los AudioSources dinámicamente para no ensuciar el Inspector
        _musicSource1 = gameObject.AddComponent<AudioSource>();
        _musicSource2 = gameObject.AddComponent<AudioSource>();
        _sfxSource = gameObject.AddComponent<AudioSource>();

        // Configuración Música
        _musicSource1.outputAudioMixerGroup = bgmGroup;
        _musicSource2.outputAudioMixerGroup = bgmGroup;
        _musicSource1.loop = true;
        _musicSource2.loop = true;

        // Configuración SFX
        _sfxSource.outputAudioMixerGroup = sfxGroup;
    }

    // --- REPRODUCCIÓN DE SFX ---
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        if (Instance == null) return;

        // Buscamos el clip en tu lista
        AudioClip[] clips = Instance.soundList[(int)sound].Sounds;
        if (clips.Length == 0) return;

        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        
        // PlayOneShot es perfecto para efectos cortos
        Instance._sfxSource.PlayOneShot(randomClip, volume);
    }

    // --- REPRODUCCIÓN DE MÚSICA CON FADE ---
    public static void PlayMusic(LevelMusic music, float fadeDuration = 1.5f)
    {
        if (Instance == null) return;

        AudioClip[] clips = Instance.musicList[(int)music].Music;
        if (clips.Length == 0) return;

        AudioClip nextClip = clips[0]; // Normalmente la música no es random, tomamos el primero

        Instance.StartCoroutine(Instance.CrossfadeMusicRoutine(nextClip, fadeDuration));
    }

    private IEnumerator CrossfadeMusicRoutine(AudioClip newClip, float duration)
    {
        AudioSource activeSource = _isPlayingSource1 ? _musicSource1 : _musicSource2;
        AudioSource newSource = _isPlayingSource1 ? _musicSource2 : _musicSource1;

        // Si ya está sonando la misma canción, no hacemos nada
        if (activeSource.clip == newClip && activeSource.isPlaying) yield break;

        newSource.clip = newClip;
        newSource.volume = 0;
        newSource.Play();

        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // Fade In del nuevo
            newSource.volume = Mathf.Lerp(0, 1, t);
            // Fade Out del viejo
            activeSource.volume = Mathf.Lerp(1, 0, t);

            yield return null;
        }

        activeSource.Stop();
        activeSource.volume = 0; // Asegurar silencio
        _isPlayingSource1 = !_isPlayingSource1; // Cambiamos el flag
    }

    // --- TU CÓDIGO DE EDITOR (INTACTO) ---
#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] soundNames = Enum.GetNames(typeof(SoundType));
        string[] musicNames = Enum.GetNames(typeof(LevelMusic));
        
        Array.Resize(ref soundList, soundNames.Length);
        Array.Resize(ref musicList, musicNames.Length);

        for (int i = 0; i < soundList.Length; i++)
            soundList[i].name = soundNames[i];

        for (int i = 0; i < musicList.Length; i++)
            musicList[i].name = musicNames[i];
    }
#endif
}

// --- TUS STRUCTS (INTACTOS) ---
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