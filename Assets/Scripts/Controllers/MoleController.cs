using UnityEngine;

[RequireComponent(typeof(NormalMole))]
[RequireComponent(typeof(Collider2D))]
public class MoleController : MonoBehaviour, IClickableActor
{
    // Editor variables
    [Header("Mole life cycle properties")]
    [Tooltip("The amount of time the mole stays active")]
    public float maxActiveTime = 0;

    // Collections
    private IMole[] moleBehaviours = null;

    // Numbers
    private int totalPoints = 0;
    private float currentActiveTime = 0.0f;

    // Awake is called at initialization of this class
    void Awake()
    {
        // Gather up all the mole behaviours
        moleBehaviours = this.gameObject.GetComponents<IMole>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Calculate total score for this mole
        for (int i = 0; i < moleBehaviours.Length; i++)
        {
            totalPoints = moleBehaviours[i].Points;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        UpdateActiveTime();
    }

    public void OnClick()
    {
        bool whackedMole = true;

        for (int i = 0; i < moleBehaviours.Length; i++)
        {
            // Check all mole behaviours for succesful whacking; some behaviours might require double click or something else.
            whackedMole = moleBehaviours[i].OnClick();

            // If not succesfull, stop checking, there's no score to be had
            if (!whackedMole)
            {
                // Could check here for a penalty on the mole; for example, if a mole has spikes and the player clicks on it, raise an event to deduct score
                break;
            }
        }

        if (whackedMole)
        {
            // Fire event
            EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_SCORED_POINTS, totalPoints);

            // Whacked the mole, clean it up.
            CleanUpMole();
        }
    }

    private void UpdateActiveTime()
    {
        // Keep track of current activation time.
        currentActiveTime += Time.deltaTime;

        if (currentActiveTime >= maxActiveTime)
            CleanUpMole();
    }

    private void CleanUpMole()
    {
        Destroy(this.gameObject);
    }
}
