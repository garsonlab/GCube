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
                point.Reset(i, j, Random.Range(1, 5));

                cols.Add(point);
            }
        }
    }

    public int Index { get { return m_index; } }
    public List<List<Point>> Data { get { return m_levelData; } }

    public Point GetPoint(int x, int y)
    {
        if(x>=0 && x < m_levelData.Count && y >= 0 && y < m_levelData[x].Count)
            return m_levelData[x][y];
        return null;
    }

    public void RemovePoint(Point point)
    {
        point.Destroy();
        m_levelData[point.x].Remove(point);
        m_pointPool.Release(point);
    }


    public void GetChanges(List<Point> m_changePoints, int from, int to)
    {
        //from = from - 5 >= 0 ? from - 5 : 0;//前后预留5
        //to = to + 5 >= m_levelData.Count ? m_levelData.Count - 1 : to + 5;

        int flag = 0;
        for (int i = from; i <= to; i++)
        {
            flag = 0;
            var cols = m_levelData[i];
            for (int j = 0; j < cols.Count; j++)
            {
                var point = cols[j];
                if (point.value <= 0)//排除有0值
                {
                    point.Destroy();
                    cols.Remove(point);
                    m_pointPool.Release(point);
                    j--;
                }
                else
                {
                    if (point.y != flag)
                    {
                        point.targetY = flag;
                        m_changePoints.Add(point);
                    }
                    else
                    {
                        point.targetY = -1;
                    }

                    flag++;
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
                point.Reset(x, j, Random.Range(1, 5));

                cols.Add(point);
            }
            return cols;
        }
        return null;
    }



    public Vector2 GenegrateBottom(List<Point> adds)
    {
        int offset = 3;//距离人物位置长度
        int addLen = 3;//添加长度
        Vector2 range = Vector2.zero;

        if (m_levelData.Count > m_flag + offset)//在添加范围内
        {
            range.x = m_flag + offset;
            range.y = m_flag + offset + addLen;
            range.y = range.y >= m_levelData.Count ? m_levelData.Count - 1 : range.y;

            for (int i = (int)range.x; i < (int)range.y; i++)
            {
                var cols = m_levelData[i];
                var point = m_pointPool.Get();
                point.Reset(i, 0, Random.Range(1, 5));
                cols.Insert(0, point);
                adds.Add(point);
            }
        }
        return range;
    }
}
