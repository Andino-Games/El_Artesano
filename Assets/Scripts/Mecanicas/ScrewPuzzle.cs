using UnityEngine;
using UnityEngine.Events;

public class ScrewPuzzle : MonoBehaviour
{
    [Header("Configuración")]
    public float jumpForce = 5f; // Fuerza con la que sale disparado el tornillo
    
    private bool _isRemoved = false;

    [Header("Eventos")]
    public UnityEvent OnScrewRemoved; // Avisa al Level Manager

    // Esta función la llama el InteractionManager al detectar los 3 clicks
    public void TriggerUnscrew()
    {
        if (_isRemoved) return; // Si ya salió, no hacemos nada
        
        _isRemoved = true;
        
        // 1. Efecto físico: Habilitar gravedad y empujar hacia afuera
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        
        rb.isKinematic = false;
        rb.useGravity = true;
        // Empuje en la dirección "hacia afuera" (normalmente -Forward o Up local, ajusta según tu modelo)
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); 
        
        // 2. Feedback visual opcional (ej. cambiar color o partículas)
        // GetComponent<MeshRenderer>().material.color = Color.grey;

        // 3. Avisar al Manager que este tornillo cayó
        OnScrewRemoved?.Invoke();
        
        Debug.Log("¡Tornillo fuera!");
    }
}