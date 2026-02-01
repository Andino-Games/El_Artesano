using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputController input;
    [SerializeField] private PlayerCollisionController collision;

    public Action OnInteractionBegin;
    public Action OnInteractionEnd;

    private void Start()
    {
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

                OnInteractionBegin?.Invoke();
            }
            if (collision.Interactable.Mode == InteractionMode.Hold)
            {
                if (interactionMode == InteractionMode.HoldEnd)
                {
                    collision.Interactable.Stop();

                    OnInteractionEnd?.Invoke();
                }
            }
        }
    }
}
