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
    // List<List<Transform>> m_cubes;
    Transform m_parent;
    Material[] m_materials;
    public void Init(Transform parent)
    {
        // m_cubes = new List<List<Transform>>();
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
                if (cols[j].transform == null)
                    CreateCube(cols[j]);
                else
                {
                    cols[j].transform.localPosition = new Vector3(cols[i].x, cols[i].y, 0);
                    cols[j].transform.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ChangeCubes(List<Point> changes)
    {
        for (int i = 0; i < changes.Count; i++)
        {
            if (changes[i].transform == null)
                CreateCube(changes[i]);

            if(changes[i].value == 0)
            {
                changes[i].transform.gameObject.SetActive((false));
            }
            else if (changes[i].targetY >= 0)
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
            if (list[i].transform == null)
                CreateCube(list[i]);
            else
            {
                list[i].transform.localPosition = new Vector3(list[i].x, list[i].y, 0);
                list[i].transform.gameObject.SetActive(true);
            }
        }
    }

    private void CreateCube(Point point)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);//Create from Resources by levedata
        cube.transform.SetParent(m_parent);
        cube.transform.localPosition = new Vector3(point.x, point.y, 0);
        point.transform = cube.transform;

        Renderer render = cube.GetComponent<Renderer>();
        render.material = m_materials[point.value];
    }
}
