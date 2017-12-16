// ========================================================
// Describe  ：Launcher
// Author    : Garson
// CreateTime: 2017/12/02
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using Garson.Scripts;
using UnityEngine;

public class Launcher : MonoBehaviour
{
	void Start ()
    {
        Timer.Instance.Init();
        InputManager.Instance.Init();
		AppFacade.Instance.Start();

        //InputManager.Instance.AddListener((InputType)0, parm1 =>
        //{
        //    Debug.Log(((InputType)0).ToString());
        //});
        //InputManager.Instance.AddListener((InputType)1, parm1 =>
        //{
        //    Debug.Log(((InputType)1).ToString());
        //});
        //InputManager.Instance.AddListener((InputType)2, parm1 =>
        //{
        //    Debug.Log(((InputType)2).ToString());
        //});
        //InputManager.Instance.AddListener((InputType)3, parm1 =>
        //{
        //    Debug.Log(((InputType)3).ToString());
        //});
        //InputManager.Instance.AddListener((InputType)4, parm1 =>
        //{
        //    Debug.Log(((InputType)4).ToString());
        //});
        //InputManager.Instance.AddListener((InputType)5, parm1 =>
        //{
        //    Debug.Log(((InputType)5).ToString());
        //});
        //InputManager.Instance.AddListener((InputType)6, parm1 =>
        //{
        //    Debug.Log(((InputType)6).ToString());
        //});
        //InputManager.Instance.AddListener((InputType)7, parm1 =>
        //{
        //    Debug.Log(((InputType)7).ToString());
        //});

        //InputManager.Instance.AddListener((InputType)8, parm1 =>
        //{
        //    Debug.Log(((InputType)8).ToString());
        //});
	}

    void Update()
    {
        Timer.Instance.OnUpdate();

    }

}
