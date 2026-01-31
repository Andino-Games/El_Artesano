using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputController input;
    [SerializeField] private PlayerCollisionController collision;

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
            }            
        }
    }
}
