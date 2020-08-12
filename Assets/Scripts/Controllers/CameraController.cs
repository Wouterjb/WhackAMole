using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Editor variables
    [Header("Camera orthographic sizes")]
    public float portraitOrthoSize = 14;
    public float landscapeOrthoSize = 5;

    // References
    private Camera cameraComponent;

    // Bool
    private bool deviceOrientationChanged = false;

    // Numbers
    private float currentScreenWidth = 0.0f;
    private float currentScreenHeight = 0.0f;

    // Awake is called at initialization of this class
    public void Awake()
    {
        // Grab camera component
        cameraComponent = this.gameObject.GetComponent<Camera>();

        // Assign variables
        currentScreenWidth = Screen.width;
        currentScreenHeight = Screen.height;
    }

    // Start is called before the first frame update
    public void Start()
    {
        // Adjust orthographic size at the start
        AdjustCameraOrthographicSize();

        // Listen for events
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_DEVICE_ORIENTATION_CHANGE, OnDeviceOrientationChanged);
    }

    // OnDestroy is called when the object is being destroyed
    public void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_DEVICE_ORIENTATION_CHANGE, OnDeviceOrientationChanged);
    }

    // Update is called once per frame
    public void Update()
    {
#if UNITY_EDITOR
        // For in editor usage.
        AdjustCameraOrthographicSize();
#endif

        if (deviceOrientationChanged && currentScreenWidth != Screen.width && currentScreenHeight != Screen.height)
        {
            // Need to wait a tiny bit for the screen to flip from landscape to portrait and vica versa, this can be detected by the screen height and width changing.
            deviceOrientationChanged = false;
            AdjustCameraOrthographicSize();
        }
    }

    private void OnDeviceOrientationChanged(System.Object args)
    {
        deviceOrientationChanged = true;
    }

    private void AdjustCameraOrthographicSize()
    {
        // Adjust the orthographic size of the camera to keep all game elements in the screen;
        // This is hardcoded, in my experience this depends on a lot of factors; such as the resolution the game was designed for, the pixels per unit of world space that is set for each image and aspect ratio.
        // Coming up a with a dynamic solution that calculates the orthographic size of the camera based on those facts just boils down to better applying this.
#if UNITY_ANDROID
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            cameraComponent.orthographicSize = portraitOrthoSize;
        else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            cameraComponent.orthographicSize = landscapeOrthoSize;

        // Ignore face up/face down orientations, those do not change the aspect ratio.
#endif

#if UNITY_EDITOR
        if (Input.deviceOrientation == DeviceOrientation.Unknown)
        {
            // Adjust for editor usage.
            if (Screen.width > Screen.height)
                cameraComponent.orthographicSize = landscapeOrthoSize;
            else
                cameraComponent.orthographicSize = portraitOrthoSize;
        }
#endif
    }
}
