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
    public int num = 5;

    void Start()
    {
        //Timer.instance;
        //var a = Time.realtimeSinceStartup;
        //Debug.Log(a);
        //Timer.instance.DelayCall(1, objs =>
        //{
        //    Debug.Log("DelayCall\t" + Time.realtimeSinceStartup+",\t" + (Time.realtimeSinceStartup-a));
        //});
        //Timer.instance.IntervalCall(1, 1f, objs =>
        //{
        //    Debug.Log("IntervalCall\t" + Time.realtimeSinceStartup + ",\t" + (Time.realtimeSinceStartup - a));
        //});
        //Timer.instance.IntervalDelayCall(3, 1f, 0.8f, objs =>
        //{
        //    Debug.Log("IntervalDelayCall\t" + Time.realtimeSinceStartup + ",\t" + (Time.realtimeSinceStartup - a));
        //    a = Time.realtimeSinceStartup;
        //});

        Timer.Instance.Init();
    }

    void Update()
    {
        Timer.Instance.OnUpdate();

        if (Input.GetKeyDown(KeyCode.A))
        {
            DelateCall();
        }
    }

    private void DelateCall()
    {
        Timer.Instance.DelayCall(1, objs =>
        {
            Debug.Log(num--);
            if (num > 0)
            {
                DelateCall();
            }
        });
    }



}
