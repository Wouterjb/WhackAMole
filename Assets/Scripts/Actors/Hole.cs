using UnityEngine;

public class Hole : MonoBehaviour
{
    // Editor variabels
    // TODO prefabs

    // References
    private MoleController activeMole = null;

    public bool HasActiveMole
    {
        get { return activeMole != null; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        // This can be done in a million ways; could just spawn one type of mole, randomly select a mole or keep track of types of moles and spawn depending on that etc..

        // TODO;
        // Spawn random mole
    }
}
