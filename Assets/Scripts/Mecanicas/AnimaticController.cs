using UnityEngine.UI;
using System;
using UnityEngine;

public class AnimaticController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject button;
    [SerializeField] private string[] dialogues;

    public Action OnAnimaticEnd;
    public Action OnContinueGameplay;

    public void Animatic1()
    {
        Animatic(0);
    }

    public void Animatic2()
    {
        Animatic(1);
    }

    public void Animatic3()
    {
        Animatic(2);
    }

    public void Animatic4()
    {
        Animatic(3);
    }

    public void Animatic5()
    {
        Animatic(4);
    }

    public void Animatic6()
    {
        Animatic(5);
    }

    private void Animatic(int index)
    {
        Pause();
        DialogueManager.instance.StartDialogue(dialogues[index]);
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

    public void EndIntro()
    {
        OnAnimaticEnd?.Invoke();
    }

    public void SkipIntro()
    {
        Resume();
        
        animator.Play("The_Animatic_Intro", 0, 0.9f);
    }

    public void PlayIntro()
    {
        animator.SetTrigger("Intro");
    }

    public void PlayOutro()
    {
        animator.SetTrigger("Outro");
    }

    public void ContinueGameplay()
    {
        OnContinueGameplay?.Invoke();
    }
}
