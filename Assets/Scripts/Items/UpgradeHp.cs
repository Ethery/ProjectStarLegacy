using UnityEngine;
using System.Collections;

public class UpgradeHp : MonoBehaviour
{

    private GameObject m_Player;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            m_Player = GameObject.FindGameObjectWithTag("Player");
            m_Player.GetComponent<HealthBar>().upgradeTotalHp();
            Destroy(gameObject);
        }
    }
}
