// ========================================================
// Describe  ：LevelStartCommand
// Author    : Garson
// CreateTime: 2017/12/12
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using Garson.Scripts;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class LevelStartCommand : SimpleCommand 
{
    public override void Execute(INotification notification)
    {
       // uint level = (uint) notification.Body;
        uint level = 1u;
        LevelProxy levelProxy = AppFacade.Instance.RetrieveProxy(LevelProxy.NAME) as LevelProxy;
        LevelMediator levelMediator = AppFacade.Instance.RetrieveMediator(LevelMediator.NAME) as LevelMediator;

        levelProxy.LoadLevelData(level);
        levelMediator.StartLevel();
        levelProxy.RegisterInputListener();
    }
}
