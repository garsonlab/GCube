// ========================================================
// Describe  ：InputManager
// Author    : Garson
// CreateTime: 2017/12/11
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garson.Scripts
{
    public class InputManager : IManager<InputManager>, IInputEventDispatcher
    {
        InputEventDispatcher m_eventDispatcher;
        InputEventBase m_inputEvent;
        bool m_isActive;

        public override void Init()
        {
            m_isActive = true;
            m_eventDispatcher = new InputEventDispatcher();
            if (Application.isMobilePlatform)
                m_inputEvent = new MobileInput();
            else
                m_inputEvent = new EditorInput();
            Timer.Instance.RepeatedCall(-1, 0, 0, false, OnUpdate);
        }



        void OnUpdate(params object[] objs)
        {
            m_inputEvent.OnUpdate();
        }


        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsActive
        {
            get { return m_isActive; }
            set
            {
                m_isActive = value;
                m_inputEvent.SetActive(value);
            }
        }
        /// <summary>
        /// 是否点击在ui上
        /// </summary>
        public bool IsTouchOnUI
        {
            get { return m_inputEvent.IsTouchOnUI; }
        }

        internal void DispatchEvent(InputType inputType, Vector2 pos)
        {
            m_eventDispatcher.DispatchEvent(inputType, pos);
        }

        public void AddListener(InputType inputType, Callback_0 listener)
        {
            m_eventDispatcher.AddListener(inputType, listener);
        }
        public void AddListener(InputType inputType, Callback_1<Vector3> listener)
        {
            m_eventDispatcher.AddListener(inputType, listener);
        }

        public void RemoveListener(InputType inputType, Callback_0 listener)
        {
            m_eventDispatcher.RemoveListener(inputType, listener);
        }

        public void RemoveListener(InputType inputType, Callback_1<Vector3> listener)
        {
            m_eventDispatcher.RemoveListener(inputType, listener);
        }

        public void RemoveListeners(InputType inputType)
        {
            m_eventDispatcher.RemoveListeners(inputType);
        }
    }
}