using UnityEngine;
using System.Collections;

public class PoisonDamages : MonoBehaviour
{

    public int damages = 2;
    public float delaiDeReaplicationDuPoison = 1.0f;
    public float timerApplication;
    public float dureePoison;

    void Start()
    {
        timerApplication = 0f;
        Destroy(transform.parent.gameObject, 20);
    }

    public void setDamages(int nDamages)
    {
        damages = nDamages;
    }
	
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<EffectsManager>() != null)
		{
			if (timerApplication > 0)
            {
                timerApplication -= Time.deltaTime;
            }
            if (timerApplication <= 0)
            {
                other.GetComponent<EffectsManager>().ableEffect(other.GetComponent<EffectsManager>().Poison, dureePoison);
                timerApplication = delaiDeReaplicationDuPoison;
				other.GetComponent<EffectsManager>().effets[other.GetComponent<EffectsManager>().Poison].degatsDeLEffet = damages;
			}
        }
    }

	void OnTriggerExit2D(Collider2D other)
	{
        if (other.GetComponent<EffectsManager>() != null)
        {
            timerApplication = 0f;
        }
    }
}
