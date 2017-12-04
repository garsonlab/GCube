﻿using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

/// <summary>
/// Introduction: AppFacade
/// Author: 	刘家诚
/// Time: 
/// </summary>
public class AppFacade : Facade 
{


    public new static AppFacade Instance
    {
        get
        {
            if (m_instance == null)
            {
                lock (m_staticSyncRoot)
                {
                    m_instance = new AppFacade();
                }
            }
            return m_instance as AppFacade;
        }
    }

    protected override void InitializeFacade()
    {
        base.InitializeFacade();

        //ToDo 初始化其他的资源加载

        RegisterCommand(MsgType.INIT_END, typeof(InitEndCommand));
    }


    public void Start()
    {
        SendNotification(MsgType.INIT_END);
    }

}