using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;


public class CameraVignete : MonoBehaviour
{
    Volume volume;
    Vignette vignette;

    private void Awake()
    {
        volume = GetComponent<Volume>();

        if (volume.profile.TryGet(out vignette))
        {
            Debug.Log("Vignette encontrada");
        }
        else
        {
            Debug.LogError("No se encontró Vignette en el Volume");
        }
    }

    public void UpdatePercentage(float percentage)
    {
        if (vignette != null)
        {
            var a = Mathf.Clamp01(percentage);
            Debug.Log("Update vignette to: " + percentage);
            vignette.intensity.Override(a);        
        }
        else
        {
            Debug.Log("Vignette is NULL");
        }
    }
}
