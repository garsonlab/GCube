// ========================================================
// Describe  ：LevelData
// Author    : Garson
// CreateTime: 2017/12/12
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    private int m_index;//当前等级
    private int m_flag;//当前在第几列
    private readonly int m_reserve = 5;
    private List<List<Point>> m_levelData;
    private ObjectPool<Point> m_pointPool;

    public LevelData(int index, ObjectPool<Point> pointPool)
    {
        m_levelData = new List<List<Point>>();
        m_pointPool = pointPool;
        m_flag = m_reserve-1;
        //TODO Init data here.

        for (int i = 0; i < 15; i++)
        {
            List<Point> cols = new List<Point>();
            m_levelData.Add(cols);

            var height = 3;
            if (i >= m_reserve)
            {
                var max = 7;
                if (i < 10)
                    max = 5;
                height = Random.Range(2, max);
            }
           
            for (int j = 0; j < height; j++)
            {
                var point = m_pointPool.Get();
                point.Reset(i, j, Random.Range(1, 7));

                cols.Add(point);
            }
        }
    }

    /// <summary>
    /// 编号
    /// </summary>
    public int Index { get { return m_index; } }
    /// <summary>
    /// 长度
    /// </summary>
    // public uint Width { get { return m_width; } }
    /// <summary>
    /// 高度
    /// </summary>
    // public uint Height { get { return m_height; } }
    /// <summary>
    /// 数据
    /// </summary>
    public List<List<Point>> Data { get { return m_levelData; } }

    /// <summary>
    /// Get point value
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Point GetPoint(int x, int y)
    {
        // if (x >= 0 && x < m_width && y >= 0 && y < m_height)
        //     return m_levelData[x, y];
        if(x>=0 && x < m_levelData.Count && y >= 0 && y < m_levelData[x].Count)
            return m_levelData[x][y];
        return null;
    }

    /// <summary>
    /// Remove point data
    /// </summary>
    public void RemovePoint(Point point)
    {
        point.Destroy();
    }

    /// <summary>
    /// Get changes when removed or added
    /// </summary>
    /// <param name="m_changePoints"></param>
    public void GetChanges(List<Point> m_changePoints)
    {
        for (int i = 0; i < m_levelData.Count; i++)
        {
            int flag = 0;
            var cols = m_levelData[i];
            for (int j = 0; j < cols.Count; j++)
            {
                if(cols[j].value > 0)
                {
                    if(cols[j].y != flag)
                    {
                        cols[j].targetY = flag;

                        m_changePoints.Add(cols[j]);
                    }
                    flag++;
                }
                else
                {
                    m_changePoints.Add(cols[j]);
                }
            }
        }

    }


    /// <summary>
    /// 获取角色当前位置
    /// </summary>
    /// <returns>列、高度、状态</returns>
    public Vector3 GetRolePos()
    {
        return new Vector3(m_flag, m_levelData[m_flag].Count, 0);
    }

    /// <summary>
    /// 获取下一步位置
    /// </summary>
    /// <returns>不能前行返回(0，0，下一步状态)，前行返回下一步</returns>
    public Vector3 GetNextPoint()
    {
        int curHeight = m_levelData[m_flag].Count;
        int nextHeight = m_levelData[m_flag+1].Count;

        if (Mathf.Abs(curHeight - nextHeight) <= 1)
        {
            m_flag++;//生成下一列

            curHeight = m_levelData[m_flag].Count;
            nextHeight = m_levelData[m_flag + 1].Count;
            return new Vector3(m_flag, curHeight, nextHeight-curHeight);
        }
        return new Vector3(0, 0, nextHeight-curHeight);
    }

    public List<Point> RecyclePointData()
    {
        int tem = m_flag - m_reserve;
        if (tem > 0)//回收超出范围的
        {
            for (int i = 0; i < tem; i++)
            {
                if (m_levelData[i].Count > 0)
                {
                    while (m_levelData[i].Count > 0)
                    {
                        var point = m_levelData[i][0];
                        m_levelData[i].RemoveAt(0);

                        point.Destroy();
                        m_pointPool.Release(point);
                    }
                }
            }
        }

        if (m_levelData.Count < m_flag + 10)
        {
            List<Point> cols = new List<Point>();
            m_levelData.Add(cols);
            int x = m_levelData.Count - 1;

            var height = Random.Range(2, 7);

            for (int j = 0; j < height; j++)
            {
                var point = m_pointPool.Get();
                point.Reset(x, j, Random.Range(1, 7));

                cols.Add(point);
            }
            return cols;
        }
        return null;
    }



    public List<Point> GenegrateBottom()
    {
        int offset = 3;//距离中间范围
        int addLen = 3;
        if (m_levelData.Count > m_flag + offset)
        {
            List<Point> changes = new List<Point>();
            for (int i = m_flag + offset; i < m_flag + offset+addLen; i++)
            {
                if (m_levelData.Count > i)
                {
                    var cols = m_levelData[i];
                    var point = m_pointPool.Get();
                    point.Reset(i, 0, Random.Range(1, 7));
                    cols.Insert(0, point);

                    for (int j = 0; j < cols.Count; j++)
                    {
                        cols[j].targetY = j;
                        changes.Add(cols[j]);
                    }
                }
            }
            return changes;
        }

        return null;
    }
}
