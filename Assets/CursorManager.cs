using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CustomCursorType
{ 
    GRAPPLE,
    NONE
}
public class CursorManager : MonoBehaviour
{
    public Texture2D m_cursorTexture;
    public CursorMode m_cursormode = CursorMode.ForceSoftware;
    public Vector2 m_hotSpot = Vector2.zero;

    public static Texture2D m_cursorTextureS;
    public static CursorMode m_cursormodeS;
    public static Vector2 m_hotSpotS;

    private static CustomCursorType m_currentCursorType = CustomCursorType.NONE;

    // Start is called before the first frame update
    void Start()
    {
        m_cursorTextureS = m_cursorTexture;
        m_cursormodeS = m_cursormode;
        m_hotSpotS = m_hotSpot;
        //Cursor.SetCursor(m_cursorTexture, m_hotSpot, m_cursormode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetCursor(CustomCursorType type)
    {
        switch (type)
        {
            case CustomCursorType.GRAPPLE:
                if (m_currentCursorType != CustomCursorType.GRAPPLE)
                { 
                    Cursor.SetCursor(m_cursorTextureS, m_hotSpotS, m_cursormodeS);
                    m_currentCursorType = CustomCursorType.GRAPPLE;
                }
                break;
            case CustomCursorType.NONE:
                if (m_currentCursorType != CustomCursorType.NONE)
                { 
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    m_currentCursorType = CustomCursorType.NONE;
                }
                break;
        }
    }
}
