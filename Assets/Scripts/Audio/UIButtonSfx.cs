using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSfx : MonoBehaviour
{
    [SerializeField] private SoundType sound = SoundType.UI_Click;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.PlaySound(sound, volume);
        });
    }
}