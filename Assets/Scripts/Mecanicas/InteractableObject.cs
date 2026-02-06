using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private InteractionMode interactionMode;
    
    [Header("Feedback")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material outlineMaterial;

    private Color _color;
    private Material _originalMaterial;

    [Header("Eventos")]
    public UnityEvent OnRemoved; // Avisa a la pieza (ceja/boca) que este tornillo cay�

    private bool _isInteractable;
    public bool isComplete;

    public InteractionMode Mode => interactionMode;

    private void Awake()
    {
        _originalMaterial = meshRenderer.material;
    }

    private void Start()
    {
        _color = Color.white;
        _originalMaterial = meshRenderer.material;
        _isInteractable = true;
    }

    
    public void ToggleOutline(bool show)
    {
        meshRenderer.material = show ? outlineMaterial : _originalMaterial; 
    }
    

   /* public void ToggleOutline(bool show, Material material)
    {
        if (show && _isInteractable)
        {
            meshRenderer.material = material;
        }
        else
        {
            meshRenderer.material = material;
        }

    }*/

    public void IsResolved()
    {
        if (_isInteractable)
        {
            _isInteractable = false;
            meshRenderer.material = _originalMaterial;
        }
    }

    public abstract void Activate();
    public abstract void Stop();
    public abstract void Completed();
}
