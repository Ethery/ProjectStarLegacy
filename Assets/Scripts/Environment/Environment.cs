using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour
{
    public float ProjectileSlow = 2;
    
    public int damageDot = 10;

    public GameObject airBar;

    private GameObject m_player;

    // Use this for initialization
    void Start () {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (gameObject.tag == "Water")
            {
                m_player.GetComponent<EffectsManager>().activateEffect(m_player.GetComponent<EffectsManager>().Noyade, 1f, damageDot);
            }
            else if (gameObject.tag == "Lava")
            {
                m_player.GetComponent<EffectsManager>().activateEffect(m_player.GetComponent<EffectsManager>().Feu, 1f, damageDot);
            }
        }
        if(other.tag == "Projectile")
        {
            other.GetComponent<ShotsManager>().SetM_speed(other.GetComponent<ShotsManager>().GetM_speed() / 2);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            m_player.GetComponent<EffectsManager>().forceDesactivateEffect(m_player.GetComponent<EffectsManager>().Noyade);
            m_player.GetComponent<EffectsManager>().disableEffect(m_player.GetComponent<EffectsManager>().Feu);
        }
        if (other.tag == "Projectile")
        {
            other.GetComponent<ShotsManager>().SetM_speed(other.GetComponent<ShotsManager>().GetM_speed() * 2);
        }
    }

    
}
