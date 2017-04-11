using UnityEngine;
using System.Collections;

public class SwapColorOnHit_Slave : MonoBehaviour {

    private bool isActivate = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ShotsManager>() != null)
        {
            if (isActivate)
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1);
                isActivate = false;
            }
            else
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0);
                isActivate = true;
            }
        }

    }

    public bool isActivated()
    {
        return isActivate;
    }
}
