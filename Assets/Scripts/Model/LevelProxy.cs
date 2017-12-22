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

    private ObjectPool<Point> m_pointPool; 
    private LevelData m_levelData;
    private List<Point> m_selectPoints;
    private List<Point> m_changePoints; 
    private Point m_lastOperate;

    public LevelProxy() : base(NAME, null)
    {
        m_pointPool = new ObjectPool<Point>(() => new Point(), null, null, point => point.Destroy());
        m_selectPoints = new List<Point>();
        m_changePoints = new List<Point>();
        m_lastOperate = new Point();
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public LevelData LoadLevelData(int level)
    {
        m_levelData = new LevelData(level, m_pointPool);
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
        GUIManager.Instance.AddDrawer(NAME, OnMoveDrawer);
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
        m_lastOperate = null;
    }

    private void OnTouchMove(Vector3 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        int x = Mathf.CeilToInt(worldPos.x+2.5f);
        int y = Mathf.CeilToInt(worldPos.y+2.5f);
        curpos.x = x;
        curpos.y = y;
        if (m_lastOperate != null && m_lastOperate.EqualTo(x, y)) //与上次操作相同
        {
            //Debug.Log("位置没变");
            return;
        }
        if (m_lastOperate != null && !m_lastOperate.IsNeighbor(x, y)) //不是第一个点或上一点的周围
        {
            //Debug.Log("不是相邻");
            return;
        }
        Point point = m_levelData.GetPoint(x, y);//获取当前操作到对象
        if (point == null)
        {
            //Debug.Log("操作为空");
            return;
        }
        if (m_lastOperate != null && !m_lastOperate.IsSimilarTo(point.value)) //和上次选择的类型不同（此处可扩展）
        {
            //Debug.Log("不同颜色");
            return;
        }
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
            //目前判断只能顺序移除，如选择多个后从其他地方绕到第一个则不会响应
            //如果在列表中已存在， 直接移除其后
            for (int i = m_selectPoints.Count - 1; i > existIndex; i--)
            {
                m_selectPoints.RemoveAt(i);
            }
        }
        else
        {
            //Debug.Log("Add");
            m_selectPoints.Add(point);
        }
        m_lastOperate = point;

        SendNotification(MsgType.SELECT_CHANGED, m_selectPoints);
    }

    private void OnTouchMoveEnd(Vector3 pos)
    {
        if (m_selectPoints.Count > 1)
        {
            for (int i = 0; i < m_selectPoints.Count; i++)
            {
                m_levelData.RemovePoint(m_selectPoints[i]);
            }

            m_changePoints.Clear();
            m_levelData.GetChanges(m_changePoints);

            SendNotification(MsgType.CHANGE_CUBE, m_changePoints);
        }
    }


    private Vector2 curpos;
    private void OnMoveDrawer()
    {
        //GUI.Label(new Rect(0,0,400,50), "上次选中："+m_lastOperate!=null?  m_lastOperate.ToString():"", new GUIStyle(){fontSize = 24});
        GUI.Label(new Rect(0,60,400,50), string.Format("当前鼠标：{0}， {1}", curpos.ToString(), Camera.main.ScreenToWorldPoint(Input.mousePosition)+Vector3.one*2.5f),new GUIStyle(){fontSize = 24});
    }


    public Vector3 GetRolePos()
    {
        return m_levelData.GetRolePos();
    }

    public Vector3 GetNextPos()
    {
        return m_levelData.GetNextPoint();
    }

    public void RecycleCubes()
    {
        var points = m_levelData.RecyclePointData();
        if(points != null)
            SendNotification(MsgType.ADD_CUBE, points);
    }

    public void GenegrateBottom()
    {
        var points = m_levelData.GenegrateBottom();
        if (points != null || points.Count > 0)
        {
            SendNotification(MsgType.CHANGE_CUBE, points);
        }
    }
}
