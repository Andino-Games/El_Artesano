using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public  bool setToNear;
    public bool setToMain;
    public bool setToFar;

    [SerializeField] private CinemachineCamera[] cameras;

    // Update is called once per frame
    void Update()
    {
        if (setToNear == true)
        {
            setToNear = false;

            SetCamera(0);
        }

        if (setToMain == true)
        {
            setToMain = false;

            SetCamera(1);
        }

        if (setToFar == true)
        {
            setToFar = false;

            SetCamera(2);
        }
    }

    private void ResetCamerasPriority()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            Debug.Log("Cámaras apagadas");
            cameras[i].gameObject.SetActive(false);
        }
    }

    public void SetCamera(int cameraIndex)
    {
        if (cameraIndex >= cameras.Length)
        {
            return;
        }

        ResetCamerasPriority();

        Debug.Log("Cámara encendida");
        cameras[cameraIndex].gameObject.SetActive(true);
    }
}
