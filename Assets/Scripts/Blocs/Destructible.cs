using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<ShotsManager>() != null)
        {
            Destroy(gameObject);
        }
    }
}
