using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerManager m_playerManager;


    public static bool m_isGameOver = false;
    public static bool m_isPaused = false;

    public GameObject m_gameOverCanvas;
    public static GameObject m_gameOverCanvass;

    public GameObject m_pauseCanvas;
    public static GameObject m_pauseCanvass;

    // Start is called before the first frame update
    void Start()
    {
        m_gameOverCanvass = m_gameOverCanvas;
        m_pauseCanvass = m_pauseCanvas;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void GameOver()
    {
        m_isGameOver = true;
        m_gameOverCanvass.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        m_isGameOver = false;
        m_isPaused = false;
    }
	public void Quit()
	{
        Application.Quit();
	}

    public static void PauseGame()
    {
        m_pauseCanvass.SetActive(true);
        m_isPaused = true;
    }
    public static void UnpauseGame()
    {
        m_pauseCanvass.SetActive(false);
        m_isPaused = false;
    }
}
