using UnityEngine;
using System.Collections;
using System;

public class Destructible : MonoBehaviour {

	private float hits_number = 0;
	public float maxHits_number;

	private void Start()
	{
		transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (1 / maxHits_number) * hits_number);
	}

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<ShotsManager>() != null)
        {
			if (hits_number < maxHits_number)
			{
				hits_number++;
				transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (1 / maxHits_number) * hits_number);
				return;
			}
            Destroy(gameObject);
        }
    }	
}
