using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Introduction: GManager
/// Author: 	刘家诚
/// Time: 
/// </summary>
public class GManager : MonoBehaviour
{
    public Material[] materials;
    public GameObject[,] cubes = new GameObject[30, 30];




    //public void InitStart()
    //{
        
    //}





    [Button]
    void InitStart()
    {
        var parent = GameObject.Find("Cubes").transform;
        parent.position = new Vector3(-7f, 0, 0);
        for (int i = 0; i < 17; i++)
        {
            var max = 7;
            if (i < 10)
                max = 6;

            var height = Random.Range(2, max);
            for (int j = 0; j < height; j++)
            {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.SetParent(parent);
                obj.transform.localPosition = new Vector3(i, j, 0);

                var index = Random.Range(0, materials.Length);
                obj.GetComponent<Renderer>().material = materials[index];
            }
        }
    }

    [Button]
    void Clear()
    {
        var parent = GameObject.Find("Cubes").transform;

        var childCount = parent.childCount - 1;
        for (int i = childCount; i >= 0 ; i--)
        {
            var obj = parent.GetChild(i);
            DestroyImmediate(obj.gameObject);
        }
    }
}
