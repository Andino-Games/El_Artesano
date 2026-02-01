using UnityEngine;
using UnityEngine.Events;

public class ScrewMechanic : MonoBehaviour
{
    public enum InteractionType { MultipleClicks, Hold }

    [Header("Configuración")]
    public InteractionType type; // AQUÍ ELIGES SI ES CLICK O HOLD
    public float rotationSpeed = 200f; // Solo para Hold
    public float jumpForce = 5f; // Fuerza al salir

    [Header("Eventos")]
    public UnityEvent OnRemoved; // Avisa a la pieza (ceja/boca) que este tornillo cayó

    private bool _isRemoved = false;
    private float _holdProgress = 0f;

    // --- Lógica para MULTIPLE CLICKS (Cejas / Mejilla) ---
    public void OnMultipleClickInput()
    {
        if (type != InteractionType.MultipleClicks || _isRemoved) return;
        
        // ¡Al tercer click (manejado por Input System) sale disparado!
        DetachObject();
    }

    // --- Lógica para HOLD (Boca) ---
    public void OnHoldInput(bool isHolding)
    {
        if (type != InteractionType.Hold || _isRemoved) return;

        if (isHolding)
        {
            // Girar visualmente
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            
            // Avanzar progreso (ajusta 0.5f para que tarde más o menos)
            _holdProgress += Time.deltaTime * 0.5f;
            
            // Opcional: Actualizar UI de círculo aquí
            
            if (_holdProgress >= 1f)
            {
                DetachObject();
            }
        }
    }

    private void DetachObject()
    {
        _isRemoved = true;
        Debug.Log("¡Pieza retirada!");

        // Física
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        OnRemoved?.Invoke(); // Avisamos al fragmento padre
    }
}