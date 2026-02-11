using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private const float VIGNETTE_MAX_COUNT = 20;
    private const float VIGNETTE_INITIAL_INTENSITY = 0.35f;

    [SerializeField] private CameraZoomController zoom;

    [Header("Vignette")]
    [SerializeField] private CameraVignete vignette;
    [SerializeField] private float duration;

    public void StartVignette()
    {
        StartCoroutine(nameof(VignetteCoroutine));
    }

    public void SetNearView()
    {
        zoom.SetCamera(0);
    }

    public void SetMainView()
    {
        zoom.SetCamera(1);
    }

    public void SetFarView()
    {
        zoom.SetCamera(2);
    }

    public void SetGeneralView()
    {
        zoom.SetCamera(3);
    }

    IEnumerator VignetteCoroutine()
    {
        int counter = 0;
        float delta = (1f - VIGNETTE_INITIAL_INTENSITY) / VIGNETTE_MAX_COUNT;

        while (counter < VIGNETTE_MAX_COUNT)
        {
            vignette.UpdatePercentage((delta * counter) + VIGNETTE_INITIAL_INTENSITY);

            yield return new WaitForSeconds(duration/VIGNETTE_MAX_COUNT);

            counter++;
        }
    }
}
