// ========================================================
// Describe  ：LevelEndCommand
// Author    : Garson
// CreateTime: 2017/12/12
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class LevelEndCommand : SimpleCommand 
{
    public override void Execute(INotification notification)
    {
        var levelMediator = AppFacade.Instance.RetrieveMediator(LevelMediator.NAME) as LevelMediator;
        var levelProxy = AppFacade.Instance.RetrieveProxy(LevelProxy.NAME) as LevelProxy;

        levelProxy.RemoveInputListener();
        levelMediator.DestroyLevel();
        levelProxy.DestroyLevel();
    }
}
