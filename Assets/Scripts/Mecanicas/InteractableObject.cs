using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material outlineMaterial;

    [SerializeField] private InteractionMode interactionMode;
    private Material _originalMaterial;

    [Header("Eventos")]
    public UnityEvent OnRemoved; // Avisa a la pieza (ceja/boca) que este tornillo cayó


    public bool isComplete;

    public InteractionMode Mode => interactionMode;

    private void Start()
    {
        _originalMaterial = meshRenderer.material;
    }

    public void ToggleOutline(bool show)
    {
        meshRenderer.enabled = show ? outlineMaterial : _originalMaterial; 
    }

    public abstract void Activate();
    public abstract void Stop();
    public abstract void Completed();
}
