// ========================================================
// Describe  ：关卡数据
// Author    : Garson
// CreateTime: 2017/12/01
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using Garson.Scripts;
using PureMVC.Patterns;
using UnityEngine;

public class LevelProxy : Proxy
{
    public new const string NAME = "LevelProxy";

    private LevelData m_levelData;
    private List<Point> m_selectPoints;
    private List<Point> m_changePoints; 
    private Point m_lastOperate;

    public LevelProxy() : base(NAME, null)
    {
        m_selectPoints = new List<Point>();
        m_changePoints = new List<Point>();
        m_lastOperate = new Point();
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public LevelData LoadLevelData(uint level)
    {
        int[,] levelData = new int[20, 15];
        for (int i = 0; i < 20; i++)
        {
            var max = 7;
            if (i < 10)
                max = 6;

            var height = Random.Range(2, max);
            for (int j = 0; j < 15; j++)
            {
                if (j < height)
                    levelData[i, j] = Random.Range(1, 7);
                else
                    levelData[i, j] = 0;
            }
        }
        m_levelData = new LevelData(1, levelData, 20, 15);

        return m_levelData;
    }
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    public LevelData GetLevelData()
    {
        return m_levelData;
    }

    public void OnSelectEnd()
    {
        
    }

    public void DestroyLevel()
    {

    }

    /// <summary>
    /// 添加输入监听事件
    /// </summary>
    public void RegisterInputListener()
    {
        InputManager.Instance.AddListener(InputType.OnMoveBegin, OnTouchMoveBegin);
        InputManager.Instance.AddListener(InputType.OnMove, OnTouchMove);
        InputManager.Instance.AddListener(InputType.OnMoveEnd, OnTouchMoveEnd);
    }
    /// <summary>
    /// 移除输入监听
    /// </summary>
    public void RemoveInputListener()
    {
        InputManager.Instance.RemoveListener(InputType.OnMoveBegin, OnTouchMoveBegin);
        InputManager.Instance.RemoveListener(InputType.OnMove, OnTouchMove);
        InputManager.Instance.RemoveListener(InputType.OnMoveEnd, OnTouchMoveEnd);
    }

    private void OnTouchMoveBegin(Vector3 pos)
    {
        m_selectPoints.Clear();
        m_lastOperate.Reset();
    }

    private void OnTouchMove(Vector3 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        int x = Mathf.CeilToInt(worldPos.x)+3;
        int y = Mathf.CeilToInt(worldPos.y)+3;
        if(m_lastOperate.EqualTo(x, y))//与上次操作相同
            return;

        if(!m_lastOperate.IsNeighbor(x, y))//不是第一个点或上一点的周围
            return;

        int v = m_levelData.GetPointValue(x, y);//当前操作有意义,小于0表示选择的是空白
        if(v <= 0)
            return;

        if(!m_lastOperate.IsSimilarTo(v))//和上次选择的类型不同（此处可扩展）
            return;

        int existIndex = -1;
        for (int i = m_selectPoints.Count-1; i >= 0; i--)
        {
            if (m_selectPoints[i].EqualTo(x, y))
            {
                existIndex = i;
                break;
            }
        }


        if (existIndex >= 0)
        {
            //如果在列表中已存在， 直接移除其后
            for (int i = m_selectPoints.Count - 1; i >= existIndex; i--)
            {
                m_selectPoints.RemoveAt(i);
            }
        }
        else
        {
            var point = new Point()
            {
                x = x,
                y = y,
                value = v
            };
            m_selectPoints.Add(point);
        }

        SendNotification(MsgType.SELECT_CHANGED, m_selectPoints);
    }

    private void OnTouchMoveEnd(Vector3 pos)
    {
        if (m_selectPoints.Count > 2)
        {
            for (int i = 0; i < m_selectPoints.Count; i++)
            {
                m_levelData.RemovePointData(m_selectPoints[i].x, m_selectPoints[i].y);
            }
            m_changePoints.Clear();
            m_levelData.GetChanges(m_changePoints);

            SendNotification(MsgType.SELECT_END, m_changePoints);
        }
    }
}


public class Point
{
    /// <summary>
    /// Pos X
    /// </summary>
    public int x;
    /// <summary>
    /// Pos Y
    /// </summary>
    public int y;

    /// <summary>
    /// If more than 0, it's target Y. Otherwise less than 0 marked as delete
    /// </summary>
    public int targetY;
    /// <summary>
    /// Block value type
    /// </summary>
    public int value;

    /// <summary>
    /// Reset values
    /// </summary>
    public void Reset()
    {
        x = -1;
        y = -1;
        targetY = 0;
        value = 0;
    }

    /// <summary>
    /// Is equal to a integer pos
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public bool EqualTo(int posX, int posY)
    {
        return x == posX && y == posY;
    }

    /// <summary>
    /// Is first select point or last selected point's neighbor: left, top, right, down
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public bool IsNeighbor(int posX, int posY)
    {
        if (x == -1 || y == -1)
            return true;
        return (posX == x && Mathf.Abs(posY-y)==1) || (posY == y && Mathf.Abs(posX-x)==1);
    }

    /// <summary>
    /// Is same value type or generic value type
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public bool IsSimilarTo(int v)
    {
        return v == value;
    }


    public override string ToString()
    {
        return string.Format("(x={0}, y={1}, v={2})", x, y, value);
    }
}