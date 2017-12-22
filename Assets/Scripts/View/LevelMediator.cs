// ========================================================
// Describe  ：LevelMediator
// Author    : Garson
// CreateTime: 2017/12/02
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class LevelMediator : Mediator
{

    public new const string NAME = "LevelMediator";
    private LevelProxy proxy;
    private LevelManager manager;

    public LevelMediator(LevelManager manager) : base(NAME, manager)
    {
        this.manager = manager;

        var obj = GameObject.Find("Cubes");
        if (obj == null)
            obj = new GameObject("Cubes");
        obj.transform.position = Vector3.zero;//   new Vector3(-3, -3);
        manager.Init(obj.transform);
    }


    public override void OnRegister()
    {
        base.OnRegister();
        proxy = AppFacade.Instance.RetrieveProxy(LevelProxy.NAME) as LevelProxy;
    }

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>()
        {
            MsgType.SELECT_CHANGED,
            MsgType.CHANGE_CUBE,
            MsgType.ADD_CUBE
        };
    }


    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case MsgType.SELECT_CHANGED:
                break;
            case MsgType.CHANGE_CUBE:
                manager.ChangeCubes((List<Point>)notification.Body);
                break;
            case MsgType.ADD_CUBE:
                manager.AddCubes((List<Point>) notification.Body);
                break;
        }
    }

    private void OnSelectEnd()
    {
        //关掉线

    }

    public void StartLevel()
    {
        LevelData data = proxy.GetLevelData();
        manager.LoadLevel(data);
    }

    public void AddNewData(List<Point> adds)
    {
        
    }

    public void DestroyLevel()
    {
        
    }

}
