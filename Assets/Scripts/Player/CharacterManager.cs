using System;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 320f;                  // Amount of force added when the player jumps.
	[Range(0, 1)]
	[SerializeField]
	private float m_CrouchSpeed = .25f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, 1)]
	[SerializeField]
	private float m_AirSpeed = .6f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[SerializeField] private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        
    [SerializeField] private bool m_Grounded;            // Whether or not the player is grounded.
    private EdgeCollider2D m_GroundCheck;   // A position marking where to check for ceilings
    private Collider2D m_CeilingCheck;   // A position marking where to check for ceilings
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    public PhysicsMaterial2D glissant;
    public PhysicsMaterial2D accrochant;

	private Collider2D physicsCollider;
	private PlayerWeaponManager weaponManager;

	private void Awake()
    {
        m_GroundCheck = transform.FindChild("GroundCheck").GetComponent<EdgeCollider2D>();
        m_CeilingCheck = transform.FindChild("CeilingCheck").GetComponent<Collider2D>();
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
		weaponManager = GetComponent<PlayerWeaponManager>();
		physicsCollider = transform.FindChild("hitbox").GetComponent<Collider2D>();

	}


    private void FixedUpdate()
    {
        m_Grounded = m_GroundCheck.IsTouchingLayers(m_WhatIsGround);

        if(m_Grounded)
        {
			physicsCollider.sharedMaterial = accrochant;
        }
        else
        {
			physicsCollider.sharedMaterial = glissant;
        }

        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            
        m_Anim.SetBool("Ground", m_Grounded);
    }


    public void Move(float move, bool crouch, bool jump)
    {
            
        // Si on veut se lever : verifier si c'est possible.
        if (!crouch && m_Anim.GetBool("Crouch"))
        {
            crouch = m_CeilingCheck.IsTouchingLayers(m_WhatIsGround);
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
			
			move = (!m_Grounded ? move * m_AirSpeed : move);
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move*m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
                // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            gameObject.GetComponent<SpriteRenderer>().flipX = !m_FacingRight;
        }
        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce*m_Rigidbody2D.mass));
        }
    }

    public void Fire(float directionx,float directiony)
	{
		// 5 - Tir
		float shootx = directionx;
        float shooty = directiony;
           
        if (shootx != 0 || shooty != 0)
        {
            if (weaponManager != null)
            {
                weaponManager.Attack(new Vector2(shootx, shooty).normalized);
            }
        }
    }

	public void ChangeWeapon()
	{
		if (weaponManager != null)
		{
			weaponManager.changeWeapon();
		}
	}

	public bool wheel()
	{
		if (weaponManager != null)
		{
			return weaponManager.isActivate;
		}
		return false;
	}

	private void Flip()
    {
        m_FacingRight = !m_FacingRight;
    }

    public bool isFacingRight()
    {
        return m_FacingRight;
    }

    public bool isGrounded()
    {
        return m_Grounded;
    }

    public float GetMaxSpeed()
    {
        return m_MaxSpeed;
    }

    public float GetJumpForce()
    {
        return m_JumpForce;
    }

    public void SetMaxSpeed(float newMaxSpeed)
    {
        m_MaxSpeed = newMaxSpeed;
    }

    public void SetJumpForce(float newJumpForce)
    {
        m_JumpForce = newJumpForce;
    }

    public void jump(float jumpForce)
    {
        m_Grounded = false;
        m_Anim.SetBool("Ground", false);
        m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce));
    }
}