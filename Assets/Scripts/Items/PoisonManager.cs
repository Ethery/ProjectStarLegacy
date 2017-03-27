using UnityEngine;

public class PoisonManager : MonoBehaviour {

    public Transform flaque;
    public float X = 300;
    public int damages = 1;

	// Use this for initialization
	void Start () {
        var speed = new Vector2(X, 500);
        gameObject.GetComponent<Rigidbody2D>().AddForce(speed);
    }
	
	// Update is called once per frame
	void Update () {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<HealthBar>() != null)
        {
            other.GetComponent<HealthBar>().setDamages(damages);
        }
        Transform flaquep = Instantiate(flaque);
        flaquep.position = transform.position;
        flaquep.GetComponentInChildren<PoisonDamages>().setDamages((damages / 2)+1);
        Destroy(gameObject);
    }

    public void setDamage(int dam)
    {
        damages = dam;
    }
}
