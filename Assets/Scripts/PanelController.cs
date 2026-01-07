using UnityEngine;

// Script untuk mengatur buka/tutup panel UI (About/Close)
public class PanelController : MonoBehaviour
{
    public GameObject panel;

    // Fungsi untuk membuka panel, akan dipanggil oleh AboutButton
    public void OpenPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    // Fungsi untuk menutup panel, akan dipanggil oleh CloseButton
    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}