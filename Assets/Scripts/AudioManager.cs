using UnityEngine;

// Script utama untuk mengatur semua audio (BGM & SFX) secara global
public class AudioManager : MonoBehaviour
{
    // Singleton instance agar mudah diakses dari mana saja
    public static AudioManager instance;

    [Header("Audio Sources")]
    [Tooltip("Refrence komponen Audio Source untuk memainkan BGM.")]
    public AudioSource bgmSource;
    [Tooltip("Refrence komponen Audio Source untuk memainkan SFX (efek suara).")]
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    // Inisialisasi singleton & pastikan tidak duplikat antar scene
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Mulai otomatis BGM jika tersedia
    void Start()
    {
        if (backgroundMusic != null && bgmSource != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    // Memainkan SFX hover (dipanggil dari UI/Button)
    public void PlayHoverSound()
    {
        if (hoverSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(hoverSound);
        }
    }

    // Memainkan SFX klik (dipanggil dari UI/Button)
    public void PlayClickSound()
    {
        if (clickSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
    }
}