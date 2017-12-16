// ========================================================
// Describe  ：InputEventDispatcher
// Author    : Garson
// CreateTime: 2017/12/12
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Garson.Scripts
{
    public class InputEventDispatcher : IInputEventDispatcher
    {
        Dictionary<InputType, InputEvent> events;

        public InputEventDispatcher()
        {
            events = new Dictionary<InputType, InputEvent>();
        }

        public void AddListener(InputType inputType, Callback_0 listener)
        {
            InputEvent _event = GetInputEvent(inputType);
            _event.AddListener(listener);
        }
        public void AddListener(InputType inputType, Callback_1<Vector3> listener)
        {
            InputEvent _event = GetInputEvent(inputType);
            _event.AddListener(listener);
        }

        public void RemoveListener(InputType inputType, Callback_0 listener)
        {
            InputEvent _event = GetInputEvent(inputType);
            _event.RemoveListener(listener);
        }

        public void RemoveListener(InputType inputType, Callback_1<Vector3> listener)
        {
            InputEvent _event = GetInputEvent(inputType);
            _event.RemoveListener(listener);
        }

        public void RemoveListeners(InputType inputType)
        {
            InputEvent _event = GetInputEvent(inputType);
            _event.RemoveListeners();
        }

        public void DispatchEvent(InputType inputType, Vector2 pos)
        {
            InputEvent _event = GetInputEvent(inputType);
            _event.Invoke(pos);
        }
        public void DispatchEvent(InputType inputType)
        {
            InputEvent _event = GetInputEvent(inputType);
            _event.Invoke();
        }

        private InputEvent GetInputEvent(InputType inputType)
        {
            InputEvent _event;
            if (!events.TryGetValue(inputType, out _event))
            {
                _event = new InputEvent();
                events.Add(inputType, _event);
            }
            return _event;
        }
    }

    public class InputEvent
    {
        event Callback_0 callback0;
        event Callback_1<Vector3> callback1;

        public void AddListener(Callback_0 listener)
        {
            callback0 -= listener;
            callback0 += listener;
        }
        public void AddListener(Callback_1<Vector3> listener)
        {
            callback1 -= listener;
            callback1 += listener;
        }

        public void RemoveListener(Callback_0 listener)
        {
            callback0 -= listener;
        }

        public void RemoveListener(Callback_1<Vector3> listener)
        {
            callback1 -= listener;
        }

        public void RemoveListeners()
        {
            callback0 = null;
            callback1 = null;
        }

        public void Invoke(Vector3 pos)
        {
            if (callback0 != null)
                callback0();
            if (callback1 != null)
                callback1(pos);
        }

        public void Invoke()
        {
            if (callback0 != null)
                callback0();
        }
    }

}