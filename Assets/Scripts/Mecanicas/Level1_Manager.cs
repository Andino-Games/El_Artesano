using UnityEngine.UI;
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

    [SerializeField] private Transform[] spawnPoints;

    [Header("Acts")]
    [SerializeField] private ActInteractions[] acts;

    [Header("Dialogue")]
    [SerializeField] private string startingDialogue;
    [SerializeField] private string[] dialogue;
    
    private int currentAct;
    private bool canChangeZoom;

    private void Awake()
    {
        canChangeZoom = true;

        player.OnInteractionBegin += InteractionBegin;
        player.OnInteractionEnd += InteractionEnd;
        animatic.OnAnimaticEnd += StartGame;
        animatic.OnContinueGameplay += ResumeGameplay;

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
            acts[currentAct].MaskPiece.DetachPiece();
            
            StopGameplay();

            currentAct++;

            if (currentAct >= acts.Length)
            {
                camera.SetGeneralView();
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

        DialogueManager.instance.StartDialogue(startingDialogue, true);
    }

    IEnumerator EndLevelCoroutine()
    {
        camera.SetGeneralView();

        yield return new WaitForSeconds(7f);

        DialogueManager.instance.EndDialogue();

        animatic.gameObject.SetActive(true);
        animatic.PlayOutro();

        //yield return new WaitForSeconds(1f);
    }

    private void StopGameplay()
    {
        camera.SetFarView();
        canChangeZoom = false;
        player.SetActive(false);

        DialogueManager.instance.StartDialogue(dialogue[currentAct], true);
    }

    private void ResumeGameplay()
    {
        canChangeZoom = true;
        DialogueManager.instance.EndDialogue();

        if (currentAct == 0)
        {
            camera.StartVignette();
        }
        else if (currentAct >= acts.Length)
        {
            Invoke(nameof(StartOutro), 3f);

            return;
        }

        map.SetActMap(currentAct);
        player.SetSpawnPoint(spawnPoints[currentAct]);
        player.SetActive(true);

        InteractionEnd();
    }

    private void StartOutro() 
    {
        animatic.gameObject.SetActive(true);
        animatic.PlayOutro();
    }
}