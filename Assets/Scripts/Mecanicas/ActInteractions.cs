using System;
using UnityEngine;

[Serializable]
public struct ActInteractions
{
    [SerializeField] private MaskFragment fragment;
    [SerializeField] private InteractableObject[] interactables;

    public MaskFragment MaskPiece => fragment;
    public InteractableObject[] Interactables => interactables;

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
