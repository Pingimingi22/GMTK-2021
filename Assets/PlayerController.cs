﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float m_playerSpeed;

    public Rigidbody2D m_playerRigidbody;

    private float xDelta;
    private float yDelta;

    public bool m_isGrounded = true;
    public float m_groundedCheckDistance = 0.5f;
    public float m_groundCheckYOffset = 0.5f;
    public float m_groundCheckRadius = 0.5f;

    public float m_jumpStrengthForce = 5;

    private bool m_hasJumped = false;

    private bool m_isSelecting = false;
    private Vector2 m_selectionPoint = Vector3.zero;
    private bool m_hasGrappled = false;

    public int m_playerControllerNum = 0;

    public PlayerManager m_playerManager;

    public bool m_hasRemovedThisFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xDelta = 0;
        yDelta = 0;

        m_isGrounded = false;

        GrappleSelect();

        CheckIfGrounded();

        if (Input.GetKeyDown(KeyCode.R))
            RopeManager.ReelRope(m_playerControllerNum);

        if (Input.GetMouseButtonUp(0) && m_hasRemovedThisFrame)
        {
            m_hasRemovedThisFrame = false;
        }

        if (m_hasGrappled)
            RemoveGrappleInput();
        if (m_isSelecting && !m_hasGrappled && !m_hasRemovedThisFrame)
            GrappleInput();
        


        if (Input.GetKey(KeyCode.A))
        {
            m_playerRigidbody.velocity = new Vector2((Vector3.left * m_playerSpeed * Time.deltaTime).x, m_playerRigidbody.velocity.y);
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_playerRigidbody.velocity = new Vector2((Vector3.right * m_playerSpeed * Time.deltaTime).x, m_playerRigidbody.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded && !m_hasJumped)
        {
            m_playerRigidbody.AddForce(Vector2.up * m_jumpStrengthForce, ForceMode2D.Impulse);
            m_hasJumped = true;
        }


        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Player " + m_playerControllerNum + " swapped.");
            SwapPlayers();
        }

        

    }

    private void CheckIfGrounded()
    {
        Vector2 playerFeetPos = new Vector2(transform.position.x, transform.position.y + m_groundCheckYOffset);
        RaycastHit2D[] results;
        results = Physics2D.CircleCastAll(transform.position, m_groundCheckRadius, Vector2.down);
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].collider.gameObject.layer == 0)
            {
                if (Vector2.Distance(playerFeetPos, results[i].point) < m_groundedCheckDistance)
                {
                    if (m_hasJumped == true)
                        m_hasJumped = false;
                    m_isGrounded = true;
                    return;
                }
            }
        }

    }

	private void OnDrawGizmos()
	{
        if (m_playerManager.m_currentPlayer != m_playerControllerNum)
        {
            return;
        }

        Vector3 playerPointDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Gizmos.DrawLine(transform.position, transform.position + (playerPointDirection * 10));

        Vector2 playerFeetPos = new Vector2(transform.position.x, transform.position.y + m_groundCheckYOffset);
        RaycastHit2D[] results;
        results = Physics2D.CircleCastAll(transform.position, m_groundCheckRadius, Vector2.down);
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].collider.gameObject.layer == 0)
            {
                if (Vector2.Distance(playerFeetPos, results[i].point) < m_groundedCheckDistance)
                { 
                    Gizmos.DrawLine(playerFeetPos, results[i].point);
                    return;
                }
            }
        } 
	}

    public void GrappleSelect()
    {
        Vector3 playerPointDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float grappleDistance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        //Gizmos.DrawLine(transform.position, transform.position + (playerPointDirection * 10));

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, playerPointDirection, grappleDistance);

        for (int i = 0; i < hit.Length; i++)
        { 
            if (hit[i].collider != null && hit[i].transform.root.tag != "Player")
            {
                CursorManager.SetCursor(CustomCursorType.GRAPPLE);
                m_isSelecting = true;
                m_selectionPoint = hit[i].point;
                return;
            }
            
        }

        CursorManager.SetCursor(CustomCursorType.NONE);
        m_isSelecting = false;
    }

    private void GrappleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RopeManager.CreateRopeChain(transform.position, m_selectionPoint);
            m_hasGrappled = true;
        }
    }

    private void RemoveGrappleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RopeManager.ClearRope(m_playerControllerNum);
            m_hasGrappled = false;
            m_hasRemovedThisFrame = true;
        }
    }

    public void SwapPlayers()
    {
        if (m_playerManager.m_currentPlayer == 0)
        {
            m_playerManager.m_currentPlayer = 1;
            PlayerManager.HideSelector(0);
            Debug.Log("Swapped to player 1");
            return;
        }
        else if (m_playerManager.m_currentPlayer == 1)
        {
            m_playerManager.m_currentPlayer = 0;
            PlayerManager.HideSelector(1);
            Debug.Log("Swapped to player 0");
            return;
        }
    }
}
