using UnityEngine;
using System;

/// <summary>
/// Clase que detecta clics en el juego y lanza eventos.
/// También reproduce un sonido cuando se hace clic en un objeto válido.
/// </summary>
public class MouseInput2DHandler : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayers;

    // Clip de sonido para reproducir al hacer clic
    [SerializeField] private AudioClip clickSound;

    // Componente AudioSource para reproducir sonidos
    private AudioSource audioSource;

    public event Action<Color, int> OnColorPinClicked;
    public event Action<CodePin> OnCodePinClicked;

    void Awake()
    {
        // Busca el componente AudioSource en este mismo GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("MouseInput2DHandler: No se encontró AudioSource en el GameObject.");
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, clickableLayers);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                // Si es un ColorPin
                if (clickedObject.TryGetComponent<ColorPin>(out var colorPin))
                {
                    OnColorPinClicked?.Invoke(colorPin.PinColor, colorPin.ColorCode);
                    PlayClickSound(); // 🔊 sonido
                }
                // Si es un CodePin
                else if (clickedObject.TryGetComponent<CodePin>(out var codePin))
                {
                    OnCodePinClicked?.Invoke(codePin);
                    PlayClickSound(); // 🔊 sonido
                }
                else
                {
                    Debug.LogError("Objeto clicado no es ColorPin ni CodePin");
                }
            }
        }
    }

    /// <summary>
    /// Reproduce el sonido de clic si está asignado.
    /// </summary>
    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
