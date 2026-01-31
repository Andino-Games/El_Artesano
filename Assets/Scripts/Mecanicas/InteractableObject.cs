using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private Color _color;
    private bool _isInteractable;

    private void Start()
    {
        _color = Color.white;
        _isInteractable = true;
    }

    public void ToggleOutline(bool show, Color color)
    {
        if (show && _isInteractable)
        {
            meshRenderer.material.color = color;
        }
        else { 
            meshRenderer.material.color = Color.white;
        }

    }

    public void IsResolved()
    {
        if (_isInteractable)
        {
            _isInteractable = false;
            meshRenderer.material.color = Color.white;
        }
    }
}
