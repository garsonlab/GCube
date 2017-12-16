// ========================================================
// Describe  ：IManager
// Author    : Garson
// CreateTime: 2017/12/13
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IManager<T> where T : new()
{
    static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
                instance = new T();
            return instance;
        }
    }

    public virtual void Init() { }
    public virtual void Clear() { }
}
