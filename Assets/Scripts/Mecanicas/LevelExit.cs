using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Configuración")]
    public string nextSceneName = "Level2_Anger"; // Nombre de la escena de Ira
    public ParticleSystem glowEffect; // Un brillo azul para llamar la atención

    // Se llama desde el InteractionManager (igual que los tornillos, con 1 click o 3)
    public void EnterNextLevel()
    {
        Debug.Log("Viajando a la Mente de la Ira...");
        
        // Aquí puedes poner una animación de Fade Out antes de cargar
        SceneManager.LoadScene(nextSceneName);
    }
    
    // Si quieres que brille cuando se revela
    void OnBecameVisible() 
    {
        if(glowEffect != null) glowEffect.Play();
    }
}
