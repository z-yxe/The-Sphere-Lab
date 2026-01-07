using UnityEngine;
using UnityEngine.EventSystems;

// Script untuk menangani efek suara pada UI button
public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    // Dipanggil otomatis saat kursor masuk ke area button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayHoverSound();
        }
    }

    // Method untuk memutar suara klik button, dipanggil melalui event onClick di Unity Inspector
    public void PlayClickSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayClickSound();
        }
    }
}