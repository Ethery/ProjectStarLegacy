using UnityEngine;
using System.Collections;

public class SwapColorOnHit_Watcher : MonoBehaviour {

    private bool isActivate = false;

    public GameObject Target;
    public GameObject triggers;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isActivated() == true && triggers.GetComponent<SwapColorOnHit_Slave>().isActivated() == true)
        {
            Target.SetActive(false);
        }
        else
        {
            Target.SetActive(true);
        }
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
