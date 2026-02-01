using UnityEngine;
using System.Collections.Generic;

public class MaskFragment : MonoBehaviour
{
    [Header("Configuración de la Pieza")]
    public ScrewPuzzle[] myScrews; // ARRASTRA AQUÍ LOS TORNILLOS QUE SOSTIENEN ESTA PIEZA
    public float fallForce = 2f;
    
    [Header("Referencias")]
    private Rigidbody _rb;
    private int _activeScrews;

    void Start()
    {
        
        
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) _rb = gameObject.AddComponent<Rigidbody>();

        // Al inicio, la pieza flota (Kinematic) para no caerse
        _rb.isKinematic = true;
        
        _activeScrews = myScrews.Length;

        // Nos suscribimos a CADA tornillo de esta pieza específica
        foreach (var screw in myScrews)
        {
            screw.OnScrewRemoved.AddListener(CheckIntegrity);
        }
    }

    // Se llama cada vez que sacamos un tornillo de ESTA pieza
    void CheckIntegrity()
    {
        _activeScrews--;

        // Si ya no quedan tornillos sosteniendo ESTA pieza...
        if (_activeScrews <= 0)
        {
            DetachPiece();
        }
        
    }

    public void DetachPiece()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) _rb = gameObject.AddComponent<Rigidbody>();
        
        Debug.Log($"DETACH PIECE {name}");

        // 1. Activar física
        _rb.isKinematic = false; 
        _rb.useGravity = true;

        // 2. Empujoncito hacia el frente para que se desprenda bonito (como en el video)
        _rb.AddForce(transform.forward * fallForce, ForceMode.Impulse);

        // 3. Rotación leve para dramatismo
        Vector3 random = Random.insideUnitSphere;
        Vector3 randomFixed = new Vector3(1, 0, 1);
        _rb.AddTorque(randomFixed * fallForce);

        // Opcional: Destruir después de 5 seg para limpiar memoria
        Destroy(gameObject, 5f); 
    }
}
