using UnityEngine;

public class SpriteTransformer : MonoBehaviour
{
    // References
    private DeviceOrientation currentOrientation;

    // Vector
    private Vector2 originalSpriteScale = Vector2.one;

    // Awake is called at initialization of this class
    public void Awake()
    {
#if UNITY_ANDROID
        currentOrientation = Input.deviceOrientation;
#endif
        originalSpriteScale = this.gameObject.transform.localScale;
    }

    // Start is called before the first frame update
    public void Start()
    {
        AdjustBackgroundSize();

        //Debug.Log("BackgroundController.Start(): localPos: " + this.gameObject.transform.localPosition);
    }

    // Update is called once per frame
    public void Update()
    {
#if UNITY_ANDROID
        DetectOrientationChange();
#endif

#if UNITY_EDITOR
        AdjustBackgroundSize();
#endif
    }

    private void AdjustBackgroundSize()
    {
        // Get components
        SpriteRenderer backgroundSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        // Setup variables
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = backgroundSpriteRenderer.sprite.bounds.size;
        Vector2 backgroundScale = Vector2.one;

        // Determine landscape/portrait and adjust
        backgroundScale.x = cameraSize.x / spriteSize.x;
        backgroundScale.y = cameraSize.y / spriteSize.y;

        // Set to background
        this.gameObject.transform.localScale = backgroundScale;
    }

    private void DetectOrientationChange()
    {
        if (currentOrientation != Input.deviceOrientation)
            AdjustBackgroundSize();
    }
}
