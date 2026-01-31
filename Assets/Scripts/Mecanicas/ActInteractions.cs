using System;
using UnityEngine;

[Serializable]
public struct ActInteractions
{
    [SerializeField] private GameObject maskPiece;
    [SerializeField] private InteractableObject[] interactables;

    public GameObject MaskPiece => maskPiece;

    public bool ValidateAct()
    {
        bool result = true;

        for(int i = 0; i < interactables.Length; i++)
        {
            if (interactables[i].isComplete == false)
            {
                result = false;
                break;
            }
        }

        return result;
    }
}
