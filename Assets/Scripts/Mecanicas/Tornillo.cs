using System.Collections;
using UnityEngine;

public class Tornillo : InteractableObject
{
    [SerializeField] private float maxHeight;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float duration;

    private float Percentage => timer / duration;
    private float timer;
    Vector3 initialPosition;

    public bool isBeingHold;


    private void OnEnable()
    {
        timer = 0f;
        isBeingHold = false;
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if(isBeingHold == true)
        {
            if (timer < duration)
            {
                Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + maxHeight, initialPosition.z);

                transform.position = Vector3.Lerp(initialPosition, targetPosition, Percentage);
                transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime);

                timer += Time.fixedDeltaTime;
            }
        }
    }

    public override void Activate()
    {
        isBeingHold = true;
    }

    public override void Stop()
    {
        isBeingHold = false;
    }
}
