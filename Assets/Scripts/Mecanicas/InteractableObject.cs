using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material _originalMaterial;

    private void Start()
    {
        _originalMaterial = meshRenderer.material;
    }

    public void ToggleOutline(bool show)
    {
        meshRenderer.enabled = show ? outlineMaterial : _originalMaterial; 
    }
}
