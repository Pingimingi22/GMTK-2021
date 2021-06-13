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
        //transform.position = m_originalCameraOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerManager.m_currentPlayer == 0)
        {
            Vector3 target = m_originalCameraOffset + target0.position;
            target.z = -10;
            transform.position = Vector3.Lerp(transform.position, target, 0.05f);
        }
        else if (m_playerManager.m_currentPlayer == 1)
        {
            Vector3 target = m_originalCameraOffset + target1.position;
            target.z = -10;
            transform.position = Vector3.Lerp(transform.position, target, 0.05f);
        }
    }
}
