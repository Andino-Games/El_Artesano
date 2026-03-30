using UnityEngine;
using UnityEngine.SceneManagement;

public class EndEvent : MonoBehaviour
{
    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }
}
