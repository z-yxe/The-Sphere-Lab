using UnityEngine;

// Script untuk membuat objek bergerak konstan ke arah tertentu sejak awal
public class ConstantMovement : MonoBehaviour
{
    [Tooltip("Kecepatan konstan pergerakan objek.")]
    public float speed = 8f;
    [Tooltip("Arah awal pergerakan objek. Akan dinormalisasi secara otomatis nanti.")]
    public Vector3 direction = new Vector3(1f, 1f, 0);

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Bergerak secara konstan (linearVelocity karena saya pakai Unity 6)
    void Start()
    {
        Vector3 moveDirection = direction.normalized;
        rb.linearVelocity = moveDirection * speed;
    }
}