using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public InteractableObject Interactable;

    private void OnTriggerEnter(Collider puzzle)
    {
        // Check if the object we bumped into is interactable
        if (puzzle.TryGetComponent<InteractableObject>(out var interactable))
        {
            interactable.ToggleOutline(true);
            Interactable = interactable;
            Debug.Log("Interactable Reached");
        }
    }

    private void OnTriggerExit(Collider puzzle)
    {
        if (puzzle.TryGetComponent<InteractableObject>(out var interactable))
        {
            interactable.ToggleOutline(false);
            Interactable = null;
            Debug.Log("Interactable Farther");
        }
    }
}
