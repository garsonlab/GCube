// ========================================================
// Describe  ：CameraMediator
// Author    : Garson
// CreateTime: 2017/12/18
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using UnityEngine;

namespace Garson.Scripts.View
{
    public class CameraMediator : Mediator
    {
        public new const string NAME = "CameraMediator";
        private CameraController cameraController;

        public CameraMediator() : base(NAME, null)
        {
            Camera camera = GameObject.FindObjectOfType<Camera>();
            cameraController = camera.gameObject.AddComponent<CameraController>();
        }


        public void SetFollowTarget(Transform target, Vector3 offset)
        {
            cameraController.SetFollowTarget(target, offset);
        }


        public void LookAt(Transform transform)
        {
            SetFollowTarget(transform, new Vector3(3,0,-10));
            cameraController.LookAtTarget();
        }
    }
}