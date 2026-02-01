using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private CanvasGroup _gradientOpacity;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _gradientOpacity = _dialoguePanel.GetComponent<CanvasGroup>();
    }

    public void StartDialogue(string text)
    {
        _gradientOpacity.alpha = Mathf.Lerp(0f, 1f, 1.2f);
        _dialogueText.text = text;
        StartCoroutine(DialogueCouroutine(text));
    }

    private IEnumerator DialogueCouroutine(string text)
    {
        yield return new WaitForSeconds(2f);
        
        _dialogueText.text = text;
        
        yield return new WaitForSeconds(5f);
        EndDialogue();
    }

    public void EndDialogue()
    {
        Debug.Log("End Dialogue");
        _gradientOpacity.alpha = Mathf.Lerp(1f, 0f, 1.2f);
        _dialogueText.text = "";
    }

}
