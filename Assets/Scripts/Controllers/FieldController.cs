using UnityEngine;

public class FieldController : MonoBehaviour
{
    // Editor variables
    [Header("Field properties")]
    [Tooltip("The maximum amount of moles visible")]
    public int maxAmountOfMoles = 3; // This could also be dependant on a difficulty grade
    [Tooltip("The minimum amount of time to wait for a mole to spawn")]
    public float minSpawnInterval = 1;
    [Tooltip("The maximum amount of time to wait for a mole to spawn")]
    public float maxSpawnInterval = 3;

    // Collections
    private HoleController[] holes = null;

    // Numbers
    private float currentActivationInterval = 0.0f;
    private float currentActivationTimer = 0.0f;

    // Bool
    private bool sessionStarted = true;

    private int AmountOfHoles
    {
        get { return holes.Length; }
    }

    private int AmountOfActiveHoles
    {
        get
        {
            int activeHoles = 0;

            for (int i = 0; i < holes.Length; i++)
            {
                if (holes[i].HasActiveMole)
                    activeHoles++;
            }

            return activeHoles;
        }
    }

    // Awake is called at initialization of this class
    public void Awake()
    {
        // Gather all holes that belong to this field
        holes = this.gameObject.GetComponentsInChildren<HoleController>();
    }

    // Start is called before the first frame update
    public void Start()
    {
        // Limit max amount of moles by the amount of holes available.
        maxAmountOfMoles = Mathf.Min(AmountOfHoles, maxAmountOfMoles);

        // Generate new spawn time interval
        GenerateNewInterval();

        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_SESSION_END, OnSessionEnd);
    }

    // OnDestroy is called when the object is being destroyed
    public void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_SESSION_END, OnSessionEnd);
    }

    // Update is called once per frame
    public void Update()
    {
        if (sessionStarted)
            UpdateTimers();
    }

    private void UpdateTimers()
    {
        if (AmountOfActiveHoles <= maxAmountOfMoles)
        {
            currentActivationTimer += Time.deltaTime;

            if (currentActivationTimer >= currentActivationInterval)
            {
                // Activate hole
                ActivateHole();

                // Generate new interval
                GenerateNewInterval();

                // Keep track of variables
                currentActivationTimer = 0.0f;
            }
        }
    }

    private void GenerateNewInterval()
    {
        currentActivationInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void ActivateHole()
    {
        // Determine next hole to activate
        int nextHole = Random.Range(0, AmountOfHoles - 1);

        while (holes[nextHole].HasActiveMole)
            nextHole = Random.Range(0, AmountOfHoles - 1);

        holes[nextHole].Activate();
    }

    private void OnSessionEnd(System.Object args)
    {
        sessionStarted = false;

        for (int i = 0; i < holes.Length; i++)
        {
            holes[i].gameObject.SetActive(false);
        }
    }
}
