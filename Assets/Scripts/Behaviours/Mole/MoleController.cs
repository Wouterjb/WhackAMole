using UnityEngine;

[RequireComponent(typeof(NormalMole))]
public class MoleController : MonoBehaviour
{
    // Editor variables
    [Header("Mole life cycle properties")]
    [Tooltip("The amount of time the mole stays active")]
    public float maxActiveTime = 0;

    // Collections
    private IMole[] moleBehaviours;

    // Numbers
    private int totalPoints = 0;
    private float currentActiveTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Gather up all the mole behaviours
        moleBehaviours = this.gameObject.GetComponents<IMole>();

        // Calculate total score for this mole
        for (int i = 0; i < moleBehaviours.Length; i++)
        {
            totalPoints = moleBehaviours[i].Points;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        UpdateActiveTime();
    }

    private void UpdateInput()
    {
        // Detect click input.
        if (Input.GetMouseButtonDown(0))
        {
            bool whackedMole = true;

            for (int i = 0; i < moleBehaviours.Length; i++)
            {
                moleBehaviours[i].OnClick();
            }

            if (whackedMole)
            {
                // Fire event
                EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_SCORED_POINTS, totalPoints);

                // Whacked the mole, clean it up.
                CleanUpMole();
            }
        }
    }

    private void UpdateActiveTime()
    {
        currentActiveTime += Time.deltaTime;

        if (currentActiveTime >= maxActiveTime)
            CleanUpMole();
    }

    private void CleanUpMole()
    {
        Destroy(this.gameObject);
    }
}
