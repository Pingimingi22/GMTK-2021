using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerManager m_playerManager;


    public static bool m_isGameOver = false;

    public GameObject m_gameOverCanvas;
    public static GameObject m_gameOverCanvass;


    // Start is called before the first frame update
    void Start()
    {
        m_gameOverCanvass = m_gameOverCanvas;
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
    }
	public void Quit()
	{
        Application.Quit();
	}
}
