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
    Dictionary<uint, TimerData> m_Timers;
    List<uint> m_Removes;
    Queue<TimerData> m_TimerCache; 
    uint timerId;

    public override void Init()
    {
        m_Timers = new Dictionary<uint, TimerData>();
        m_Removes = new List<uint>();
        m_TimerCache = new Queue<TimerData>();
        timerId = 0;
    }

    public void OnUpdate()
    {
        var enumerator = m_Timers.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.OnUpdate();
            if(enumerator.Current.Value.IsDone)
                m_Removes.Add(enumerator.Current.Value.Id);
        }

        for (int i = 0; i < m_Removes.Count; i++)
        {
            var timer = m_Timers[m_Removes[i]];
            m_Timers.Remove(m_Removes[i]);
            m_TimerCache.Enqueue(timer);
        }
        m_Removes.Clear();
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
        var timer = GetAvailableTimerData();
        timer.Init(timerId++,times, delay, interval, ignoreTimeScale, callback, parms);
        m_Timers.Add(timer.Id, timer);
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
        if (m_Timers.TryGetValue(timerId, out timer))
            return timer;
        return null;
    }

    private TimerData GetAvailableTimerData()
    {
        TimerData timer = m_TimerCache.Count > 0 ? m_TimerCache.Dequeue() : new TimerData();
        return timer;
    }
}

public delegate void TimerCallback(params object[] objs);