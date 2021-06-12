using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{

    public GameObject m_ropeSegment;
    public static GameObject m_ropeSegmentS;

    public PlayerManager m_playerManager;
    public static PlayerManager m_playerManagerS;


    private static float m_ropeWidth;



    public static List<GameObject> m_player0Ropes;
    public static List<GameObject> m_player1Ropes;

    // Start is called before the first frame update
    void Start()
    {
        m_player0Ropes = new List<GameObject>();
        m_player1Ropes = new List<GameObject>();

        m_ropeSegmentS = m_ropeSegment;
        m_ropeWidth = (GameObject.Find("rope").GetComponent<SpriteRenderer>().bounds.size.x);
        m_playerManagerS = m_playerManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CreateRopeChain(Vector2 from, Vector2 to)
    {
        float distance = Vector2.Distance(from, to);

        Vector2 dir = (to - from).normalized;

        float ropeRotation = Mathf.Atan2(dir.y, dir.x);
        ropeRotation *= Mathf.Rad2Deg;

        float amountOfRopeSegments = distance / m_ropeWidth;
        Debug.Log("Amount of rope segments required: " + amountOfRopeSegments);

        //List<GameObject> ropeChain = new List<GameObject>();

        for (int i = (int)amountOfRopeSegments; i >= 0; i--)
        {
            if (i == (int)amountOfRopeSegments)
            {
                // Initial rope needs a fixed joint rather than a hinge joint. It will be connected to the terrain rather than other ropes.
                GameObject newRopeSegment = Instantiate(m_ropeSegmentS);

                // Giving the intial rope a unique name for debugging purposes.
                newRopeSegment.name = "Initial_segment";

                //Destroy(newRopeSegment.GetComponent<HingeJoint2D>());
                //newRopeSegment.AddComponent<FixedJoint2D>();
                //FixedJoint2D fixedJoint = newRopeSegment.GetComponent<FixedJoint2D>();
                //fixedJoint.anchor = Vector3.zero;
                //fixedJoint.connectedAnchor = Vector3.zero;
                //fixedJoint.transform.position = to;

                

                HingeJoint2D hingeJoint = newRopeSegment.GetComponent<HingeJoint2D>();
                hingeJoint.anchor = Vector3.zero;
                hingeJoint.connectedAnchor = to;
                hingeJoint.transform.position = to;

                JointAngleLimits2D hingeLimit = hingeJoint.limits;
                hingeLimit.min = -90;
                hingeLimit.max = 90;
                hingeJoint.limits = hingeLimit;

                // Disabling the first rope segments box collider so it doesn't spazz out.
                BoxCollider2D collider = newRopeSegment.GetComponent<BoxCollider2D>();
                collider.enabled = false;

                newRopeSegment.transform.rotation = Quaternion.Euler(newRopeSegment.transform.rotation.x, newRopeSegment.transform.rotation.y, ropeRotation);

                if (m_playerManagerS.m_currentPlayer == 0)
                    m_player0Ropes.Insert(0, newRopeSegment);
                else if (m_playerManagerS.m_currentPlayer == 1)
                    m_player1Ropes.Insert(0, newRopeSegment);
            }
            else
            {
                GameObject otherRopeSegment = Instantiate(m_ropeSegmentS);
                Vector2 posPoint = Vector2.zero;
                if (m_playerManagerS.m_currentPlayer == 0)
                {
                    SetRopeRigidbody(otherRopeSegment, m_player0Ropes[0].GetComponent<Rigidbody2D>(), 0);

                    posPoint = m_player0Ropes[0].transform.localPosition - m_player0Ropes[0].transform.right * m_ropeWidth;
                }
                

                else if (m_playerManagerS.m_currentPlayer == 1)
                {
                    SetRopeRigidbody(otherRopeSegment, m_player1Ropes[0].GetComponent<Rigidbody2D>(), 0);

                    posPoint = m_player1Ropes[0].transform.localPosition - m_player1Ropes[0].transform.right * m_ropeWidth;
                }

                Vector2 anchorPoint = new Vector2(-0.08f, 0);

                otherRopeSegment.transform.position = posPoint;

                otherRopeSegment.transform.rotation = Quaternion.Euler(otherRopeSegment.transform.rotation.x, otherRopeSegment.transform.rotation.y, ropeRotation);

                SetConnectedAnchorPoint(otherRopeSegment, anchorPoint, 0);

                if (m_playerManagerS.m_currentPlayer == 0)
                    m_player0Ropes.Insert(0, otherRopeSegment);
                else if (m_playerManagerS.m_currentPlayer == 1)
                    m_player1Ropes.Insert(0, otherRopeSegment);
            }

        }

        if (m_playerManagerS.m_currentPlayer == 0)
        {
            m_playerManagerS.m_playerController0.gameObject.AddComponent<HingeJoint2D>();
            HingeJoint2D playerHinge = m_playerManagerS.m_playerController0.GetComponent<HingeJoint2D>();
            playerHinge.autoConfigureConnectedAnchor = false;
            // set players hinge to not auto configure.

            Vector2 playerAnchorPoint = new Vector2(-0.08f, 0);

            SetRopeRigidbody(m_playerManagerS.m_playerController0.gameObject, m_player0Ropes[0].GetComponent<Rigidbody2D>(), 0);

            SetConnectedAnchorPoint(m_playerManagerS.m_playerController0.gameObject, playerAnchorPoint, 0);
        }
        else
        {
            m_playerManagerS.m_playerController1.gameObject.AddComponent<HingeJoint2D>();
            //HingeJoint2D playerHinge = m_playerManagerS.m_playerController1.GetComponent<HingeJoint2D>();
            HingeJoint2D[] playerHinges = m_playerManagerS.m_playerController1.GetComponents<HingeJoint2D>();
            playerHinges[1].autoConfigureConnectedAnchor = false;
            Vector2 playerAnchorPoint = new Vector2(-0.08f, 0);

            SetRopeRigidbody(m_playerManagerS.m_playerController1.gameObject, m_player1Ropes[0].GetComponent<Rigidbody2D>(), 1);

            SetConnectedAnchorPoint(m_playerManagerS.m_playerController1.gameObject, playerAnchorPoint, 1);
        }


        //GameObject otherRopeSegment = Instantiate(m_ropeSegmentS);
        //SetRopeRigidbody(otherRopeSegment, ropeChain[0].GetComponent<Rigidbody2D>());
        //Matrix4x4 previousChainSpace = ropeChain[0].transform.worldToLocalMatrix;
        //Matrix4x4 worldSpace = ropeChain[0].transform.localToWorldMatrix;
        //Vector2 posPoint = previousChainSpace * (new Vector2(ropeChain[0].transform.position.x - (m_ropeWidth), ropeChain[0].transform.position.y));
        //
        //Vector2 anchorPoint = new Vector2(-0.08f, 0);
        ////anchorPoint = worldSpace * anchorPoint;
        ////Matrix4x4 localSpace = otherRopeSegment.transform.worldToLocalMatrix;
        ////anchorPoint = localSpace * anchorPoint;
        //
        //posPoint = worldSpace * posPoint;
        //otherRopeSegment.transform.position = posPoint;
        //otherRopeSegment.transform.Rotate(otherRopeSegment.transform.rotation.x, otherRopeSegment.transform.rotation.y, ropeRotation);
        //
        //SetConnectedAnchorPoint(otherRopeSegment, anchorPoint);
        //
        //
        //
        //
        //
        //
        //
        //GameObject otherRopeSegment1 = Instantiate(m_ropeSegmentS);
        //SetRopeRigidbody(otherRopeSegment1, otherRopeSegment.GetComponent<Rigidbody2D>());
        //Matrix4x4 previousChainSpace1 = ropeChain[0].transform.worldToLocalMatrix;
        //Matrix4x4 worldSpace1 = otherRopeSegment.transform.localToWorldMatrix;
        //Vector2 posPoint1 = previousChainSpace * (new Vector2(otherRopeSegment.transform.position.x - (m_ropeWidth), otherRopeSegment.transform.position.y));
        //
        //Vector2 anchorPoint1 = new Vector2(-0.08f, 0);
        ////anchorPoint = worldSpace * anchorPoint;
        ////Matrix4x4 localSpace = otherRopeSegment.transform.worldToLocalMatrix;
        ////anchorPoint = localSpace * anchorPoint;
        //
        //posPoint1 = worldSpace1 * posPoint1;
        //otherRopeSegment1.transform.position = posPoint1;
        //otherRopeSegment1.transform.Rotate(otherRopeSegment1.transform.rotation.x, otherRopeSegment1.transform.rotation.y, ropeRotation);
        //
        //SetConnectedAnchorPoint(otherRopeSegment1, anchorPoint1);



        //SetAnchorPoint(otherRopeSegment, anchorPoint);

    }

    private static void SetRopeRigidbody(GameObject rope, Rigidbody2D rigidbody, int component)
    {
        HingeJoint2D[] joints = rope.GetComponents<HingeJoint2D>();
        joints[component].connectedBody = rigidbody;
    }
    private static void SetHingeSettings(GameObject rope)
    {
        HingeJoint2D joint = rope.GetComponent<HingeJoint2D>();
        joint.useLimits = true;
        JointAngleLimits2D limits = joint.limits;
        limits.min = -45;
        limits.max = 45;
        joint.limits = limits;

    }
    private static void SetConnectedAnchorPoint(GameObject rope, Vector2 pos, int component)
    {
        HingeJoint2D[] joints = rope.GetComponents<HingeJoint2D>();
        joints[component].connectedAnchor = pos;
    }
    private static void SetAnchorPoint(GameObject rope, Vector2 pos)
    {
        HingeJoint2D joint = rope.GetComponent<HingeJoint2D>();
        joint.anchor = pos;
    }

    public static void ClearRope(int player)
    {
        if (player == 0)
        {
            Destroy(m_playerManagerS.m_playerController0.GetComponent<HingeJoint2D>());
            for (int i = 0; i < m_player0Ropes.Count; i++)
            {
                Destroy(m_player0Ropes[i]);
            }
            m_player0Ropes.Clear();
        }
        else if (player == 1)
        {
            HingeJoint2D[] joints = m_playerManagerS.m_playerController1.GetComponents<HingeJoint2D>();
            Destroy(joints[1]);
            for (int i = 0; i < m_player1Ropes.Count; i++)
            {
                Destroy(m_player1Ropes[i]);
            }
            m_player1Ropes.Clear();
        }
    }

    public static void ReelRope(int player)
    {
        //m_playerManagerS.m_playerController0.gameObject.AddComponent<HingeJoint2D>();
        Vector2 playerAnchorPoint = new Vector2(-0.08f, 0);

        if (player == 0)
        {
            Destroy(m_player0Ropes[0]);
            m_player0Ropes.RemoveAt(0);

            SetRopeRigidbody(m_playerManagerS.m_playerController0.gameObject, m_player0Ropes[0].GetComponent<Rigidbody2D>(), 0);

            SetConnectedAnchorPoint(m_playerManagerS.m_playerController0.gameObject, playerAnchorPoint, 0);
        }
        else if (player == 1)
        {
            Destroy(m_player1Ropes[0]);
            m_player1Ropes.RemoveAt(0);

            SetRopeRigidbody(m_playerManagerS.m_playerController1.gameObject, m_player1Ropes[0].GetComponent<Rigidbody2D>(), 0);

            SetConnectedAnchorPoint(m_playerManagerS.m_playerController1.gameObject, playerAnchorPoint, 0);
        }
    }
}
