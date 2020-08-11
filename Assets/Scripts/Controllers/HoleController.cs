using UnityEngine;

public class HoleController : MonoBehaviour
{
    // Editor variabels
    public GameObject[] molePrefabs = null;

    // References
    private GameObject activeMole = null;

    public bool HasActiveMole
    {
        get { return activeMole != null; }
    }

    public void Activate()
    {
        // This can be done in a million ways; could just spawn one type of mole, randomly select a mole or keep track of types of moles and spawn depending on that etc..
        int randomMoleIndex = Random.Range(0, molePrefabs.Length - 1);

        activeMole = GameObject.Instantiate(molePrefabs[randomMoleIndex], this.gameObject.transform.position, Quaternion.identity);
    }

    // This function is called when the object becomes disabled or inactive
    public void OnDisable()
    {
        if (activeMole != null)
            activeMole.SetActive(false);
    }
}
