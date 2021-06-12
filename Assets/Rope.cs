using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRope(Vector2 pos)
    {
        GameObject rope = Instantiate(RopeManager.m_ropeSegmentS);
        rope.transform.position = pos;
    }

    
}
