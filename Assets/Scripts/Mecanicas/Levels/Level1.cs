using System.Collections;
using UnityEngine;

public class Level1 : LevelManager
{
    private void Start()
    {
        StartCoroutine(DialogueCoroutine());
    }

    private IEnumerator DialogueCoroutine()
    {
        yield return new WaitForSeconds(2f);

        DialogueManager.instance.StartDialogue(levelDialogue[0]);

        yield return new WaitForSeconds(2f);

        DialogueManager.instance.EndDialogue();
    }
}
