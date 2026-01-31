using UnityEngine;
using UnityEngine.UI;

public class CircularProgressUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public Image progressFillImage; // La imagen circular (tipo Filled)
    public GameObject uiContainer;  // El objeto padre (para ocultarlo/mostrarlo)

    [Header("Configuración")]
    public Gradient colorGradient; // Para que cambie de rojo a verde mientras se llena

    private void Start()
    {
        // Al iniciar, escondemos el indicador
        if(uiContainer != null) uiContainer.SetActive(false);
    }

    // Llama a esta función desde el script de 'ScrewPuzzle' o 'InteractionManager'
    public void UpdateProgress(float current, float max)
    {
        if (uiContainer != null && !uiContainer.activeSelf) 
            uiContainer.SetActive(true);

        float percentage = current / max;
        
        if (progressFillImage != null)
        {
            progressFillImage.fillAmount = percentage;
            progressFillImage.color = colorGradient.Evaluate(percentage);
        }

        // Si se completa, ocultamos la UI
        if (percentage >= 1f || percentage <= 0f)
        {
            // Invoke("HideUI", 0.5f); // Opcional: retardo para ocultar
            if(percentage <= 0f) HideUI();
        }
    }

    public void HideUI()
    {
        if(uiContainer != null) uiContainer.SetActive(false);
    }
}