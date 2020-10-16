using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    static AudioPlayer instance;
    public static AudioPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("BGMPlayer");
                instance = go.AddComponent<AudioPlayer>();
                return instance;
            }
            return instance;
        }
    }

    [Header("Music Settings")]
    public AudioClip musicAudioClip;
    public bool musicPlayOnAwake = true;
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [Header("Ambient Settings")]
    public AudioClip ambientAudioClip;
    public bool ambientPlayOnAwake = true;
    [Range(0f, 1f)]
    public float ambientVolume = 1f;

    [Header("Sound Effect Settings")]
    public SFXInfo[] soundEffectsSettings;
    Dictionary<SFXName, SFXInfo> soundEffects;

    public enum SFXName
    {
        Click,
        Link
    }

    [System.Serializable]
    public class SFXInfo
    {
        public SFXName name;
        public AudioClip audioClip;
        [Range(0f, 1f)]
        public float volume = 1f;
    }

    [Header("Ending Music Settings")]
    public AudioClip endingAudioClip;
    [Range(0f, 1f)]
    public float endingVolume = 1;
    bool endingMusicPlayed = false;

    AudioSource musicAudioSource;
    AudioSource ambientAudioSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicAudioClip != null)
        {
            musicAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource.clip = musicAudioClip;
            musicAudioSource.loop = true;
            musicAudioSource.volume = musicVolume;
            if (musicPlayOnAwake) musicAudioSource.Play();
        }

        if (ambientAudioClip != null)
        {
            ambientAudioSource = gameObject.AddComponent<AudioSource>();
            ambientAudioSource.clip = ambientAudioClip;
            ambientAudioSource.loop = true;
            ambientAudioSource.volume = ambientVolume;
            if (ambientPlayOnAwake) ambientAudioSource.Play();
        }

        soundEffects = new Dictionary<SFXName, SFXInfo>();
        for (int i = 0; i < soundEffectsSettings.Length; i++)
        {
            soundEffects.Add(soundEffectsSettings[i].name, soundEffectsSettings[i]);
        }
    }

    public void PlayBGM()
    {
        if (musicAudioSource != null) musicAudioSource.Play();
        if (ambientAudioSource != null) ambientAudioSource.Play();
    }

    public void StopBGM()
    {
        if (musicAudioSource != null) musicAudioSource.Stop();
        if (ambientAudioSource != null) ambientAudioSource.Stop();
    }

    public void MuteBGM(float fadeTime)
    {
        if (musicAudioSource != null)
            StartCoroutine(VolumeFade(musicAudioSource, 0f, fadeTime));
        if (ambientAudioSource != null)
            StartCoroutine(VolumeFade(ambientAudioSource, 0f, fadeTime));
    }

    public void UnmuteBGM(float fadeTime)
    {
        if (musicAudioSource != null)
            StartCoroutine(VolumeFade(musicAudioSource, 0f, fadeTime));
        if (ambientAudioSource != null)
            StartCoroutine(VolumeFade(ambientAudioSource, 0f, fadeTime));
    }

    public void PlaySFX(SFXName name)
    {
        if (!soundEffects.ContainsKey(name)) return;
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundEffects[name].audioClip;
        audioSource.loop = false;
        audioSource.volume = soundEffects[name].volume;
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length);
    }

    IEnumerator VolumeFade(AudioSource source, float finalVolume, float fadeTime)
    {
        float volumeDiff = Mathf.Abs(source.volume - finalVolume);
        while (!Mathf.Approximately(source.volume, finalVolume))
        {
            float deltaVolume = volumeDiff / fadeTime * Time.deltaTime;
            source.volume = Mathf.MoveTowards(source.volume, finalVolume, deltaVolume);
            yield return null;
        }
        source.volume = finalVolume;
    }

    public void PlayEndingMusic()
    {
        if (!endingMusicPlayed)
        {
            MuteBGM(1f);
            if (endingAudioClip != null)
            {
                AudioSource endingAudioSource = gameObject.AddComponent<AudioSource>();
                endingAudioSource.clip = endingAudioClip;
                endingAudioSource.loop = false;
                endingAudioSource.volume = endingVolume;
                endingAudioSource.Play();
            }
        }
        endingMusicPlayed = true;
    }
}