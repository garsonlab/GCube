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
    /// 操作结束
    /// </summary>
    public const string SELECT_END = "selet_end";
    /// <summary>
    /// 游戏结束
    /// </summary>
    public const string GAME_END = "game_end";
}
