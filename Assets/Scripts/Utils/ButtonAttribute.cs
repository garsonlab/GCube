#if UNITY_EDITOR
using System;

/// <summary>
/// Introduction: ButtonAttribute
///             自定义Button按钮，替代ContexMenuItem
/// Author: 	刘家诚
/// Time: 
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute
{
    public enum ExcuteMode
    {
        AlwaysEnabled,
        InEditorMode,
        InPlayMode,
    }

    private string m_Name = String.Empty;
    private ExcuteMode m_ExcuteMode = ExcuteMode.AlwaysEnabled;
    public string name { get { return m_Name; } }
    public ExcuteMode excuteMode { get { return m_ExcuteMode; } }



    public ButtonAttribute()
    {

    }

    public ButtonAttribute(string buttonName)
    {
        this.m_Name = buttonName;
    }

    public ButtonAttribute(ExcuteMode mode)
    {
        this.m_ExcuteMode = mode;
    }

    public ButtonAttribute(string buttonName, ExcuteMode mode)
    {
        this.m_Name = buttonName;
        this.m_ExcuteMode = mode;
    }

}

#endif