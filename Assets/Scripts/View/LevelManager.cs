// ========================================================
// Describe  ：LevelManager
// Author    : Garson
// CreateTime: 2017/12/02
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    List<List<Transform>> m_cubes;
    Transform m_parent;
    internal void Init(Transform parent)
    {
        m_cubes = new List<List<Transform>>();
        m_parent = parent;
    }

    internal void LoadLevel(LevelData data)
    {
        for (int i = 0; i < data.Width; i++)
        {
            if(m_cubes.Count <= i)
                m_cubes.Add(new List<Transform>());
            for (int j = 0; j < data.Height; j++)
            {
                if (data.Data[i, j] > 0)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);//Create from Resources by levedata
                    cube.transform.SetParent(m_parent);
                    cube.transform.localPosition = new Vector3(i, j, 0);
                    
                    m_cubes[i].Add(cube.transform);
                }
            }
        }
    }

    internal void CloseLevel()
    {
        
    }

}
