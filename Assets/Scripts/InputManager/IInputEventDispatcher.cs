// ========================================================
// Describe  ：IInputEventDispatcher
// Author    : Garson
// CreateTime: 2017/12/12
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garson.Scripts
{
    public interface IInputEventDispatcher
    {
        /// <summary>
        /// 添加输入监听
        /// </summary>
        /// <param name="inputType">输入类型</param>
        /// <param name="listener">回调函数</param>
        void AddListener(InputType inputType, Callback_0 listener);
        /// <summary>
        /// 添加输入监听
        /// </summary>
        /// <param name="inputType">输入类型</param>
        /// <param name="listener">回调函数</param>
        void AddListener(InputType inputType, Callback_1<Vector3> listener);
        /// <summary>
        /// 移除输入监听
        /// </summary>
        /// <param name="inputType">输入类型</param>
        /// <param name="listener">回调函数</param>
        void RemoveListener(InputType inputType, Callback_0 listener);
        /// <summary>
        /// 移除输入监听
        /// </summary>
        /// <param name="inputType">输入类型</param>
        /// <param name="listener">回调函数</param>
        void RemoveListener(InputType inputType, Callback_1<Vector3> listener);
        /// <summary>
        /// 移除所有监听
        /// </summary>
        /// <param name="inputType">输入类型</param>
        void RemoveListeners(InputType inputType);
    }
}
