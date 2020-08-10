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
    void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D raycastHit = Physics2D.Raycast(screenPos, Vector3.zero);
            
            if (raycastHit.collider != null)
            {
                IClickableActor clickableActor = raycastHit.collider.gameObject.GetComponent<IClickableActor>();

                if (clickableActor != null)
                    clickableActor.OnClick();
            }
        }
    }
}
