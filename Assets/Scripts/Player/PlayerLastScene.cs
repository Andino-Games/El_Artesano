using UnityEngine;
using System;

public class PlayerLastScene : MonoBehaviour
{
    const float TARGET_DISTANCE_THRESHOLD = 1f;

    [SerializeField] private Transform target;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float fallSpeed;

    private bool isTriggered;
    private bool isFalling;

    public Action OnTargetReached;

    private void Awake()
    {
        isFalling = false;
        isTriggered = false;
    }

    private void FixedUpdate()
    {
        if (isFalling == true)
        {
            Vector3 downwards = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, downwards, fallSpeed * Time.fixedDeltaTime);

            return;
        }

        if (isTriggered == false)
        {
            return;
        }

        Vector3 direccion = target.position - transform.position;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.fixedDeltaTime);

        if (direccion.magnitude <= TARGET_DISTANCE_THRESHOLD)
        {
            OnTargetReached?.Invoke();

            isTriggered = false;
        }
    }

    public void Trigger()
    {
        isTriggered = true;
    }

    public void StartFalling()
    {
        isFalling = true;
    }
}
