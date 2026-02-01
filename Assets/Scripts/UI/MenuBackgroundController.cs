using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[RequireComponent(typeof(VideoPlayer))]
public class MenuBackgroundController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("La RawImage donde se mostrará el video")]
    public RawImage targetImage;

    private VideoPlayer _videoPlayer;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        
        // Configuración defensiva: asegurar que el video no arranque solo
        _videoPlayer.playOnAwake = false;
        _videoPlayer.isLooping = true;
        
        // Opcional: Desactivar la imagen hasta que el video esté listo para evitar pantallazos negros
        if(targetImage != null) targetImage.enabled = false;

        _videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    private void OnEnable()
    {
        // "Memento Mori": Recordar que este objeto ha "nacido" (se activó el menú)
        // Iniciamos la preparación asíncrona.
        _videoPlayer.Prepare();
    }

    private void OnDisable()
    {
        // Si cerramos el menú, paramos el video inmediatamente.
        _videoPlayer.Stop();
        if(targetImage != null) targetImage.enabled = false;
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        // Solo cuando el video está listo en memoria, lo reproducimos y mostramos la imagen.
        source.Play();
        if(targetImage != null) targetImage.enabled = true;
    }
    
    // Si destruimos el objeto, liberamos la memoria de la Render Texture (Limpieza)
    private void OnDestroy()
    {
        if (_videoPlayer.targetTexture != null)
        {
            _videoPlayer.targetTexture.Release();
        }
    }
}