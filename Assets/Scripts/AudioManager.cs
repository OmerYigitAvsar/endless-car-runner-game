using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get;
        private set;
    }

    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        Instance = this;
        CreateAudioSources();
    }

    private void Start()
    {
        PlaySound("MainTheme");
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void CreateAudioSources()
    {
        if (sounds == null)
        {
            Debug.LogError(
                "AudioManager: Sounds listesi atanmamış.",
                this
            );

            return;
        }

        foreach (Sound sound in sounds)
        {
            if (sound == null)
                continue;

            if (sound.clip == null)
            {
                Debug.LogWarning(
                    sound.name +
                    " isimli sesin AudioClip'i bulunmuyor.",
                    this
                );

                continue;
            }

            sound.source =
                gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;

            sound.source.volume =
                Mathf.Clamp01(sound.volume);

            sound.source.loop = sound.loop;
            sound.source.playOnAwake = false;
            sound.source.spatialBlend = 0f;
        }
    }

    private void OnValidate()
    {
        if (sounds == null)
            return;

        foreach (Sound sound in sounds)
        {
            if (sound == null)
                continue;

            sound.volume =
                Mathf.Clamp01(sound.volume);

            if (sound.source == null)
                continue;

            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
            sound.source.clip = sound.clip;
        }
    }

    public void PlaySound(string soundName)
    {
        Sound sound = FindSound(soundName);

        if (sound == null)
            return;

        if (sound.source == null ||
            sound.clip == null)
        {
            Debug.LogWarning(
                soundName +
                " isimli ses oynatılamıyor; " +
                "AudioSource veya AudioClip eksik.",
                this
            );

            return;
        }

        if (sound.loop)
        {
            if (!sound.source.isPlaying)
                sound.source.Play();
        }
        else
        {
            /*
             * Hızlı tekrarlanan efektlerin
             * birbirini kesmesini engeller.
             */
            sound.source.PlayOneShot(
                sound.clip
            );
        }
    }

    public void StopSound(string soundName)
    {
        Sound sound = FindSound(soundName);

        if (sound == null)
            return;

        if (sound.source == null)
        {
            Debug.LogWarning(
                soundName +
                " isimli sesin AudioSource'u bulunamadı.",
                this
            );

            return;
        }

        sound.source.Stop();
    }

    public void SetVolume(
        string soundName,
        float volume
    )
    {
        Sound sound = FindSound(soundName);

        if (sound == null)
            return;

        volume = Mathf.Clamp01(volume);
        sound.volume = volume;

        if (sound.source != null)
            sound.source.volume = volume;
    }

    private Sound FindSound(string soundName)
    {
        if (string.IsNullOrWhiteSpace(soundName))
        {
            Debug.LogWarning(
                "AudioManager: Ses adı boş gönderildi.",
                this
            );

            return null;
        }

        if (sounds == null)
        {
            Debug.LogWarning(
                "AudioManager: Sounds listesi bulunamadı.",
                this
            );

            return null;
        }

        Sound sound = Array.Find(
            sounds,
            item =>
                item != null &&
                item.name == soundName
        );

        if (sound == null)
        {
            Debug.LogWarning(
                soundName +
                " isminde ses bulunamadı.",
                this
            );
        }

        return sound;
    }
}