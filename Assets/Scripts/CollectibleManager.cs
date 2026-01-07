using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Script utama untuk mengatur spawn, pooling dan pengambilan collectible (dengan struktur data queue), serta update score
public class CollectibleManager : MonoBehaviour
{
    [Header("Core Settings")]
    [SerializeField] private Transform sphereTransform;
    public GameObject collectiblePrefab;
    public GameObject destructionEffectPrefab;
    public bool canRespawn = true;

    [Header("Object Pool Configuration")]
    [Tooltip("Ukuran buffer sebagai persentase dari jumlah spawn maksimum. 0.25 = 25% buffer.")]
    [Range(0.1f, 1.0f)]
    public float poolBuffer = 0.25f;
    private Queue<GameObject> collectiblePool;

    [Header("Spawn Configuration")]
    [Tooltip("Refrence ke game object yang menjadi area spawn untuk collectible.")]
    public Collider spawnArea;
    private const int MAX_SPAWN_ATTEMPTS = 50;
    private Bounds spawnBounds;
    public int minSpawn = 5;
    public int maxSpawn = 15;
    public float respawnDelay = 3f;
    [Tooltip("Jarak minimal collectible muncul agar tidak spawn diatas player.")]
    public float safeDistance = 1f;

    [Header("Score Configuration")]
    [Tooltip("Reference ke komponen TextMeshPro untuk menampilkan score.")]
    public TextMeshProUGUI scoreText;
    private int score = 0;

    [Header("Audio Configuration")]
    public AudioClip pickupSound;
    private AudioSource audioSource;

    // Untuk validasi awal
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("CollectibleManager butuh komponen AudioSource!", this);
        }
        if (sphereTransform == null)
        {
            Debug.LogError("sphereTransform belum di-assign!", this);
        }
        if (collectiblePrefab == null)
        {
            Debug.LogError("Collectible Prefab belum di-assign!", this);
        }
        if (spawnArea == null)
        {
            Debug.LogError("Spawn Area belum di-assign!", this);
        }
    }

    // Untuk inisialisasi awal
    void Start()
    {
        // Inisialisasi object pooling
        InitializePool();

        if (spawnArea == null) return;

        spawnBounds = spawnArea.bounds;
        int amountToSpawn = Random.Range(minSpawn, maxSpawn + 1);

        for (int i = 0; i < amountToSpawn; i++)
        {
            SpawnFromPool();
        }

        UpdateScore();
    }

    // Method untuk membuat & mengisi pool dengan object di awal permainan.
    private void InitializePool()
    {
        // Ukuran buffer menyesuaikan dari ukuran maxSpawn
        float bufferSize = maxSpawn * poolBuffer;
        int poolSize = maxSpawn + Mathf.CeilToInt(bufferSize);

        // Menggunakan stuktur data Queue untuk object pooling
        collectiblePool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject collectible = Instantiate(collectiblePrefab);
            collectible.SetActive(false);
            collectible.transform.SetParent(this.transform);
            collectiblePool.Enqueue(collectible);
        }
    }

    // Method untuk mengambil object dari pool, lalu mengaktifkannya.
    private void SpawnFromPool()
    {
        if (collectiblePool.Count > 0)
        {
            // Ambil object dari antrian pertama di pool.
            GameObject collectible = collectiblePool.Dequeue();

            collectible.transform.position = RandomSpawn();
            collectible.SetActive(true);
        }
    }

    // Method untuk menangani ketika menabrak collectible.
    public void OnCollectibleHit(GameObject collectible)
    {
        score++;
        UpdateScore();

        if (audioSource != null && pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }

        if (destructionEffectPrefab != null)
        {
            Instantiate(destructionEffectPrefab, collectible.transform.position, Quaternion.identity);
        }

        ReturnToPool(collectible);

        // Cek apakah collectible bisa respawn atau tidak (Stage 7 atau 8).
        if (canRespawn)
        {
            StartCoroutine(RespawnAfterDelay());
        }
    }

    // Method untuk menonaktifkan object dan mengembalikannya ke pool.
    private void ReturnToPool(GameObject collectible)
    {
        collectible.SetActive(false);
        collectiblePool.Enqueue(collectible);
    }

    // Coroutine untuk menangani proses respawn dengan jeda waktu.
    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnFromPool();
    }

    // Method untuk random spawn di dalam spawnArea yang bukan didalam player (Sphere).
    private Vector3 RandomSpawn()
    {
        if (sphereTransform == null)
        {
            return GenerateRandomPos();
        }

        // Mencoba hingga menemukan posisi aman batasi hingga 50 kali percobaan agar tidak infinite loop jika terjadi kesalahan system.
        int attempts = 0;
        while (attempts < MAX_SPAWN_ATTEMPTS)
        {
            Vector3 spawnPosition = GenerateRandomPos();
            if (Vector3.Distance(spawnPosition, sphereTransform.position) > safeDistance)
            {
                return spawnPosition;
            }
            attempts++;
        }

        // Jika setelah 50 kali percobaan gagal, tetap spawn secara bebas
        return GenerateRandomPos();
    }

    // Method untuk generate posisi random
    private Vector3 GenerateRandomPos()
    {
        float x = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
        float y = Random.Range(spawnBounds.min.y, spawnBounds.max.y);
        return new Vector3(x, y, 0);
    }

    // Method untuk mengupdate score.
    private void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{score}";
        }
    }
}