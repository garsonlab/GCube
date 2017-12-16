// ========================================================
// Describe  ：ResourceManager
// Author    : Garson
// CreateTime: 2017/12/13
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garson.Scripts
{
    public class ResourceManager : IManager<ResourceManager>
    {

        public T Load<T>(string path) where T : new()
        {

            return new T();
        }

        public void LoadAsync(string path, Callback_1<Object> callback)
        {

        }

        public string LoadString(string path)
        {
            return "";
        }
    }
}