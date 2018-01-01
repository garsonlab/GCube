// ========================================================
// Describe  ：PointLine
// Author    : Garson
// CreateTime: 2017/12/27
// Version   : v1.0
// ========================================================
// ========================================================
// Describe  ：PointLine
// Author    : Garson
// CreateTime: 2017/12/18
// Version   : v1.0
// ========================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garson.Scripts.View
{
    public class PointLine
    {
        private GameObject m_obj;
        private LineRenderer m_line;
        private List<Vector3> m_vectors;
        private int m_count;

        public PointLine()
        {
            m_obj = GameObject.Find("LineEffect");
            if(m_obj == null)
                m_obj = new GameObject("LineEffect");
            m_line = m_obj.GetComponent<LineRenderer>();
            if (m_line == null)
                m_line = m_obj.AddComponent<LineRenderer>();

            m_line.startWidth = 0.2f;
            m_line.endWidth = 0.2f;

            m_vectors = new List<Vector3>();
            m_count = 0;
        }


        public void Reset()
        {
            m_vectors.Clear();
            m_line.positionCount = 0;
        }

        public void SetPositions(List<Point> points)
        {
            int count = points.Count;
            m_vectors.Clear();
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = points[i].transform.position;
                pos.z = -1;
                
                m_vectors.Add(pos);
                if (i == count - 1)//需要多加一个当前位置点
                    m_vectors.Add(pos);
            }
            m_line.positionCount = m_vectors.Count;
            m_line.SetPositions(m_vectors.ToArray());
        }


        public void UpdateTemPos(Vector3 pos)
        {
            if(m_vectors.Count < 1)
                return;
            pos -= Vector3.one*0.5f;
            pos.z = -1;
            m_line.SetPosition(m_vectors.Count-1, pos);
        }

    }
}
