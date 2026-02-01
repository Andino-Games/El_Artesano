using UnityEngine;

public class Level1_Manager : MonoBehaviour
{
    [SerializeField] private PlayerManager player;
    [SerializeField] private CameraZoomController zoomController;

    [Header("Acts")]
    [SerializeField] private ActInteractions[] acts;
    
    private int currentAct;

    private void Awake()
    {
        player.OnInteractionBegin += InteractionBegin;
        player.OnInteractionEnd += InteractionEnd;
    }

    void Start()
    {
        // Nos conectamos a cada tornillo
        for (int actIndex = 0; actIndex < acts.Length; actIndex++)
        {
            for (int i = 0; i < acts[actIndex].Interactables.Length; i++)
            {
                acts[actIndex].Interactables[i].OnRemoved.AddListener(CountScrew);
            }
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
            acts[currentAct].MaskPiece.DetachPiece();

            currentAct++;

            if (currentAct < acts.Length)
            {
                if (acts[currentAct].GameZone != null)
                {
                    acts[currentAct].GameZone.gameObject.SetActive(true);
                }
            }
        }
    }

    private void TurOffAllGameZones()
    {
        for (int i = 0; i < acts.Length; i++)
        {
            acts[i].GameZone.gameObject.SetActive(false);
        }
    }

    private void InteractionBegin()
    {
        zoomController.SetCamera(0);
    }

    private void InteractionEnd()
    {
        zoomController.SetCamera(1);
    }
}