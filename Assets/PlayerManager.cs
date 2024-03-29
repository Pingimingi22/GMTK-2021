﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public int m_currentPlayer = 0;

    public GameObject m_selector0;
    public GameObject m_selector1;

    public static GameObject m_selector0s;
    public static GameObject m_selector1s;

    public PlayerController m_playerController0;
    public PlayerController m_playerController1;

    // Start is called before the first frame update
    void Start()
    {
        m_selector0s = m_selector0;
        m_selector1s = m_selector1;

        m_playerController0.FixStuckIssue();
        m_playerController1.FixStuckIssue();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.m_isPaused)
                GameManager.UnpauseGame();
        }

        if (!GameManager.m_isGameOver && !GameManager.m_isPaused)
        {
            if (m_currentPlayer == 0)
            {
                m_playerController0.enabled = true;
                m_playerController1.enabled = false;
            }
            else if (m_currentPlayer == 1)
            {
                m_playerController0.enabled = false;
                m_playerController1.enabled = true;
            }
        }
        else // Game is over or paused so lets lock player controls.
        {
            m_playerController0.enabled = false;
            m_playerController1.enabled = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public static void HideSelector(int playerToHide)
    {
        if (playerToHide == 0)
        {
            m_selector1s.GetComponent<SpriteRenderer>().enabled = true;
            m_selector0s.GetComponent<SpriteRenderer>().enabled = false;
        }

        else if (playerToHide == 1)
        { 
            m_selector0s.GetComponent<SpriteRenderer>().enabled = true;
            m_selector1s.GetComponent<SpriteRenderer>().enabled = false;
        }
    }


}
