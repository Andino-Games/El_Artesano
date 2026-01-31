using System.Collections;
using UnityEngine;

public class Tornillo : InteractableObject
{
    [SerializeField] private float maxHeight;
    [SerializeField] private float maxActivations;
    [SerializeField] private float duration;

    private float heightStep;

    private int activationsCount;


    private void OnEnable()
    {
        heightStep = 0;
    }

    public override void Activate()
    {
        if (activationsCount == 0)
        {
            heightStep = (maxHeight - transform.position.y) / maxActivations;
        }

        StartCoroutine(nameof(ActivationCorotine));
    }

    IEnumerator ActivationCorotine()
    {
        Vector3 currentPosition = transform.position;

        float timer = 0f;

        while (timer < duration) 
        {
            Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y + heightStep, currentPosition.z);
            timer += Time.deltaTime;
            float percentage = timer / duration;

            transform.position = Vector3.Lerp(transform.position, newPosition, transform.position.z);

            yield return null;
        }

        activationsCount++;
    }
}
