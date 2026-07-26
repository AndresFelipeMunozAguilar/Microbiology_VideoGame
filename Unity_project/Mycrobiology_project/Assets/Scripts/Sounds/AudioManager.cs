using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip[] sfxClips;
    private string currentMusic = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PlayMusic("menuMusic");
    }   

    public void PlaySFX(string clipName)
    {
        AudioClip clip = System.Array.Find(sfxClips, c => c.name == clipName);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + clipName);
        }
    }

    public void PlayMusic(string clipName)
    {
        if (currentMusic == clipName)
        {
            return;
        }

        AudioClip clip = System.Array.Find(sfxClips, c => c.name == clipName);
        if (clip != null)
        {
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.Play();
            currentMusic = clipName;
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + clipName);
        }
    }
}