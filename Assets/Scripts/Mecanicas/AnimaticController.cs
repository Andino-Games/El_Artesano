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
        DialogueManager.instance.StartDialogue(dialogues[0]);
    }

    public void Animatic2()
    {
        Pause();
        DialogueManager.instance.StartDialogue(dialogues[1]);
    }

    public void Animatic3()
    {
        Pause();
        DialogueManager.instance.StartDialogue(dialogues[2]);
    }

    public void Animatic4()
    {
        Pause();
        DialogueManager.instance.StartDialogue(dialogues[3]);
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
    }

    public void End()
    {
        OnAnimaticEnd?.Invoke();
    }

    public void Skip()
    {
        animator.Play("Animatic_Clip", 0, 0.9f);
    }
}
