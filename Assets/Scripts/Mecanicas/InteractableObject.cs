using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private Color _color;

    private void Start()
    {
        _color = Color.white;
    }

    public void ToggleOutline(bool show, Color color)
    {
        if (show)
        {
            _color = color;
            meshRenderer.material.color = _color;
        }
        else
        {
            _color = color;
            meshRenderer.material.color = _color;
        }
            
    }
}
