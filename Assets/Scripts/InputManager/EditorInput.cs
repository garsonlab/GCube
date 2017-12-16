// ========================================================
// Describe  ：EditorInput
// Author    : Garson
// CreateTime: 2017/12/12
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garson.Scripts
{
    /// <summary>
    /// 鼠标操作，鼠标左键的8种行为；缩放使用中央滚轮，z表示缩放增量。
    /// </summary>
    public class EditorInput : InputEventBase
    {
        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Input.GetMouseButtonDown(0))
            {
                CheckOnUI(-1);
                OnTouchStart(Input.mousePosition);
            }

            if (m_touchStatus != TouchStatus.None && Input.GetMouseButton(0))
            {
                OnTouch(Input.mousePosition);
            }

            if (m_touchStatus != TouchStatus.None && Input.GetMouseButtonUp(0))
            {
                OnTouchEnd(Input.mousePosition);
            }
            OnScale();
        }

        protected override void OnScale()
        {
            m_scaleDelata = Input.mousePosition;
            m_scaleDelata.z = Input.GetAxis("Mouse ScrollWheel");
            if(m_scaleDelata.z != 0)
                InputManager.Instance.DispatchEvent(InputType.OnScale, m_scaleDelata);
        }
    }
}
