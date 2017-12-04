using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class ChangeScriptTemplates : UnityEditor.AssetModificationProcessor
{

#region C#注释
    // 添加脚本注释模板
    private static string str =
@"// ========================================================
// Describe  ：#SCRIPTNAME#
// Author    : Garson
// CreateTime: #CreateTime#
// Version   : v1.0
// ========================================================
";
#endregion
    // 创建资源调用
    public static void OnWillCreateAsset(string path)
    {
        // 只修改C#脚本
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string allText = str;
            allText += File.ReadAllText(path);
            // 替换字符串为系统时间
            allText = allText.Replace("#CreateTime#", System.DateTime.Now.ToString("yyyy/MM/dd"));
            allText = allText.Replace("#SCRIPTNAME#", Path.GetFileNameWithoutExtension(path));
            File.WriteAllText(path, allText);
            AssetDatabase.Refresh();
        }
    }
}