using UnityEngine;

public class Level1_Manager : MonoBehaviour
{
    [SerializeField] private ActInteractions[] acts;

    [Header("Objetivos")]
    public Tornillo[] screws; // ARRASTRA AQUÍ LOS 3 TORNILLOS DEL ESCENARIO
    public GameObject smilePiece; // La pieza de la máscara que caerá
    
    [Header("Configuración")]
    public int screwsNeeded = 3; // Confirmamos que son 3
    private int _screwsRemovedCount = 0;


    private int currentAct;

    void Start()
    {
        // Nos conectamos a cada tornillo
        foreach (var screw in screws)
        {
            screw.OnRemoved.AddListener(CountScrew);
        }
    }

    void CountScrew()
    {
        if(currentAct >= acts.Length)
        {
            return;
        }

        bool isActComplete = acts[currentAct].ValidateAct();

        if(isActComplete == true)
        {
            Debug.Log("Try to change Mask");
            ReleaseMaskPiece(acts[currentAct].MaskPiece);
            currentAct++;
        }
    }

    void CountScrew(string sobrecarga)
    {
        _screwsRemovedCount++;
        
        Debug.Log($"Progreso: {_screwsRemovedCount}/{screwsNeeded}");

        if (_screwsRemovedCount >= screwsNeeded)
        {
            //ReleaseMaskPiece();
        }
    }

    void ReleaseMaskPiece(GameObject maskPiece)
    {
        Debug.Log("¡SECCIÓN COMPLETADA! La máscara cambia.");

        // Lógica para que caiga la pieza de la sonrisa/máscara
        if (maskPiece != null)
        {
            Rigidbody rb = maskPiece.GetComponent<Rigidbody>();
            if (rb == null) rb = maskPiece.AddComponent<Rigidbody>();
            
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.down * 2f, ForceMode.Impulse); // Empujoncito hacia abajo
            
            // Opcional: Destruir el objeto después de unos segundos para limpiar
            Destroy(maskPiece, 5f);
        }

        // AQUÍ: Activar transición al siguiente paso de la narrativa o nivel
    }
}