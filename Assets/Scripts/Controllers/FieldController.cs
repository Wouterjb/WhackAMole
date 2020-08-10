using UnityEngine;

public class FieldController : MonoBehaviour
{
    // Editor variables
    [Header("Field properties")]
    [Tooltip("The maximum amount of moles visible")]
    public int maxAmountOfMoles = 3; // This could also be dependant on a difficulty grade
    [Tooltip("The minimum amount of time to wait for a mole to spawn")]
    public int minSpawnInterval = 1;
    [Tooltip("The maximum amount of time to wait for a mole to spawn")]
    public int maxSpawnInterval = 3;

    // Collections
    private Hole[] holes = null;

    // Numbers
    private int activeAmountOfMoles = 0;
    private float currentSpawnInterval = 0.0f;
    private float currentSpawnTimer = 0.0f;

    private int AmountOfHoles
    {
        get { return holes.Length; }
    }

    // Awake is called at initialization of this class
    void Awake()
    {
        // Gather all holes that belong to this field
        holes = this.gameObject.GetComponentsInChildren<Hole>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Limit max amount of moles by the amount of holes available.
        maxAmountOfMoles = Mathf.Min(AmountOfHoles, maxAmountOfMoles);

        // Generate new spawn time interval
        GenerateNewSpawnInterval();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        if (activeAmountOfMoles <= maxAmountOfMoles)
        {
            currentSpawnTimer += Time.deltaTime;

            if (currentSpawnTimer >= currentSpawnInterval)
            {
                // Spawn new mole.
                SpawnNewMole();

                // Keep track of variables
                activeAmountOfMoles++;
                currentSpawnTimer = 0.0f;

                // Generate new interval
                GenerateNewSpawnInterval();
            }
        }
    }

    private void GenerateNewSpawnInterval()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnNewMole()
    {
        // TODO
        // Determine where to spawn a mole; when a hole already has a mole, do not spawn it there, but another place etc.
    }
}
