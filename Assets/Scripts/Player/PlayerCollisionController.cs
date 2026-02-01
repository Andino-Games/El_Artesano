using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public InteractableObject interactable;

    public InteractableObject Interactable => interactable;

    private void OnTriggerEnter(Collider puzzle)
    {
        // Check if the object we bumped into is interactable
        if (puzzle.TryGetComponent<InteractableObject>(out var interactable))
        {
            interactable.ToggleOutline(true);
            this.interactable = interactable;
            Debug.Log("Interactable Reached");
        }
    }

    private void OnTriggerExit(Collider puzzle)
    {
        if (puzzle.TryGetComponent<InteractableObject>(out var interactable))
        {
            interactable.ToggleOutline(false);
            this.interactable = null;
            Debug.Log("Interactable Farther");
        }
    }
}
