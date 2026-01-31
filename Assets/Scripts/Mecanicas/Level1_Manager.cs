using UnityEngine;

public class Level1_Manager : MonoBehaviour
{
    [Header("Objetivos")]
    public ScrewPuzzle[] screws; // ARRASTRA AQUÍ LOS 3 TORNILLOS DEL ESCENARIO
    public GameObject smilePiece; // La pieza de la máscara que caerá
    
    [Header("Configuración")]
    public int screwsNeeded = 3; // Confirmamos que son 3
    private int _screwsRemovedCount = 0;

    void Start()
    {
        // Nos conectamos a cada tornillo
        foreach (var screw in screws)
        {
            screw.OnScrewRemoved.AddListener(CountScrew);
        }
    }

    void CountScrew()
    {
        _screwsRemovedCount++;
        
        Debug.Log($"Progreso: {_screwsRemovedCount}/{screwsNeeded}");

        if (_screwsRemovedCount >= screwsNeeded)
        {
            ReleaseMaskPiece();
        }
    }

    void ReleaseMaskPiece()
    {
        Debug.Log("¡SECCIÓN COMPLETADA! La máscara cambia.");

        // Lógica para que caiga la pieza de la sonrisa/máscara
        if (smilePiece != null)
        {
            Rigidbody rb = smilePiece.GetComponent<Rigidbody>();
            if (rb == null) rb = smilePiece.AddComponent<Rigidbody>();
            
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.down * 2f, ForceMode.Impulse); // Empujoncito hacia abajo
            
            // Opcional: Destruir el objeto después de unos segundos para limpiar
            Destroy(smilePiece, 5f);
        }

        // AQUÍ: Activar transición al siguiente paso de la narrativa o nivel
    }
}