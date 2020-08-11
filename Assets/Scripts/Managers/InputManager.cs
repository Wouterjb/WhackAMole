using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Singleton
    private static InputManager instance;

    public static InputManager Instance
    {
        get { return instance; }
    }

    // Awake is called at initialization of this class
    public void Awake()
    {
        // Singleton creation
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<InputManager>();
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Detected input from the mouse, let's see if we have something that is being clicked
            // First convert to a screen coordinate and use that to raycast and detect colliders.
            Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D raycastHit = Physics2D.Raycast(screenPos, Vector3.zero);
            
            if (raycastHit.collider != null)
            {
                // Found a collider under the click, let's call OnClick if there is a IClickableActor
                IClickableActor clickableActor = raycastHit.collider.gameObject.GetComponent<IClickableActor>();

                if (clickableActor != null)
                    clickableActor.OnClick();
            }
        }
    }
}
