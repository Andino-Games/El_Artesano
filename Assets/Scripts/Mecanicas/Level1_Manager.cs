using UnityEngine;

public class Level1_Manager : MonoBehaviour
{
    private const float TIME_TO_RESET = 5f;

    [SerializeField] private PlayerManager player;
    [SerializeField] private CameraZoomController zoomController;
    [SerializeField] new private CameraManager camera;

    [Header("Acts")]
    [SerializeField] private ActInteractions[] acts;
    
    [Header("Dialogue")]
    [SerializeField] private string[] dialogue;
    
    private int currentAct;
    private bool canChangeZoom;

    private void Awake()
    {
        canChangeZoom = true;

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

        camera.StartVignette();
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
            camera.SetFarView();
            canChangeZoom = false;
            
            DialogueManager.instance.StartDialogue(dialogue[currentAct]);

            Invoke(nameof(ResetCamera), TIME_TO_RESET);

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
        camera.SetNearView();
    }

    private void InteractionEnd()
    {
        if(canChangeZoom == true)
        {
            camera.SetMainView();
        }
    }

    private void ResetCamera()
    {
        canChangeZoom = true;
        InteractionEnd();
    }
}