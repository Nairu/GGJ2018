using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2DController : MonoBehaviour {

    public float m_MaxSpeed = 10f;
    public float m_Speed = 2f;
    public float m_GravityMultiplier = 1.5f;
    public float m_JumpForce = 400f;
    public int m_MaxNumberOfJumps = 2;
    public float m_CrouchSpeed = .36f;
    public bool m_AirControl = false;
    public LayerMask m_WhatIsGround;
    public Transform m_Graphics;

    private int m_numberOfJumps = 0;
    private Transform m_GroundCheck;
    private float k_GroundedRadius;
    private bool m_Grounded;
    private Transform m_CeilingCheck;
    private float k_CeilingRadius = 0.01f;
    private Rigidbody m_Rigidbody;

    private bool justJumped = false;

    void Awake()
    {
        if (m_GroundCheck == null)
            m_GroundCheck = transform.Find("GroundCheck");
        if (m_CeilingCheck == null)
            m_CeilingCheck = transform.Find("CeilingCheck");
    
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        m_Grounded = false;

        if (!justJumped)
        {
            Collider[] colliders = Physics.OverlapSphere(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_numberOfJumps = 0;
                    m_Grounded = true;
                }
            }
        }
        else
        {
            justJumped = false;
        }

        m_Rigidbody.AddForce(Physics.gravity * m_GravityMultiplier, ForceMode.Acceleration);
    }

    public void move(float move, bool crouch, bool jump)
    {
        if (!crouch)
        {
            if (Physics.CheckSphere(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // only move if we're on the ground or can control movement in the air
        if (m_Grounded || m_AirControl)
        {
            move = (crouch ? move * m_CrouchSpeed : move);

            if (!WillHitWall(move))
            {
                float t = m_Rigidbody.velocity.x / m_MaxSpeed;
                float lerp = 0f;

                if (t >= 0f)
                    lerp = Mathf.Lerp(m_MaxSpeed, 0f, t);
                else if (t < 0f)
                    lerp = Mathf.Lerp(m_MaxSpeed, 0f, Mathf.Abs(t));

                Vector2 movement = new Vector2(move * lerp * m_Speed, 0f);

                m_Rigidbody.AddForce(movement, ForceMode.Acceleration);
                //m_Rigidbody.AddForce(movement, ForceMode.Impulse);
            }
        }

        if (jump)
        {
            //Jump();
        }
    }

    public void AddForce(Vector2 force)
    {
        if (m_Grounded)
        {
            force = new Vector2(force.x, 0f);
        }

        m_Rigidbody.velocity = force;
    }

    private bool WillHitWall(float move)
    {
        RaycastHit hit;
        bool hitWall = false;
        if (move > 0f)
        {
            if (Physics.SphereCast(transform.position, 0.5f, transform.right, out hit, 6f))
            {
                if (hit.collider.tag == "environmentTag")
                    hitWall = true;
            }
        }
        else if (move < 0f)
        {
            if (Physics.SphereCast(transform.position, 0.5f, -transform.right, out hit, 6f))
            {
                if (hit.collider.tag == "environmentTag")
                    hitWall = true;
            }
        }
        return hitWall;
    }
}
