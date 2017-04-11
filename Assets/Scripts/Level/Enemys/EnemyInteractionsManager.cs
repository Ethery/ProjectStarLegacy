using UnityEngine;
using System.Collections;

public class EnemyInteractionsManager : MonoBehaviour {

    public bool invincibilityODT = true;

    public float damagesOnHit = 5;
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            if (!other.GetComponent<EffectsManager>().effets[other.GetComponent<EffectsManager>().Invincible].actif)
            {
                other.GetComponent<HealthBar>().setDamages(damagesOnHit);
                if (invincibilityODT)
                {
                    other.GetComponent<EffectsManager>().ableEffect(other.GetComponent<EffectsManager>().Invincible, 1);
                }
            }
        }
    }
}
