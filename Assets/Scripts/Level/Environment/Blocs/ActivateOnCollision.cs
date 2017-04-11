using UnityEngine;
using System.Collections;

public class DestructByTrigger : MonoBehaviour {

    public GameObject choosenGameObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Projectile")
            choosenGameObject.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Projectile")
            choosenGameObject.SetActive(true);
    }
}
