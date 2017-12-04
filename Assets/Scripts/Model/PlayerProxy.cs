// ========================================================
// Describe  ：玩家代理，保存玩家数据
// Author    : Garson
// CreateTime: 2017/12/01
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using UnityEngine;

public class PlayerProxy : Proxy
{
    public new const string NAME = "PlayerProxy";


    public PlayerProxy() : base(NAME, null) { }

}
