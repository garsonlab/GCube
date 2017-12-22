// ========================================================
// Describe  ：ObjectPool
// Author    : Garson
// CreateTime: 2017/12/18
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> 
{
    readonly List<T> m_List = new List<T>();
    readonly Stack<T> m_Stack = new Stack<T>();
    readonly T_Callback<T> m_Creater; 
    readonly Callback_1<T> m_OnGet;
    readonly Callback_1<T> m_OnRelease;
    readonly Callback_1<T> m_OnClear;

    public int countAll { get { return m_List.Count; }}
    public int countActive { get { return countAll - countInactive; } }
    public int countInactive { get { return m_Stack.Count; } }

    public ObjectPool(T_Callback<T> creater, Callback_1<T> onGet, Callback_1<T> onRelease, Callback_1<T> onClear)
    {
        this.m_Creater = creater;
        this.m_OnGet = onGet;
        this.m_OnRelease = onRelease;
        this.m_OnClear = onClear;
    }


    public T Get()
    {
        if (m_Creater == null)
            throw new System.NotImplementedException("Internal error. The Creater function is not assigned.");

        T element;
        if (m_Stack.Count == 0)
        {
            element = m_Creater();
            m_List.Add(element);
        }
        else
        {
            element = m_Stack.Pop();
        }
        if (m_OnGet != null)
            m_OnGet(element);
        return element;
    }


    public void Release(T element)
    {

        if (!m_List.Contains(element))
        {
            Debug.LogError("Internal error. Trying to destroy object that is not in the pool.");
            return;
        }

        if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
        {
            Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            return;
        }

        if (m_OnRelease != null)
            m_OnRelease(element);
        m_Stack.Push(element);
    }

    public void Clear()
    {
        var enumerator = m_List.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (m_OnRelease != null)
                m_OnClear(enumerator.Current);
        }

        m_List.Clear();
        m_Stack.Clear();
    }

}
