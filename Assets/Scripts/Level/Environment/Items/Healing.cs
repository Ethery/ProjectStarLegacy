using UnityEngine;
using System.Collections;

public class Healing : MonoBehaviour {

    private GameObject m_Player;
    public GameObject healthBar;
    public GameObject heart;
    public float heal = 3f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            m_Player = GameObject.FindGameObjectWithTag("Player");
            //float pv = m_Player.GetComponent<HealthBar>().getPv();
      
            if(heart.tag == "smallHeart")
            {
                m_Player.GetComponent<HealthBar>().setHeal(heal);
            } else if(heart.tag == "Heart")
            {
                m_Player.GetComponent<HealthBar>().setHeal(3 * heal);
            } else
            {
                m_Player.GetComponent<HealthBar>().setHeal(6 * heal);
            }
            Destroy(heart);
        }
    }
}
