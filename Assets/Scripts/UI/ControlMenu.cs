using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMenu : MonoBehaviour
{
    [Header("Navegación (Build Indices)")]
    public int playSceneIndex = 1;
    public int creditsSceneIndex = 2;

    [Header("Control de Sonido")]
    public Image soundButtonImage; // Arrastra el componente Image del botón de sonido
    public Sprite soundOnSprite;   // Icono de sonido activo
    public Sprite soundOffSprite;  // Icono de sonido tachado/mute

    private bool isMuted = false;

    // --- NAVEGACIÓN ---

    public void PlayGame()
    {
        SceneManager.LoadScene(playSceneIndex);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene(creditsSceneIndex);
    }

    // --- LÓGICA DE SONIDO (Lista para conectar) ---

    public void ToggleSound()
    {
        // 1. Cambiamos el estado
        isMuted = !isMuted;

        // 2. Feedback Visual
        if (soundButtonImage != null)
        {
            soundButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }

        // 3. Conexión al Motor de Audio
        // Usamos AudioListener.pause para que funcione desde ya,
        // pero aquí es donde conectarías tu futuro AudioManager.
        AudioListener.pause = isMuted;

        Debug.Log("Estado del Mute: " + isMuted);
    }
}