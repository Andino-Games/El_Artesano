using GLTFast.Schema;
using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputController input;
    [SerializeField] private PlayerCollisionController collision;
    [SerializeField] private MovimientoJugador movement;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] new private GameObject light;

    private Vector2 moveInput;

    public Action OnInteractionBegin;
    public Action OnInteractionEnd;

    private bool canMove;

    private void Awake()
    {
        canMove = false;
        
        collision.OnPlayerFell += RestartPosition;
        input.OnClickInteraction += ValidateInteraction;
    }

    private void Update()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (canMove)
        {
            movement.Move(direction);
        }

        movement.AplicarGravedad();
    }

    // Recibe el input del componente Player Input
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    //  Make sure that you will interact with an object only with its interaction mode
    private void ValidateInteraction(InteractionMode interactionMode)
    {
        if(collision.Interactable != null)
        {
            if (collision.Interactable.Mode == interactionMode)
            {
                collision.Interactable.Activate();
                collision.Interactable.OnRemoved.AddListener(StopInteraction);

                movement.Anim.SetBool("IsHolding", true);

                OnInteractionBegin?.Invoke();
            }
            if (collision.Interactable.Mode == InteractionMode.Hold)
            {
                if (interactionMode == InteractionMode.HoldEnd)
                {
                    StopInteraction();
                }
            }
        }
    }

    private void StopInteraction()
    {
        collision.Interactable.Stop();

        movement.Anim.SetBool("IsHolding", false);

        OnInteractionEnd?.Invoke();
    }

    public void RestartPosition()
    {
        Debug.Log("Restart Position");

        movement.TeleportTo(spawnPoint.position);
    }

    public void SetActive(bool newActive)
    {
        canMove = newActive;
        light.SetActive(newActive);
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
