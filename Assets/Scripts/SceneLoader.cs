using UnityEngine;
using UnityEngine.SceneManagement;

// Script utama yang mengatur mulai dai perpindahan scene, restart, hingga exit game
public class SceneLoader : MonoBehaviour
{
    // Validasi scene
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene belum diisi!");
        }
    }

    // Untuk restart scene jika ada error
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Konfigurasi untuk tombol exit atau quit
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}