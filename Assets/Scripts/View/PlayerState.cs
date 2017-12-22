// ========================================================
// Describe  ：PlayerState
// Author    : Garson
// CreateTime: 2017/12/22
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garson.Scripts.View
{
    public class PlayerState
    {
        RoleState m_RoleState = RoleState.None;

        public PlayerState()
        {
            
        }



    }

    public enum RoleState
    {
        None = 0,
        Default = 1,
        Walk = 2,
        /// <summary>
        /// 忧愁，前方高
        /// </summary>
        Sorrow = 3,
        /// <summary>
        /// 恐惧，前方低
        /// </summary>
        Fear = 4,
        Happy = 5,
        Sad = 6
    }
}
