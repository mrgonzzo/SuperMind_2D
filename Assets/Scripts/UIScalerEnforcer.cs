using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScalerEnforcer : MonoBehaviour
{
    // Propiedad estática para el patrón Singleton. Permite acceso global si fuera necesario,
    // aunque aquí se usa principalmente para asegurar unicidad.
    public static UIScalerEnforcer Instance { get; private set; }

    [Header("Settings")]
    // Resolución objetivo de diseño (ej. Full HD). La UI se adaptará basándose en esto.
    [SerializeField] private Vector2 referenceResolution = new Vector2(1920, 1080);
    
    // Define si priorizamos el ancho (0), el alto (1) o un punto medio (0.5) al escalar.
    [Range(0f, 1f)]
    [SerializeField] private float matchWidthOrHeight = 0.5f;

    private void Awake()
    {
        // Implementación del patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            // Crucial: Hace que este objeto sobreviva a la carga de nuevas escenas.
            DontDestroyOnLoad(gameObject);
            // Nos suscribimos al evento de carga de escena para ejecutar la lógica automáticamente.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // Si ya existe uno (ej. volvimos al menú principal), destruimos el duplicado.
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Buena práctica: Desuscribirse de eventos para evitar fugas de memoria (Memory Leaks).
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        // Ejecuta la lógica al iniciar la primera escena donde se coloque el script.
        EnforceCanvasScaling();
    }

    // Este método es llamado automáticamente por el SceneManager.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnforceCanvasScaling();
    }

    public void EnforceCanvasScaling()
    {
        // UNITY 6 OPTIMIZATION:
        // FindObjectsByType es mucho más rápido que el antiguo FindObjectsOfType.
        // Usamos FindObjectsSortMode.None porque no necesitamos ordenarlos, lo que ahorra CPU.
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);

        foreach (var canvas in canvases)
        {
            // Nota: Aquí podrías descomentar el código para ignorar Canvas anidados si usas sub-canvas.
            // if (canvas.transform.parent != null && canvas.transform.parent.GetComponentInParent<Canvas>() != null) continue;

            // Intentamos obtener el componente CanvasScaler
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            
            // Si no existe, lo añadimos dinámicamente. Esto previene errores en Canvas creados rápidamente.
            if (scaler == null)
            {
                scaler = canvas.gameObject.AddComponent<CanvasScaler>();
                Debug.Log($"Added CanvasScaler to {canvas.gameObject.name}");
            }

            // Bandera para optimizar: Solo hacemos Log si realmente cambiamos algo.
            bool changed = false;

            // Verificación 1: Modo de escalado
            if (scaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                changed = true;
            }

            // Verificación 2: Resolución de referencia
            if (scaler.referenceResolution != referenceResolution)
            {
                scaler.referenceResolution = referenceResolution;
                changed = true;
            }

            // Verificación 3: Match Width/Height (con pequeña tolerancia para float)
            if (Mathf.Abs(scaler.matchWidthOrHeight - matchWidthOrHeight) > 0.01f)
            {
                scaler.matchWidthOrHeight = matchWidthOrHeight;
                changed = true;
            }

            if (changed)
            {
                Debug.Log($"Enforced UI Scaling on {canvas.gameObject.name}");
            }
        }
    }
}
