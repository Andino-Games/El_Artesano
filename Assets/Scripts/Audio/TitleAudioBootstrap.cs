using UnityEngine;

public class TitleAudioBootstrap : MonoBehaviour
{
    private void Start()
    {
        AudioManager.PlayMusic(LevelMusic.MainMenu, 0.25f);
    }
}