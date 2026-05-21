using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("BGM")]
    [SerializeField]
    private AudioClip bgmTitle;
    [SerializeField]
    private AudioClip bgmGame;
    [Header("効果音")]
    [SerializeField]
    private AudioClip seJump;
    [SerializeField]
    private AudioClip seItem;
    [SerializeField]
    private AudioClip seStomp;
    [SerializeField]
    private AudioClip seGameOver;
    [SerializeField]
    private AudioClip seClear;

    [Header("音量設定")]
    [SerializeField]
    private float bgmVolume = 0.5f;

    [SerializeField]
    private float seVolume = 1.0f;

    private AudioSource bgmSource;
    private AudioSource seSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void SetupAudioSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;
        bgmSource.playOnAwake = false;

        seSource = gameObject.AddComponent<AudioSource>();
        seSource.loop = false;
        seSource.volume = seVolume;
        seSource.playOnAwake = false;
    }

    public void PlayBGM(string bgmName)
    {

        AudioClip clip = null;

        switch (bgmName)
        {
            case "title":
                clip = bgmTitle;
                break;
            case "game":
                clip = bgmGame;
                break;
        }

        if (clip == null)
        {
            StopBGM();
            return;
        }

        if (clip != null && bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }

    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySE(string seName)
    {
        AudioClip clip = null;
        switch (seName)
        {
            case "jump":
                clip = seJump;
                break;
            case "item":
                clip = seItem;
                break;
            case "stomp":
                clip = seStomp;
                break;
            case "gameover":
                clip = seGameOver;
                break;
            case "clear":
                clip = seClear;
                break;

        }

        if (clip != null)
        {
            seSource.PlayOneShot(clip, seVolume);
        }

    }
}
