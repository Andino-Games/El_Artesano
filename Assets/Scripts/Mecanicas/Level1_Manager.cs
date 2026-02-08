using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Level1_Manager : MonoBehaviour
{
    private const float TIME_TO_RESET = 5f;

    [SerializeField] private PlayerManager player;
    [SerializeField] new private CameraManager camera;
    [SerializeField] private MapController map;
    [SerializeField] private AnimaticController animatic;

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
        animatic.OnAnimaticEnd += StartGame;

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
        if (currentAct >= acts.Length)
        {
            return;
        }

        bool isActComplete = acts[currentAct].ValidateAct();

        if (isActComplete == true)
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
                map.SetActMap(currentAct);

                /*
                if (acts[currentAct].GameZone != null)
                {
                    acts[currentAct].GameZone.gameObject.SetActive(true);
                }
                */
            }
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

    public void StartGame()
    {
        animatic.gameObject.SetActive(false);

        StartCoroutine(nameof(StartLevelCoroutine));
    }

    IEnumerator StartLevelCoroutine()
    {
        camera.SetGeneralView();

        yield return new WaitForSeconds(1f);

        camera.SetMainView();

        yield return new WaitForSeconds(2f);

        player.SetActive(true);
        camera.StartVignette();
    }
}