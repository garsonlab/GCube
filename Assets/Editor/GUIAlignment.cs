// ========================================================
// Describe  ：GUIAlignment
// Author    : Garson
// CreateTime: 2017/12/14
// Version   : v1.0
// ========================================================
///
/// Author:刘家诚
/// Time:2017/2/27
/// Des:扩展RectTransform，一键对齐
/// 

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RectTransform))]
public class GUIAlignment : DecoratorEditor
{
    public GUIAlignment() : base("RectTransformEditor") { }
    

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

	    EditorGUILayout.BeginHorizontal();//横向
        EditorGUILayout.LabelField("AlignParent");

        EditorGUILayout.BeginVertical();//开始绘制九宫格
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();//first row
        if (GUILayout.Button("┏", GUILayout.Width(25)))
        {
            RectTransform self = (RectTransform)target; //获得当前操作的transform，target是父中参数
            if (self.parent != null)
            {
                RectTransform parent = (RectTransform)self.parent;          
                SetPos(self, parent, 1);
            }
        }
        if (GUILayout.Button("┳", GUILayout.Width(25)))
        {
            RectTransform self = (RectTransform)target;
            if (self.parent != null)
            {
                RectTransform parent = (RectTransform)self.parent;
                SetPos(self, parent, 2);
            }
        }
        if (GUILayout.Button("┓", GUILayout.Width(25)))
        {
            RectTransform self = (RectTransform)target;
            if (self.parent != null)
            {
                RectTransform parent = (RectTransform)self.parent;
                SetPos(self, parent, 3);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();//second row
        if (GUILayout.Button("┣", GUILayout.Width(25)))
        {
            RectTransform self = (RectTransform)target;
            if (self.parent != null)
            {
                RectTransform parent = (RectTransform)self.parent;
                SetPos(self, parent, 4);
            }
        }
        if (GUILayout.Button("╋", GUILayout.Width(25)))
        {
            RectTransform self = (RectTransform)target;
            if (self.parent != null)
            {
                RectTransform parent = (RectTransform)self.parent;
                SetPos(self, parent, 5);
            }
        }
        if (GUILayout.Button("┫", GUILayout.Width(25)))
        {
            RectTransform self = (RectTransform)target;
            if (self.parent != null)
            {
                RectTransform parent = (RectTransform)self.parent;
                SetPos(self, parent, 6);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();//third row
        if (GUILayout.Button("┗", GUILayout.Width(25)))
        {
            RectTransform self = (RectTransform)target;
            if (self.parent != null)
            {
                RectTransform parent = (RectTransform)self.parent;
                SetPos(self, parent, 7);
            }
        }
        if (GUILayout.Button("┻", GUILayout.Width(25)))
        {
            RectTransform self = (RectTransform)target;
            if (self.parent != null)
            {
                RectTransform parent = (RectTransform)self.parent;
                SetPos(self, parent, 8);
            }
        }
        if (GUILayout.Button("┛", GUILayout.Width(25)))
        {
            RectTransform self = (RectTransform)target;
            if (self.parent != null)
            {
                RectTransform parent = (RectTransform)self.parent;
                SetPos(self, parent, 9);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        
	}

    /// <summary>
    /// 设置位置
    /// 1 2 3
    /// 4 5 6
    /// 7 8 9
    /// </summary>
    /// <param name="self">操作的UI</param>
    /// <param name="parent">父节点</param>
    /// <param name="type">九宫格位置</param>
    void SetPos(RectTransform self, RectTransform parent, int type)
    {
        //Vector2 max = self.anchorMax;
        //Vector2 min = self.anchorMin;
        Vector2 pos = self.anchoredPosition;

        Vector2 p_middle = parent.sizeDelta*0.5f;
        p_middle.x = p_middle.x * parent.localScale.x; p_middle.y = p_middle.y * parent.localScale.y;//考虑到缩放
        Vector2 s_middle = self.sizeDelta*0.5f;
        s_middle.x = s_middle.x * self.localScale.x; s_middle.y = s_middle.y * self.localScale.y;

        self.anchorMax = Vector2.one * 0.5f;//重置锚点位置为居中
        self.anchorMin = Vector2.one * 0.5f;
        self.anchoredPosition = Vector2.zero;//重置UI位置为正中

        switch (type)
        {
            case 1:
                pos.x = -(p_middle.x - s_middle.x);
                pos.y = (p_middle.y - s_middle.y);
                break;
            case 2:
                pos.x = 0;
                pos.y = (p_middle.y - s_middle.y);
                break;
            case 3:
                pos.x = (p_middle.x - s_middle.x);
                pos.y = (p_middle.y - s_middle.y);
                break;
            case 4:
                pos.x = -(p_middle.x - s_middle.x);
                pos.y = 0;
                break;
            case 5:
                pos.x = 0;
                pos.y = 0;
                break;
            case 6:
                pos.x = (p_middle.x - s_middle.x);
                pos.y = 0;
                break;
            case 7:
                pos.x = -(p_middle.x - s_middle.x);
                pos.y = -(p_middle.y - s_middle.y);
                break;
            case 8:
                pos.x = 0;
                pos.y = -(p_middle.y - s_middle.y);
                break;
            case 9:
                pos.x = (p_middle.x - s_middle.x);
                pos.y = -(p_middle.y - s_middle.y);
                break;
        }
        self.anchoredPosition = pos;
       // self.anchorMax = max;
       // self.anchorMin = min;
    }

}
