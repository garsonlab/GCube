// ========================================================
// Describe  ：LevelStartCommand
// Author    : Garson
// CreateTime: 2017/12/12
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using Garson.Scripts;
using Garson.Scripts.View;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class LevelStartCommand : SimpleCommand 
{
    public override void Execute(INotification notification)
    {
        int level = (int)notification.Body;
        LevelProxy levelProxy = AppFacade.Instance.RetrieveProxy(LevelProxy.NAME) as LevelProxy;
        LevelMediator levelMediator = AppFacade.Instance.RetrieveMediator(LevelMediator.NAME) as LevelMediator;
        PlayerMediator playerMediator = AppFacade.Instance.RetrieveMediator(PlayerMediator.NAME) as PlayerMediator;
        CameraMediator cameraMediator = AppFacade.Instance.RetrieveMediator(CameraMediator.NAME) as CameraMediator;

        levelProxy.LoadLevelData(level);//加载数据
        levelMediator.StartLevel();//显示地形
        playerMediator.CreateRole();//加载人物
        cameraMediator.LookAt(playerMediator.Role);//移动相机
        levelProxy.RegisterInputListener();//注册输入事件
        SendNotification(MsgType.PLAYER_MOVE);
    }
}
