using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Editor variables
    [Header("Camera orthographic sizes")]
    public float portraitOrthoSize = 14;
    public float landscapeOrthoSize = 5;

    // References
    private Camera cameraComponent;

    // Start is called before the first frame update
    public void Start()
    {
        // Grab camera component
        cameraComponent = this.gameObject.GetComponent<Camera>();

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
    }

    private void OnDeviceOrientationChanged(System.Object args)
    {
        AdjustCameraOrthographicSize();
    }

    private void AdjustCameraOrthographicSize()
    {
        // Adjust the orthographic size of the camera to keep all game elements in the screen;
        // This is hardcoded, in my experience this depends on a lot of factors; such as the resolution the game was designed for, the pixels per unit of world space that is set for each image and aspect ratio.
        // Coming up a with a dynamic solution that calculates the orthographic size of the camera based on those facts just boils down to better applying this.
#if UNITY_ANDROID
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            cameraComponent.orthographicSize = portraitOrthoSize;
        else
            cameraComponent.orthographicSize = landscapeOrthoSize;
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
