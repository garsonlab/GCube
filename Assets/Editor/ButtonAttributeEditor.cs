#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Introduction: ButtonAttributeEditor
/// Author: 	刘家诚
/// Time: 
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(UnityEngine.Object), true)]
public class ButtonAttributeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
       ButtonAttributeHelper.InitButton(this.target, this.targets);
    }
}


public static class ButtonAttributeHelper
{
    public static void InitButton(UnityEngine.Object target, UnityEngine.Object[] targets)
    {
        var methods =
           target.GetType()
               .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
               .Where(m => m.GetParameters().Length == 0);

        foreach (var method in methods)
        {
            var button = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));
            if (button != null)
            {
                GUI.enabled = (button.excuteMode == ButtonAttribute.ExcuteMode.AlwaysEnabled)
                              || (button.excuteMode == ButtonAttribute.ExcuteMode.InEditorMode && !Application.isPlaying)
                              || (button.excuteMode == ButtonAttribute.ExcuteMode.InPlayMode && Application.isPlaying);

                var buttonName = String.IsNullOrEmpty(button.name) ? ObjectNames.NicifyVariableName(method.Name) : button.name;
                if (GUILayout.Button(buttonName))
                {
                    foreach (var t in targets)
                    {
                        method.Invoke(t, null);
                    }
                }

                GUI.enabled = true;
            }
        }
    }
}
#endif