// ========================================================
// Describe  ：MsgType
// Author    : Garson
// CreateTime: 2017/12/02
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgType
{
    /// <summary>
    /// 初始化结束
    /// </summary>
    public const string INIT_END = "init_end";
    /// <summary>
    /// 开始关卡
    /// </summary>
    public const string LEVEL_START = "level_start";
    /// <summary>
    /// 结束关卡
    /// </summary>
    public const string LEVEL_END = "level_end";
    /// <summary>
    /// 选择改变
    /// </summary>
    public const string SELECT_CHANGED = "select_change";
    /// <summary>
    /// 选择删除
    /// </summary>
    public const string SELET_DELETE = "select_delete";
    /// <summary>
    /// 方块改变
    /// </summary>
    public const string CHANGE_CUBE = "change_cube";
    /// <summary>
    /// 游戏结束
    /// </summary>
    public const string GAME_END = "game_end";
    /// <summary>
    /// 角色移动
    /// </summary>
    public const string PLAYER_MOVE = "player_move";
    /// <summary>
    /// 添加新的方块
    /// </summary>
    public const string ADD_CUBE = "add_cube";

}
