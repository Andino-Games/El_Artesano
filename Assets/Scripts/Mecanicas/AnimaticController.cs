using UnityEngine.UI;
using System;
using UnityEngine;

public class AnimaticController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject button;
    [SerializeField] private string[] dialogues;

    public Action OnAnimaticEnd;

    public void Animatic1()
    {
        Pause();
        DialogueManager.instance.StartDialogue(dialogues[0], false);

        /*
        var image = animator.transform.parent.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 1);
        */
    }

    public void Animatic2()
    {
        Pause();
        DialogueManager.instance.StartDialogue(dialogues[1], false);
    }

    public void Animatic3()
    {
        Pause();
        DialogueManager.instance.StartDialogue(dialogues[2], false);
    }

    public void Animatic4()
    {
        Pause();
        DialogueManager.instance.StartDialogue(dialogues[3], false);

        /*
        var image = animator.transform.parent.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
        */
    }

    public void Animatic5()
    {
        Pause();
        DialogueManager.instance.StartDialogue(dialogues[4], false);
    }

    public void Animatic6()
    {
        Pause();
        DialogueManager.instance.StartDialogue(dialogues[5], false);
    }

    public void Pause()
    {
        animator.speed = 0f;
        button.SetActive(true);
    }

    public void Resume()
    {
        animator.speed = 1f;
        button.SetActive(false);

        DialogueManager.instance.EndDialogue();
    }

    public void End()
    {
        OnAnimaticEnd?.Invoke();
    }

    public void Skip()
    {
        Resume();
        //animator.SetTrigger("Intro");
        animator.Play("Animatic_Intro", 0, 0.9f);
    }

    public void PlayIntro()
    {
        animator.SetTrigger("Intro");
    }

    public void PlayOutro()
    {
        animator.SetTrigger("Outro");
    }
}
