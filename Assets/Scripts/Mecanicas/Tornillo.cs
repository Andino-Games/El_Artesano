using System;
using System.Collections;
using UnityEngine;

public class Tornillo : InteractableObject
{
    [SerializeField] private float maxHeight;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float duration;
    [SerializeField] private float jumpForce;

    private float Percentage => timer / duration;
    private float timer;
    Vector3 initialPosition;

    public bool isBeingHold;


    private void OnEnable()
    {
        timer = 0f;
        isComplete = false;
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
            else
            {
                if(isComplete == false)
                {
                    isComplete = true;

                    Completed();
                }
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

    public override void Completed()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        float random = UnityEngine.Random.Range(-2f, 2f);
        Vector3 jumpVector = new Vector3(random, 1f) * jumpForce;

        rb.AddForce(jumpVector, ForceMode.Impulse);
        rb.AddTorque(UnityEngine.Random.insideUnitSphere * jumpForce * 3f);

        Destroy(gameObject, 3f);

        OnRemoved?.Invoke();
    }
}

