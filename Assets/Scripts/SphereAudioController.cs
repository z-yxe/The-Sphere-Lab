using UnityEngine;

// Script untuk mengatur efek suara dari sphere, saat ini masih hanya untuk saat menabrak dinding
public class SphereAudioController : MonoBehaviour
{
    public AudioClip wallHitSound;
    private AudioSource audioSource;

    // Inisialisasi komponen audio di awal
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    // Trigger sound jika menabrak object dengan tag wall
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            audioSource.PlayOneShot(wallHitSound);
        }
    }
}