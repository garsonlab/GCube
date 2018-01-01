// ========================================================
// Describe  ：Test
// Author    : Garson
// CreateTime: 2017/12/04
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform obj;



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 p1 = Camera.main.WorldToScreenPoint(obj.position);
            Debug.Log(p1 + "," + Camera.main.ScreenToWorldPoint(p1));

            Vector3 pos = Input.mousePosition;
            pos.z = p1.z;
            Debug.Log("pz   "+Camera.main.ScreenToWorldPoint(pos));
            pos.z = 10;
            Debug.Log("10   " +Camera.main.ScreenToWorldPoint(pos));
            pos.z = 1;
            Debug.Log("1   " + Camera.main.ScreenToWorldPoint(pos));
            pos.z = -1;
            Debug.Log("-1   " + Camera.main.ScreenToWorldPoint(pos));
        }
    }

}
