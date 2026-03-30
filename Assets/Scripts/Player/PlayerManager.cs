using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputController input;
    [SerializeField] private PlayerCollisionController collision;
    [SerializeField] private MovimientoJugador movement;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] new private GameObject light;
    [SerializeField] private FootstepsController footsteps;
    [SerializeField] private PlayerLastScene lastAct;


    private Vector2 moveInput;

    public Action OnInteractionBegin;
    public Action OnInteractionEnd;
    public Action OnTearReached;

    private bool canMove;

    public bool CanMove => canMove;

    private void Awake()
    {
        canMove = false;

        if (footsteps == null)
            footsteps = GetComponentInChildren<FootstepsController>();

        if (footsteps != null)
            footsteps.enabled = false;

        collision.OnPlayerFell += RestartPosition;
        input.OnClickInteraction += ValidateInteraction;
        lastAct.OnTargetReached += TearReached;
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
                collision.Interactable.OnRemoved.AddListener(StopInteraction);
                
                collision.Interactable.Activate();
                collision.Interactable.OnRemoved.AddListener(StopInteraction);

                movement.Anim.SetBool("IsHolding", true);
                
                // SFX al iniciar interacción (opcional)
                AudioManager.PlaySound(SoundType.Mechanical, 0.8f);
                
                // Sin pasos mientras interactúa
                if (footsteps != null)
                    footsteps.enabled = false;

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
        
        // Rehabilita pasos si el player puede moverse
        if (footsteps != null)
            footsteps.enabled = canMove;

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

        if (footsteps != null)
            footsteps.enabled = newActive;
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    public void TriggerLastScene()
    {
        movement.ControllerSetActive(false, true);
        lastAct.Trigger();
    }

    private void TearReached()
    {
        movement.ControllerSetActive(false, false);
        lastAct.StartFalling();

        OnTearReached?.Invoke();
    }
}
