using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMenu : MonoBehaviour
{
    [Header("Navegación (Lista de Escenas)")]
    [Tooltip("Añade aquí los índices de tus escenas. El orden lo decides tú.")]
    public int[] sceneIndices;

    [Header("Control de Sonido")]
    public Image soundButtonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool isMuted = false;

    // --- NAVEGACIÓN FLEXIBLE ---

    /// <summary>
    /// Carga una escena basada en su posición dentro del array 'sceneIndices'.
    /// </summary>
    /// <param name="listIndex">El índice dentro de la lista (0, 1, 2...)</param>
    public void LoadSceneFromList(int listIndex)
    {
        if (listIndex >= 0 && listIndex < sceneIndices.Length)
        {
            SceneManager.LoadScene(sceneIndices[listIndex]);
        }
        else
        {
            Debug.LogWarning("El índice " + listIndex + " no existe en la lista de escenas.");
        }
    }

    // --- LÓGICA DE SONIDO ---

    public void ToggleSound()
    {
        isMuted = !isMuted;

        if (soundButtonImage != null)
        {
            soundButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }

        AudioListener.pause = isMuted;
        Debug.Log("Mute: " + isMuted);
    }
}