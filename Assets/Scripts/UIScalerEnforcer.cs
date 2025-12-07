using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScalerEnforcer : MonoBehaviour
{
    public static UIScalerEnforcer Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private Vector2 referenceResolution = new Vector2(1920, 1080);
    [Range(0f, 1f)]
    [SerializeField] private float matchWidthOrHeight = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        EnforceCanvasScaling();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnforceCanvasScaling();
    }

    public void EnforceCanvasScaling()
    {
        // Find all Canvases, not just existing Scalers, so we can add Scalers if missing
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None );

        foreach (var canvas in canvases)
        {
            // Skip if it's a child of another canvas (optional, but usually good for nested UI)
            // if (canvas.transform.parent != null && canvas.transform.parent.GetComponentInParent<Canvas>() != null) continue;

            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler == null)
            {
                scaler = canvas.gameObject.AddComponent<CanvasScaler>();
                Debug.Log($"Added CanvasScaler to {canvas.gameObject.name}");
            }

            bool changed = false;

            if (scaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                changed = true;
            }

            if (scaler.referenceResolution != referenceResolution)
            {
                scaler.referenceResolution = referenceResolution;
                changed = true;
            }

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
