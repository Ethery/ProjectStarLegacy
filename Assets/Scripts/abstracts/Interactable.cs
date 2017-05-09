using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	protected bool active,ranged,nRanged = true;
	public bool isActive()
	{
		return active;
	}
	public bool isRanged()
	{
		return ranged;
	}
	public bool needRanged()
	{
		return nRanged;
	}
	public abstract bool Activate(bool a, string key);

	public abstract void Activate(bool a);

	public virtual void init(){return;}

	public virtual bool nextStep(string key) { return false; }

	public virtual bool previousStep(string key) { return false; }
    
}
