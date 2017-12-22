// ========================================================
// Describe  ：GUIManager
// Author    : Garson
// CreateTime: 2017/12/17
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garson.Scripts
{
    public class GUIManager : IManager<GUIManager>
    {
        Dictionary<string, Callback_0> m_guiDrawers;

        public override void Init()
        {
            m_guiDrawers = new Dictionary<string, Callback_0>();
        }

        public void AddDrawer(string key, Callback_0 callback)
        {
            if (m_guiDrawers.ContainsKey(key))
            {
                m_guiDrawers[key] = callback;
            }
            else
            {
                m_guiDrawers.Add(key, callback);
            }
        }

        public void RemoveDrawer(string key)
        {
            if (m_guiDrawers.ContainsKey(key))
                m_guiDrawers.Remove(key);
        }

        public void Drawing()
        {
            var enumerator = m_guiDrawers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value();
            }
        }

        public override void Clear()
        {
            if (m_guiDrawers != null)
                m_guiDrawers.Clear();
        }
    }
}