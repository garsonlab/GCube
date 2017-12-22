// ========================================================
// Describe  ：PlayerMediator
// Author    : Garson
// CreateTime: 2017/12/18
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

namespace Garson.Scripts.View
{
    public class PlayerMediator : Mediator
    {
        public new const string NAME = "PlayerMediator";
        public const string ANIMATION_DEFAULT = "default";
        public const string ANIMATION_SORROW = "sorrow";//忧愁
        public const string ANIMATION_FEAR = "fear";//害怕
        public const string ANIMATION_HAPPY = "happy";
        public const string ANIMATION_SAD = "sad";

        public Transform Role { get; private set; }
        public PlayerMediator() : base(NAME, null){}

        private LevelProxy proxy;
        private Vector3 rolePos;//x当前第几列， y高度， z下一列比本列（1，高， -1低， 0相等）
        private Animation animation;

        public override void OnRegister()
        {
            base.OnRegister();
            proxy = AppFacade.Instance.RetrieveProxy(LevelProxy.NAME) as LevelProxy;
        }

        public override IList<string> ListNotificationInterests()
        {
            return new List<string>()
            {
                MsgType.PLAYER_MOVE
            };
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case MsgType.PLAYER_MOVE:
                    StartMove();
                    break;
            }
        }

        /// <summary>
        /// 创建人物
        /// </summary>
        public void CreateRole()
        {
            if (Role == null)
            {
                Role = GameObject.CreatePrimitive(PrimitiveType.Capsule).transform;
            }

            rolePos = proxy.GetRolePos();
            Role.position = new Vector3(rolePos.x, rolePos.y, 0);// - new Vector3(3, 3, 0);
            PlayAnimation(ANIMATION_DEFAULT);
        }


        private void StartMove()
        {
            Vector3 next = proxy.GetNextPos();//获取下一个可移动的点
            if (next.x > 0)//如果下一列是可移动的
            {
                next.z = 0;
                MoveTo(next, true);//移动到下一列
                return;
            }

            if (next.z > 0)
            {
                PlayAnimation(ANIMATION_SORROW);
            }
            else
            {
                PlayAnimation(ANIMATION_FEAR);
            }
            proxy.GenegrateBottom();//每次移动后添加底部增加
        }

        private void MoveTo(Vector3 pos, bool animate)
        {
            Timer.Instance.DelayCall(1, objs =>
            {
                Role.position = pos;//-new Vector3(3,3,0);
                //移动结束
                proxy.RecycleCubes();//回收多余的并产生新的列
                StartMove();//继续判断移动
            });
           
        }

        private void PlayAnimation(int ani)
        {
            switch (ani)
            {
                case 1:
                    PlayAnimation(ANIMATION_FEAR);
                    break;
                case -1:
                    PlayAnimation(ANIMATION_SORROW);
                    break;
                default:
                    PlayAnimation(ANIMATION_DEFAULT);
                    break;
            }
        }

        public void PlayAnimation(string clip)
        {
            if (animation != null)
            {
                animation.CrossFadeQueued(clip, 0.2f);
            }
        }

    }
}