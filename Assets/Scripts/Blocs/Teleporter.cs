using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

    public GameObject button;

    public bool requireButtonPress = true;
    private bool waitForPress = false;

    public GameObject teleporterSortie;
    public bool passageJoueur = true;
    public bool passageTir = false;
    public Vector2 DirectionSortie;

    private Collider2D objectToTp;
    
	
	// Update is called once per frame
	void Update () {
        if (waitForPress && Input.GetButton("Submit"))
        {
            button.SetActive(false);
            waitForPress = false;
            objectToTp.transform.position = teleporterSortie.transform.position+new Vector3(1,0,0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(passageJoueur && other.tag == "Player")
        {
            if (requireButtonPress)
            {
                button.SetActive(true);
                waitForPress = true;
                objectToTp = other;
                return;
            }
            other.transform.position = teleporterSortie.transform.position;
        }
        
        if (passageTir)
        {
            if (other.tag == "Projectile")
            {
                other.transform.position = teleporterSortie.transform.position;
                if (!(DirectionSortie.x == 0 && DirectionSortie.y == 0))
                {
                    other.GetComponent<ShotsManager>().m_direction = DirectionSortie;
                }
            }
        }

        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            button.SetActive(false);
            waitForPress = false;
        }
    }
}
