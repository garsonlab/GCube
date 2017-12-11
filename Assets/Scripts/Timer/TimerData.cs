// ========================================================
// Describe  ：TimerData
// Author    : Garson
// CreateTime: 2017/12/11
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerData
{
    private uint index;
    public int times;
    public float delay;
    public float interval;
    public bool ignoreTimeScale;
    public TimerCallback callback;
    public object[] parms;

    private bool m_isPause;
    private bool m_isDone;
    public uint Id { get { return index; } }
    public bool IsDone { get { return m_isDone; } }

    private float m_curTimer;
    private bool m_inDelay;

    public TimerData(uint index, int times, float delay, float interval, bool ignoreTimeScale, TimerCallback callback, params object[] parms)
    {
        this.index = index;
        this.times = times;
        this.delay = delay;
        this.interval = interval;
        this.ignoreTimeScale = ignoreTimeScale;
        this.callback = callback;
        this.parms = parms;
        
        this.m_curTimer = 0;
        this.m_inDelay = true;
        this.m_isPause = false;
        this.m_isDone = false;
    }


    public void OnUpdate()
    {
        if(m_isPause || m_isDone)
            return;

        m_curTimer += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

        if (m_inDelay)
        {
            if (m_curTimer >= delay)
            {
                m_curTimer -= delay > 0 ? delay : 0;
                m_inDelay = false;
            }
            else
            {
                return;
            }
        }


        if (m_curTimer >= interval)
        {
            m_curTimer -= interval;
            if (callback != null)
                callback(parms);

            if (times > 0)
                times--;
            if (times == 0)
                m_isDone = true;
        }
    }


    public void SetPause(bool pause)
    {
        this.m_isPause = pause;
    }


    public void Destroy()
    {
        this.m_isDone = true;
    }
}
