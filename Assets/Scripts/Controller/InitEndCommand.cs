// ========================================================
// Describe  ：初始化模块结束
// Author    : Garson
// CreateTime: 2017/12/01
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class InitEndCommand : SimpleCommand 
{
    public override void Execute(INotification notification)
    {
        //启动所有
        AppFacade.Instance.RegisterProxy(new PlayerProxy());
        AppFacade.Instance.RegisterProxy(new LevelProxy());



    }
}
