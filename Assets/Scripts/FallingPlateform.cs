using UnityEngine;
using System.Collections;

public class FallingPlateform : MonoBehaviour
{
    public AudioClip sound;
    private bool hasFall = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (hasFall == false)
            {
                hasFall = true;
                GetComponent<AudioSource>().PlayOneShot(sound);
                gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }

        }
    }
}
