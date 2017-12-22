// ========================================================
// Describe  ：初始化模块结束
// Author    : Garson
// CreateTime: 2017/12/01
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using Garson.Scripts.View;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class InitEndCommand : SimpleCommand 
{
    public override void Execute(INotification notification)
    {
        //启动所有
        //Proxy
        AppFacade.Instance.RegisterProxy(new LevelProxy());

        //Mediator
        AppFacade.Instance.RegisterMediator(new LevelMediator(new LevelManager()));
        AppFacade.Instance.RegisterMediator(new PlayerMediator());
        AppFacade.Instance.RegisterMediator(new CameraMediator());
        //init ui

        SendNotification(MsgType.LEVEL_START, 1);
    }
}
