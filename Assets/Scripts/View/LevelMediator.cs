// ========================================================
// Describe  ：LevelMediator
// Author    : Garson
// CreateTime: 2017/12/02
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using UnityEngine;

public class LevelMediator : Mediator
{

    public new const string NAME = "LevelMediator";
    private LevelProxy proxy;

    public LevelMediator(LevelManager manager) : base(NAME, manager)
    {
        
    }


    public override void OnRegister()
    {
        base.OnRegister();
        proxy = AppFacade.Instance.RetrieveProxy(LevelProxy.NAME) as LevelProxy;
    }
}
