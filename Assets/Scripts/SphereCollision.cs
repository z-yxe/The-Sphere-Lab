using UnityEngine;

// Script untuk mendeteksi tabrakan sphere dengan collectible
public class SphereCollision : MonoBehaviour
{
    [Tooltip("Refrence GameObject yang memiliki script CollectibleManager.")]
    [SerializeField] private CollectibleManager manager;

    // Validasi reference manager di awal
    void Start()
    {
        if (manager == null)
        {
            Debug.LogError("Refrence CollectibleManager belum di-asign", this);
        }
    }

    // Deteksi trigger dengan collectible, panggil event ke manager
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            if (manager != null)
            {
                manager.OnCollectibleHit(other.gameObject);
            }
        }
    }
}