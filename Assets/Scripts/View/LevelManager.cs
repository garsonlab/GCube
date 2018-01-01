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
    Transform m_parent;
    Material[] m_materials;
    public void Init(Transform parent)
    {
        m_parent = parent;
        m_materials = new Material[7];
        for (int i = 0; i < 7; i++)
        {
            var mat = Resources.Load<Material>("Shader/cube" + i);
            m_materials[i] = mat;
        }
    }

    public void LoadLevel(LevelData data)
    {
        var datas = data.Data;
        for (int i = 0; i < datas.Count; i++)
        {
            var cols = datas[i];
            for (int j = 0; j < cols.Count; j++)
            {
                CreateCube(cols[j]);
                cols[j].transform.gameObject.name = "active";
            }
        }
    }

    public void ChangeCubes(List<Point> changes)
    {
        for (int i = 0; i < changes.Count; i++)
        {
            if (changes[i].targetY >= 0)
            {
                changes[i].MoveToTargetY();
            }
        }
    }

    public void CloseLevel()
    {
        
    }


    public void AddCubes(List<Point> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            CreateCube(list[i]);
            list[i].transform.gameObject.name = "NewAdd";
        }
    }

    private void CreateCube(Point point)
    {
        if (point.transform == null)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube); //Create from Resources by levedata
            cube.transform.SetParent(m_parent);
            point.transform = cube.transform;
            cube.transform.localScale = new Vector3(0.8f, 0.9f, 0.8f);
        }
        point.transform.localPosition = new Vector3(point.x, point.y, 0);
        point.transform.gameObject.SetActive(true);
        Renderer render = point.transform.GetComponent<Renderer>();
        render.material = m_materials[point.value];
    }

    public void SelectDelete(List<Point> list)
    {
        Debug.Log("删除选中");
    }
}
