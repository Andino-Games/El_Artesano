using UnityEngine;
using System;
using System.Xml.Serialization;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputController input;
    [SerializeField] private PlayerCollisionController collision;
    [SerializeField] private MovimientoJugador movement;
    [SerializeField] private Transform spawnPoint;

    public Action OnInteractionBegin;
    public Action OnInteractionEnd;

    private void Start()
    {
        collision.OnPlayerFell += RestartPosition;
        input.OnClickInteraction += ValidateInteraction;
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
}
