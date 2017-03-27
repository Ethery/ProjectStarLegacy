using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
public class IA : MonoBehaviour {

	public float speed;

	//direction
	SpriteRenderer enemyGraphic;
	bool canFlip = true;
	bool facingRight = false;
	float flipTime = 5f;
	float nextFlipChance = 0f;

	//attaque

	public float chargeTime;
	float startChargeTime;
	bool charging;
	Rigidbody2D enemyRB;


	// Use this for initialization
	void Start () {
		enemyRB = GetComponent<Rigidbody2D>();
		enemyGraphic = GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextFlipChance)
		{
			if(Random.Range(0,10)>= 5) flipFacing();
			nextFlipChance = Time.time + flipTime;
		}

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			if (facingRight && collision.transform.position.x < transform.position.x)
				flipFacing();
			else if (!facingRight && collision.transform.position.x > transform.position.x)
				flipFacing();

			canFlip = false;
			charging = true;
			startChargeTime = Time.time + chargeTime;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			if (startChargeTime < Time.time && charging)
			{
				if (!facingRight)
				{
					enemyRB.AddForce(new Vector2(-1, 0) * speed);
				}
				else
				{
					enemyRB.AddForce(new Vector2(1, 0) * speed);
				}
			}
		}
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			canFlip = true;
			charging = false;
			enemyRB.velocity = new Vector2(0, 0);
		}
	}

	void flipFacing()
	{
		if (!canFlip)
            return;

		enemyGraphic.flipX = !enemyGraphic.flipX;
		facingRight = !facingRight;
	}
}
