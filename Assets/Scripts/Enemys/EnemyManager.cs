using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public Transform dialogue;

    public bool canMove = true;
    public bool airControl = true;
    public bool gravityAffected = true;

    private BoxCollider2D m_GroundCheck;
    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask m_WhatIsGround;

    public float totalPv = 10;

    [HideInInspector]
    public bool m_Grounded;


    public float moveSpeed=50f;
    public float maxJumpHeigth = 3;
    public float jumpForce = 50f;
    private float actualPv;


    void Awake()
    {
        actualPv = totalPv;
        if (canMove)
        {
            rb = GetComponent<Rigidbody2D>();
            if (gravityAffected)
            {
                m_GroundCheck = transform.Find("GroundCheck").GetComponent<BoxCollider2D>();
            }
        }
    }

    void FixedUpdate()
    {
        if(m_GroundCheck != null)
        {
            m_Grounded = m_GroundCheck.IsTouchingLayers(m_WhatIsGround);
        }
    }

    public void takeDamage(int damages)
    {
        actualPv -= damages;
        afficherFluctuationLife(false, damages);
        if (actualPv <= 0)
        {
            die();
        }
    }

    private void die()
    { 
        Destroy(gameObject);
    }

    public void afficherFluctuationLife(bool isHeal, float value)
    {
        if (dialogue != null)
        {
            // Création d'un objet copie du prefab
            var dialogueTransform = Instantiate(dialogue) as Transform;
            dialogueTransform.transform.SetParent(null);


            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;
            dialogueTransform.transform.position = new Vector2(x, y);

            if (isHeal)
            {
                dialogueTransform.GetComponent<Text>().color = Color.green;
                dialogueTransform.GetComponent<Text>().text = "+" + value;
            }
            else
            {
                dialogueTransform.GetComponent<Text>().color = Color.red;
                dialogueTransform.GetComponent<Text>().text = "-" + value;
            }
        }
    }

    public void Move(float move, float jumpHeigth)
    {
        if (canMove)
        {
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || airControl|| !gravityAffected)
            {
                // Move the character
                rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
            }
            // If the player should jump...
            if (m_Grounded && jumpHeigth > 0)
            {
                if (jumpHeigth > maxJumpHeigth)
                {
                    jumpHeigth = maxJumpHeigth;
                }
                // Add a vertical force to the player.
                rb.AddForce(new Vector2(0f, jumpHeigth * jumpForce));
            }
        }
    }

}