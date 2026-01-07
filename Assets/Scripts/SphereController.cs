using UnityEngine;

// Script utama untuk mengontrol pergerakan bola (sphere) dengan keyboard atau mouse
public class SphereController : MonoBehaviour
{
    // Enum untuk mode kontrol utama
    public enum ControlMode { Keyboard, FollowMouse }

    [Header("Control Settings")]
    public ControlMode controlMode = ControlMode.Keyboard;

    [Header("Speed Controller")]
    public float speed = 10f;
    [Tooltip("Sesuaikan prefrensi sensitivitas keyboard")]
    public float keyboardSensitivity = 2f;
    [Tooltip("Kekuatan rem pada followMouse untuk mencegah overshoot.")]
    public float mouseDamping = 3f;

    [Header("Required Components")]
    [SerializeField] private Camera mainCamera;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Plane gamePlane;

    // Validasi awal
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Inisialisasi plane untuk tracking posisi mouse.
        gamePlane = new Plane(Vector3.forward, Vector3.zero);
    }

    void Update()
    {
        // Hanya menangani input keyboard di Update karena mouse pakai kursor tidak menggunakan input.
        if (controlMode == ControlMode.Keyboard)
        {
            HandleKeyboardInput();
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // Panggil method gerakan sesuai mode kontrol yang dipilih.
        switch (controlMode)
        {
            case ControlMode.Keyboard:
                MoveWithKeyboard();
                break;
            case ControlMode.FollowMouse:
                MoveWithMouse();
                break;
        }
    }

    // Method baru untuk mengubah mode kontrol dari UI
    public void SetControlMode(int modeIndex)
    {
        controlMode = (ControlMode)modeIndex;
    }

    // Method input untuk keyboard
    private void HandleKeyboardInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        moveInput = new Vector3(horizontalInput, verticalInput, 0f).normalized;
    }

    // Menggerakkan bola dengan keyboard menggunakan force
    private void MoveWithKeyboard()
    {
        rb.AddForce(moveInput * (speed * keyboardSensitivity));
    }

    // Bola mengikuti kursor dengan kombinasi force dan damping
    private void MoveWithMouse()
    {
        // Menggunakan raycast untuk track kursor yang berpotongan dengan plane
        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (gamePlane.Raycast(mouseRay, out float distance))
        {
            Vector3 targetPosition = mouseRay.GetPoint(distance);

            // Kombinasi force dengan damping agar feels lebih natural
            Vector3 drivingForce = (targetPosition - rb.position) * speed;
            Vector3 dampingForce = -rb.linearVelocity * mouseDamping;

            rb.AddForce(drivingForce + dampingForce);
        }
    }
}