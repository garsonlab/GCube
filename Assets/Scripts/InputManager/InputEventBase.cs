// ========================================================
// Describe  ：InputEventBase
// Author    : Garson
// CreateTime: 2017/12/12
// Version   : v1.0
// ========================================================

using UnityEngine;

namespace Garson.Scripts
{
    public abstract class InputEventBase
    {
        protected bool m_isTouchOnUI;
        protected TouchStatus m_touchStatus; //状态
        protected Vector3 m_pointPos; //点击位置
        protected Vector3 m_curPos; //当前点击位置
        protected Vector3 m_scaleDelata; //缩放位置比例

        protected float m_moveTolerace; //移动容忍，超出长度表示移动
        protected float m_longPressSpan; //长按间隔
        protected float m_longPressTimer; //长按计时器
        protected float m_moveDis; //移动距离


        public InputEventBase()
        {
            m_moveTolerace = 30f;
            m_longPressSpan = 0.5f;
            Reset();
        }

        /// <summary>
        /// 只能由InputManager调用的更新
        /// </summary>
        public virtual void OnUpdate()
        {
            if (!InputManager.Instance.IsActive)
                return;

        }

        /// <summary>
        /// 设置是否可用
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active)
        {
            if (!active)
                Reset();
        }

        /// <summary>
        /// 是否点击在UI上
        /// </summary>
        public bool IsTouchOnUI
        {
            get { return m_isTouchOnUI; }
        }


        /// <summary>
        /// 重置状态
        /// </summary>
        protected virtual void Reset()
        {
            m_isTouchOnUI = false;
            m_touchStatus = TouchStatus.None;
            m_longPressTimer = 0;
        }

        /// <summary>
        /// 开始点击
        /// </summary>
        /// <param name="pointPos"></param>
        protected virtual void OnTouchStart(Vector3 curPos)
        {
            m_touchStatus = TouchStatus.Touch;
            m_pointPos = curPos;
            m_curPos = curPos;
            m_longPressTimer = 0;

            InputManager.Instance.DispatchEvent(InputType.OnTouchBegin, m_pointPos);
        }

        /// <summary>
        /// 点击中
        /// </summary>
        /// <param name="pointPos"></param>
        protected virtual void OnTouch(Vector3 curPos)
        {
            m_longPressTimer += Time.deltaTime;
            m_curPos = curPos;

            if (m_touchStatus != TouchStatus.Moving)
            {
                m_moveDis = Vector2.Distance(m_pointPos, m_curPos);
                if (m_moveDis >= m_moveTolerace)
                {
                    m_touchStatus = TouchStatus.Moving;
                    InputManager.Instance.DispatchEvent(InputType.OnMoveBegin, m_curPos);
                }

                if (m_touchStatus == TouchStatus.Touch && m_longPressTimer >= m_longPressSpan)
                {
                    m_touchStatus = TouchStatus.LongPress;
                    InputManager.Instance.DispatchEvent(InputType.OnLongPress, m_curPos);
                }
            }
            else
            {
                InputManager.Instance.DispatchEvent(InputType.OnMove, m_curPos);
            }
        }

        protected virtual void OnTouchEnd(Vector3 curPos)
        {
            m_curPos = curPos;

            if (m_touchStatus == TouchStatus.Moving)
                InputManager.Instance.DispatchEvent(InputType.OnMoveEnd, m_curPos);
            if (m_touchStatus == TouchStatus.LongPress)
                InputManager.Instance.DispatchEvent(InputType.OnEndLongPress, m_curPos);
            if (m_touchStatus == TouchStatus.Touch)
                InputManager.Instance.DispatchEvent(InputType.OnClick, m_curPos);

            InputManager.Instance.DispatchEvent(InputType.OnTouchEnd, m_curPos);

            m_touchStatus = TouchStatus.None;
            m_isTouchOnUI = false;
        }

        protected virtual void OnScale()
        {
            
        }

        protected virtual void CheckOnUI(int fingerId)
        {
            if(UnityEngine.EventSystems.EventSystem.current == null)
                return;
            if(fingerId < 0)
                m_isTouchOnUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            else
                m_isTouchOnUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(fingerId);
        }

        /// <summary>
        /// 手势状态
        /// </summary>
        protected enum TouchStatus
        {
            /// <summary>
            /// 无状态
            /// </summary>
            None,

            /// <summary>
            /// 刚刚按下
            /// </summary>
            Touch,

            /// <summary>
            /// 长按
            /// </summary>
            LongPress,

            /// <summary>
            /// 移动
            /// </summary>
            Moving,
        }
    }


    public enum InputType
    {
        /// <summary>
        /// 触摸开始
        /// </summary>
        OnTouchBegin = 0,
        /// <summary>
        /// 长按
        /// </summary>
        OnLongPress,
        /// <summary>
        /// 开始移动
        /// </summary>
        OnMoveBegin,
        /// <summary>
        /// 移动中
        /// </summary>
        OnMove,
        /// <summary>
        /// 移动结束
        /// </summary>
        OnMoveEnd,
        /// <summary>
        /// 点击
        /// </summary>
        OnClick,
        /// <summary>
        /// 触摸结束
        /// </summary>
        OnTouchEnd,
        /// <summary>
        /// 缩放，回调x,y表示缩放初始中心点，z为当前缩放增量
        /// </summary>
        OnScale,
        /// <summary>
        /// 手指抬起时仍未长按状态，和OnClick不能同时响应
        /// </summary>
        OnEndLongPress,
    }
}