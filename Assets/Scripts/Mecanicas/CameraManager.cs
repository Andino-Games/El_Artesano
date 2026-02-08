using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private const float DURATION_FRACTION = 20;
    private const float INITIAL_INTENSITY = 0.2f;

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
        float delta = (1f - INITIAL_INTENSITY) / DURATION_FRACTION;

        while (counter < DURATION_FRACTION)
        {
            vignette.UpdatePercentage((delta * counter) + 0.2f);

            yield return new WaitForSeconds(duration/DURATION_FRACTION);

            counter++;
        }
    }
}
