using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Background Music")]
    public AudioClip[] MusicTracks; // Array of music tracks
    public float musicVolume = 0.5f; // Default music volume
    private int currentMusicIndex = 0;

    [Header("Sound Effects")]
    public AudioClip BlasterShotOne;
    public AudioClip BlasterShotTwo;
    public AudioClip GameOver;
    public AudioClip LevelCompletion;

    public float sfxVolume = 1.0f; // Default SFX volume

    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize AudioSources
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            // Set default volumes
            musicSource.volume = musicVolume;
            sfxSource.volume = sfxVolume;

            Debug.Log("AudioManager initialized.");

            // Start playing the first music track if available
            if (MusicTracks.Length > 0)
            {
                PlayMusicTrack(currentMusicIndex);
            }
            else
            {
                Debug.LogError("No music tracks available in MusicTracks array.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusicTrack(int index)
    {
        if (index >= 0 && index < MusicTracks.Length)
        {
            Debug.Log("Playing music track: " + MusicTracks[index].name);
            musicSource.clip = MusicTracks[index];
            musicSource.loop = true;
            musicSource.Play();
            currentMusicIndex = index;
        }
        else
        {
            Debug.LogError("Invalid music track index.");
        }
    }

    public void SwitchToNextMusicTrack()
    {
        int nextIndex = (currentMusicIndex + 1) % MusicTracks.Length;
        PlayMusicTrack(nextIndex);
    }

    public void MuteAudio()
    {
        AudioListener.volume = 0f;
    }

    public void UnmuteAudio()
    {
        AudioListener.volume = 1f;
    }
}
