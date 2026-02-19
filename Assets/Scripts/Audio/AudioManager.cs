using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;



public enum SoundType
{
    StepA,
    StepB,
    PlayerSteps,
    UI_Click,
    Mechanical
}

public enum LevelMusic
{
    MainMenu,
    Level_Introduction,
    Level_Gameplay,
    GameOver
}

[ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    // ===============================
    // ======= SINGLETON SAFE ========
    // ===============================

    public static AudioManager Instance;

    // Resetea estáticos aunque Domain Reload esté desactivado
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        Instance = null;
    }

    private void Awake()
    {
        if (!Application.isPlaying) return;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeSources();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    // ===============================
    // ======= MIXER CONFIG ==========
    // ===============================

    [Header("Configuración del Mixer")]
    public AudioMixerGroup bgmGroup;
    public AudioMixerGroup sfxGroup;

    [Header("Volúmenes Base")]
    [SerializeField] private float musicVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;

    // ===============================
    // ======= AUDIO SOURCES =========
    // ===============================

    private AudioSource _musicSource1;
    private AudioSource _musicSource2;
    private AudioSource _sfxSource;
    private AudioSource _loopSfxSource;
    private Coroutine _loopFadeCoroutine;

    private bool _isPlayingSource1 = true;
    private Coroutine _musicCoroutine;

    private void InitializeSources()
    {
        _musicSource1 = gameObject.AddComponent<AudioSource>();
        _musicSource2 = gameObject.AddComponent<AudioSource>();
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _loopSfxSource = gameObject.AddComponent<AudioSource>();

        // Música
        _musicSource1.outputAudioMixerGroup = bgmGroup;
        _musicSource2.outputAudioMixerGroup = bgmGroup;

        _musicSource1.loop = true;
        _musicSource2.loop = true;

        _musicSource1.playOnAwake = false;
        _musicSource2.playOnAwake = false;

        // SFX
        _sfxSource.outputAudioMixerGroup = sfxGroup;
        _sfxSource.playOnAwake = false;
        
        //Loop SFX (para tornillo y sonidos sostenidos)
        _loopSfxSource.outputAudioMixerGroup = sfxGroup;
        _loopSfxSource.loop = true;
        _loopSfxSource.playOnAwake = false;
    }

    // ===============================
    // ======= LIBRERÍA AUDIO ========
    // ===============================

    [Header("Biblioteca de Audio")]
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private MusicList[] musicList;

    // ===============================
    // ========= PLAY SFX ============
    // ===============================

    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        if (Instance == null) return;

        if ((int)sound >= Instance.soundList.Length) return;

        AudioClip[] clips = Instance.soundList[(int)sound].Sounds;
        if (clips == null || clips.Length == 0) return;

        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        Instance._sfxSource.PlayOneShot(randomClip, volume * Instance.sfxVolume);
    }
    // ===============================
    // ========= PLAY LOOP ==========
    // ===============================
    public static void PlayLoopSfx(SoundType sound, float volume = 1f)
    {
        if (Instance == null) return;
        if ((int)sound >= Instance.soundList.Length) return;

        var clips = Instance.soundList[(int)sound].Sounds;
        if (clips == null || clips.Length == 0) return;

        var clip = clips[0];

        if (Instance._loopSfxSource.isPlaying && Instance._loopSfxSource.clip == clip)
            return;

        Instance._loopSfxSource.clip = clip;
        Instance._loopSfxSource.volume = volume * Instance.sfxVolume;
        Instance._loopSfxSource.Play();
    }

    public static void StopLoopSfx(float fadeOut = 0f)
    {
        if (Instance == null) return;

        if (fadeOut <= 0f)
        {
            Instance._loopSfxSource.Stop();
            Instance._loopSfxSource.clip = null;
            return;
        }

        Instance.InstanceStopLoopFade(fadeOut);
    }

    private void InstanceStopLoopFade(float fadeOut)
    {
        if (_loopFadeCoroutine != null)
            StopCoroutine(_loopFadeCoroutine);

        _loopFadeCoroutine = StartCoroutine(StopLoopFadeRoutine(fadeOut));
    }

    private IEnumerator StopLoopFadeRoutine(float fadeOut)
    {
        if (!_loopSfxSource.isPlaying) yield break;

        float startVol = _loopSfxSource.volume;
        float t = 0f;

        while (t < fadeOut)
        {
            t += Time.deltaTime;
            _loopSfxSource.volume = Mathf.Lerp(startVol, 0f, t / fadeOut);
            yield return null;
        }

        _loopSfxSource.Stop();
        _loopSfxSource.clip = null;
        _loopSfxSource.volume = startVol;
    }


    // ===============================
    // ========= PLAY MUSIC ==========
    // ===============================

    public static void PlayMusic(LevelMusic music, float fadeDuration = 1.5f)
    {
        if (Instance == null) return;

        if ((int)music >= Instance.musicList.Length) return;

        AudioClip[] clips = Instance.musicList[(int)music].Music;
        if (clips == null || clips.Length == 0) return;

        AudioClip nextClip = clips[0];

        if (Instance._musicCoroutine != null)
            Instance.StopCoroutine(Instance._musicCoroutine);

        Instance._musicCoroutine =
            Instance.StartCoroutine(Instance.CrossfadeMusicRoutine(nextClip, fadeDuration));
    }

    private IEnumerator CrossfadeMusicRoutine(AudioClip newClip, float duration)
    {
        AudioSource activeSource = _isPlayingSource1 ? _musicSource1 : _musicSource2;
        AudioSource newSource = _isPlayingSource1 ? _musicSource2 : _musicSource1;

        if (activeSource.clip == newClip && activeSource.isPlaying)
            yield break;

        newSource.clip = newClip;
        newSource.volume = 0f;
        newSource.Play();

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            newSource.volume = Mathf.Lerp(0f, musicVolume, t);
            activeSource.volume = Mathf.Lerp(musicVolume, 0f, t);

            yield return null;
        }

        activeSource.Stop();
        activeSource.volume = 0f;

        _isPlayingSource1 = !_isPlayingSource1;
    }

#if UNITY_EDITOR
    // Más seguro que OnEnable (no te borra clips al recompilar)
    private void OnValidate()
    {
        string[] soundNames = Enum.GetNames(typeof(SoundType));
        string[] musicNames = Enum.GetNames(typeof(LevelMusic));

        if (soundList == null || soundList.Length != soundNames.Length)
            Array.Resize(ref soundList, soundNames.Length);

        if (musicList == null || musicList.Length != musicNames.Length)
            Array.Resize(ref musicList, musicNames.Length);

        for (int i = 0; i < soundList.Length; i++)
            soundList[i].name = soundNames[i];

        for (int i = 0; i < musicList.Length; i++)
            musicList[i].name = musicNames[i];
    }
#endif
}

// ===============================
// ========= STRUCTS =============
// ===============================

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds => soundList;

    public string name;
    [SerializeField] private AudioClip[] soundList;
}

[Serializable]
public struct MusicList
{
    public AudioClip[] Music => musicList;

    public string name;
    [SerializeField] private AudioClip[] musicList;
}
