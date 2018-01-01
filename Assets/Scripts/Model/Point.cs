// ========================================================
// Describe  ：Point
// Author    : Garson
// CreateTime: 2017/12/18
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Point
{
    private int m_x;
    private int m_y;
    private int m_value;

    public int x{get { return m_x; }}
    public int y { get { return m_y; } }
    public int value { get { return m_value; } }

    public int targetY;
    /// <summary>
    /// Target trans
    /// </summary>
    public Transform transform;

    /// <summary>
    /// Reset values
    /// </summary>
    public void Reset(int x, int y, int value)
    {
        m_x = x;
        m_y = y;
        m_value = value;
        targetY = -1;
    }

    /// <summary>
    /// Is equal to a integer pos
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public bool EqualTo(int posX, int posY)
    {
        return m_x == posX && m_y == posY;
    }

    /// <summary>
    /// Is first select point or last selected point's neighbor: left, top, right, down
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public bool IsNeighbor(int posX, int posY)
    {
        return (posX == m_x && Mathf.Abs(posY - m_y) == 1) || (posY == m_y && Mathf.Abs(posX - m_x) == 1);
    }

    /// <summary>
    /// Is same value type or generic value type
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public bool IsSimilarTo(int v)
    {
        return v == m_value;
    }

    /// <summary>
    /// Move to targetY
    /// </summary>
    public void MoveToTargetY()
    {
        m_y = targetY;
        targetY = -1;
        if (transform != null)
            //transform.localPosition = new Vector3(m_x, m_y, 0);
            transform.DOLocalMoveY(m_y, 0.2f);
    }


    public void Destroy()
    {
        transform.gameObject.name = "Delete";
        if (transform != null)
            transform.gameObject.SetActive(false);
        m_value = 0;
    }

    public override string ToString()
    {
        return string.Format("(x={0}, y={1}, v={2})", m_x, m_y, m_value);
    }
}
