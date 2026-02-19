using UnityEngine;

public class SceneMusicSetter : MonoBehaviour
{
    [SerializeField] private LevelMusic music = LevelMusic.Level_Gameplay;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Start()
    {
        AudioManager.PlayMusic(music, fadeDuration);
    }
}

