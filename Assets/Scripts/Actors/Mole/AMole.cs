using UnityEngine;

public abstract class AMole : MonoBehaviour, IMole
{
	// Editor variables
	[Header("Mole properties")]
	[Tooltip("The amount of points scored for whacking a mole")]
	public int points = 0;

	public int Points
	{
		get { return points; }
	}

	public virtual bool OnClick()
	{
		// Normal mole behaviour; single click, score points.
		return true;
	}

	public abstract void Update();
}
