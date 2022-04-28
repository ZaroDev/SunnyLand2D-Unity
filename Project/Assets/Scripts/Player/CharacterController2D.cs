using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	public Animator animator;
	private float movement;
	private bool isJumping = false;
	private bool isFalling = false;
	private Vector2 velocity;

	[Header("Player Options")]
	[SerializeField] private float runSpeed = 40f;
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private float coyoteTime = 0.2f;
	[SerializeField] private float jumpBufferLenght = 0.1f;
    [SerializeField] private ParticleSystem footSteps;
    [SerializeField] private ParticleSystem impactEffect;

    private bool wasOnGround = false;
	private float hangCounter = 0;
	private float jumpBufferCount = 0;
    private ParticleSystem.EmissionModule footEmission;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	private void Awake()
	{
        footEmission = footSteps.emission;
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}
	void Update()
    {
        SetState();

        GetInput();
        CheckGrounded();

        CoyoteTime();
        Jump();

        Move(movement * Time.fixedDeltaTime);
        SetParticles();
    }

    private void CheckGrounded()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    private void SetState()
    {
        velocity = m_Rigidbody2D.velocity;

        if (velocity.y < -0.1)
        {
            isFalling = true;
            isJumping = false;
        }
        else if (velocity.y > 0.1)
        {
            isJumping = true;
            isFalling = false;
        }
        else
        {
            isFalling = false;
            isJumping = false;
        }
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsFalling", isFalling);
    }

    private void Jump()
    {
        if (hangCounter > 0 && jumpBufferCount >= 0)
        {
            // Add a vertical force to the player.
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
            m_Grounded = false;
            jumpBufferCount = 0;
        }
        if (Input.GetButtonUp("Jump") && m_Rigidbody2D.velocity.y > 0)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * 0.5f);
        }
    }

    private void GetInput()
    {
        movement = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", ((uint)movement));
        //Coyote time
        // If the player should jump...
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCount = jumpBufferLenght;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }
        

    }

    private void SetParticles()
    {
        //Set the particle system on and off
        if (Input.GetAxisRaw("Horizontal") != 0 && m_Grounded)
        {
            footEmission.rateOverTime = 35;
        }
        else
        {
            footEmission.rateOverTime = 0;
        }

        if (!wasOnGround && m_Grounded)
        {
            impactEffect.gameObject.SetActive(true);
            impactEffect.Stop();
            impactEffect.transform.position = footSteps.transform.position;
            impactEffect.Play();
        }


        wasOnGround = m_Grounded;
    }

    private void CoyoteTime()
    {
        if (m_Grounded)
        {
            hangCounter = coyoteTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }
    }

    public void Move(float move)
	{
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}