using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Editor variables
    [Header("Camera variables")]
    [Tooltip("The background object that will always be fit to screen orientation and size")]
    public GameObject background;

    // Objects
    private DeviceOrientation currentOrientation;

    // Awake is called at initialization of this class
    void Awake()
    {
#if UNITY_ANDROID
        currentOrientation = Input.deviceOrientation;
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        if (background == null)
            Debug.Log("CameraController.Awake(): No background gameobject assigned!");
        else
            AdjustBackgroundSize();  // Set initial background size and position
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        DetectOrientationChange();
#endif
    }

    // TODO: Should be called when changing aspect from/to landscape/portrait, must be an event somewhere.
    private void AdjustBackgroundSize()
    {
        if (background == null)
            return;

        // Get components
        SpriteRenderer backgroundSpriteRenderer = background.GetComponent<SpriteRenderer>();

        // Setup variables
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = backgroundSpriteRenderer.sprite.bounds.size;
        Vector2 backgroundScale = Vector2.one;

        // Determine landscape/portrait and adjust
        backgroundScale.x = cameraSize.x / spriteSize.x;
        backgroundScale.y = cameraSize.y / spriteSize.y;

        // Set to background
        background.transform.localScale = backgroundScale;
    }

    private void DetectOrientationChange()
    {
        if (currentOrientation != Input.deviceOrientation)
            AdjustBackgroundSize();
    }
}
