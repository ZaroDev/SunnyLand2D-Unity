using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumAi : MonoBehaviour
{
    [Header("Movement parameters")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 2f;
    [Header("AI parameters")]
    [SerializeField] private float chaseDistance = 4f;
    [SerializeField] private Transform viewPoint;
    [SerializeField] private float chaseSpeed = 4f;

    private RaycastHit2D frontInfo;
    private bool movingLeft = true;
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }
    private void FixedUpdate()
    {
        frontInfo = Physics2D.Raycast(viewPoint.position, transform.right, chaseDistance);
        Debug.DrawRay(viewPoint.position, transform.right * chaseDistance, Color.yellow);
        //Checking if the ray collides with anything
        if (frontInfo.collider != null)
        {
            //If the ray collides with the player chase it
            if (frontInfo.collider.CompareTag("Player"))
            {
                Chase();
                Debug.Log("Chasing player");
            }
            else
            {
                Debug.Log("Patroling the area");
                Patrol();
            }
        }
        else
        { 
            Debug.Log("Patroling the area");
            Patrol();
        }
    }

    private void Patrol()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistance);
        if (!groundInfo.collider)
        {
            if (movingLeft)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingLeft = true;
            }
        }
    }
    void Chase()
    {
        if(target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
            rb.velocity = new Vector2(moveDirection.x * chaseSpeed, rb.velocity.y);
        }
    }
}