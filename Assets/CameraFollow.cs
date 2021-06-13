using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target0;
    public Transform target1;

    public PlayerManager m_playerManager;


    private Vector3 m_originalCameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        m_originalCameraOffset = transform.position - target0.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_playerManager.m_currentPlayer == 0)
        //{
            transform.position = m_originalCameraOffset + target0.position;
        //}
        //else if (m_playerManager.m_currentPlayer == 1)
        //{
        //    transform.position = m_originalCameraOffset + target1.position;
        //}
    }
}
