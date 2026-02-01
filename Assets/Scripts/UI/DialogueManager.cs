using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private GameObject _gradientSprite;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private CanvasGroup _gradientOpacity;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _gradientOpacity = _gradientSprite.GetComponent<CanvasGroup>();
    }

    public void StartDialogue(string text)
    {
        _gradientOpacity.alpha = Mathf.Lerp(0f, 1f, 1.2f);
        _dialogueText.text = text;
    }

    public void EndDialogue()
    {
        _gradientOpacity.alpha = Mathf.Lerp(1f, 0f, 1.2f);
        _dialogueText.text = "";
    }

}
