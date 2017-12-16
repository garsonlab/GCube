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
    private uint m_index;
    private int[,] m_levelData;
    private uint m_width;
    private uint m_height;

    public LevelData(uint index, int[,] data, uint width, uint height)
    {
        this.m_index = index;
        this.m_levelData = data;
        this.m_width = width;
        this.m_height = height;
    }

    /// <summary>
    /// 编号
    /// </summary>
    public uint Index { get { return m_index; } }
    /// <summary>
    /// 长度
    /// </summary>
    public uint Width { get { return m_width; } }
    /// <summary>
    /// 高度
    /// </summary>
    public uint Height { get { return m_height; } }
    /// <summary>
    /// 数据
    /// </summary>
    public int[,] Data { get { return m_levelData; } }

    /// <summary>
    /// Get point value
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int GetPointValue(int x, int y)
    {
        if (x >= 0 && x < m_width && y >= 0 && y < m_height)
            return m_levelData[x, y];
        return 0;
    }

    /// <summary>
    /// Remove point data
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void RemovePointData(int x, int y)
    {
        if (x >= 0 && x < m_width && y >= 0 && y < m_height)
            m_levelData[x, y] = 0;
    }

    /// <summary>
    /// Get changes when removed or added
    /// </summary>
    /// <param name="m_changePoints"></param>
    public void GetChanges(List<Point> m_changePoints)
    {
        for (int i = 0; i < m_width; i++)
        {
            int flag = 0;
            for (int j = 0; j < m_height; j++)
            {
                if (m_levelData[i, j] > 0)
                {
                    //m_levelData[i, flag] = m_levelData
                }
            }
        }
    }
}
