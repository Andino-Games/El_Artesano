using System;
using UnityEngine;

[Serializable]
public struct ActInteractions
{
    [SerializeField] private MaskFragment fragment;
    [SerializeField] private GameObject gameZone;
    [SerializeField] private InteractableObject[] interactables;

    public MaskFragment MaskPiece => fragment;
    public GameObject GameZone => gameZone;
    public InteractableObject[] Interactables => interactables;

    public bool ValidateAct()
    {
        bool result = true;

        if (interactables.Length > 0)
        {
            for(int i = 0; i < interactables.Length; i++)
            {
                if (interactables[i].isComplete == false)
                {
                    result = false;
                    break;
                }
            }
        }
        else 
        {
            result = false;
        }

        return result;
    }
}
