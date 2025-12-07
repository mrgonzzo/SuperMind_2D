using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private int targetWidth = 1920;
    [SerializeField] private int targetHeight = 1080;
    // [SerializeField] private bool fullScreen = true;

    void Start()
    {
        SetResolution();
    }

    public void SetResolution()
    {
        // Si estamos en standalone o editor, intentamos ajustar la resolución
#if UNITY_STANDALONE || UNITY_EDITOR
        // Opción 1: Forzar una resolución específica
        // Screen.SetResolution(targetWidth, targetHeight, fullScreen);

        // Opción 2: Usar la resolución nativa del monitor pero asegurar pantalla completa
        // Esto suele ser mejor para evitar distorsiones si el monitor no es 16:9
        Resolution currentResolution = Screen.currentResolution;

        // Si queremos asegurar 1920x1080 como mínimo o base, podemos hacer lógica aquí.
        // Por ahora, vamos a respetar la resolución nativa en pantalla completa,
        // que es lo que suele verse "mejor" en cualquier monitor moderno.

        /* if (fullScreen && !Screen.fullScreen)
         {
             Screen.fullScreen = true;
         }

         Debug.Log($"Resolution set to: {Screen.width}x{Screen.height}, Fullscreen: {Screen.fullScreen}");*/
     #endif
    }

    // Método para cambiar modo ventana/pantalla completa en runtime si se desea
    /*  public void ToggleFullScreen()
      {
          Screen.fullScreen = !Screen.fullScreen;
      }*/
}