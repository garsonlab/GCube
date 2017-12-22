// ========================================================
// Describe  ：Timer
// Author    : Garson
// CreateTime: 2017/12/11
// Version   : v1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : IManager<Timer>, ITimer
{
    Dictionary<uint, TimerData> m_timers;
    List<TimerData> m_toRemove;
    List<TimerData> m_toAdd;
    ObjectPool<TimerData> m_timerPool;
    uint timerId;

    public override void Init()
    {
        m_timers = new Dictionary<uint, TimerData>();
        m_toRemove = new List<TimerData>();
        m_toAdd = new List<TimerData>();
        m_timerPool = new ObjectPool<TimerData>(() => new TimerData(), null, null, null);
        timerId = 0;
    }

    public void OnUpdate()
    {
        if (m_timers.Count > 0)
        {
            var enumerator = m_timers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.IsDone)
                    m_toRemove.Add(enumerator.Current.Value);
                else
                    enumerator.Current.Value.OnUpdate();
            }
            enumerator.Dispose();
        }

        int removeLen = m_toRemove.Count;
        if (removeLen > 0)
        {
            for (int i = 0; i < removeLen; i++)
            {
                TimerData timer = m_toRemove[i];
                m_timers.Remove(timer.Id);
                m_timerPool.Release(timer);
            }
            m_toRemove.Clear();
        }

        int addLen = m_toAdd.Count;
        if (addLen > 0)
        {
            for (int i = 0; i < addLen; i++)
            {
                m_timers.Add(m_toAdd[i].Id, m_toAdd[i]);
            }
            m_toAdd.Clear();
        }
    }



    public TimerData DelayCall(float delay, TimerCallback callback, params object[] parms)
    {
        return RepeatedCall(1, delay, 0, false, callback, parms);
    }

    public TimerData IntervalCall(int times, float interval, TimerCallback callback, params object[] parms)
    {
        return RepeatedCall(times, 0, interval, false, callback, parms);
    }

    public TimerData IntervalDelayCall(int times, float delay, float interval, TimerCallback callback, params object[] parms)
    {
        return RepeatedCall(times, delay, interval, false, callback, parms);
    }
    public TimerData RepeatedCall(int times, float delay, float interval, bool ignoreTimeScale, TimerCallback callback, params object[] parms)
    {
        if (callback == null)
        {
            Debug.Log("Timer Callback can not be null! ");
            return null;
        }

        var timer = m_timerPool.Get();
        timer.Init(timerId++,times, delay, interval, ignoreTimeScale, callback, parms);
        m_toAdd.Add(timer);
        return timer;
    }

    public void SetPause(uint timerId, bool pause)
    {
        TimerData timer = GetTimerData(timerId);
        timer.SetPause(pause);
    }

    public void CancelCallback(uint timerId)
    {
        TimerData timer = GetTimerData(timerId);
        timer.SetPause(true);
        timer.Destroy();
    }

    public TimerData GetTimerData(uint timerId)
    {
        TimerData timer;
        if (m_timers.TryGetValue(timerId, out timer))
            return timer;
        return null;
    }

}

public delegate void TimerCallback(params object[] objs);