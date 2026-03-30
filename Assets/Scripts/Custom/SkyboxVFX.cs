using UnityEngine;

public class SkyboxVFX : MonoBehaviour
{
    //[SerializeField] private Material 

    [SerializeField] private float rotationSpeed;
    [SerializeField] private AnimationCurve exposure;
    [SerializeField] private float exposureTimeScale;

    private float percentage;

    private void Awake()
    {
        percentage = 0;
    }

    private void FixedUpdate()
    {
        var rotation = Mathf.RoundToInt(percentage * 360);

        RenderSettings.skybox.SetInt("_Rotation", rotation);
        RenderSettings.skybox.SetFloat("_Exposure", exposure.Evaluate(percentage * exposureTimeScale));


        percentage += Time.fixedDeltaTime * rotationSpeed;
        percentage = Mathf.Repeat(percentage, 1f);
    }
}
