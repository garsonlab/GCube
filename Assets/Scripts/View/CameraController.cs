// ========================================================
// Describe  ：CameraController
// Author    : Garson
// CreateTime: 2017/12/18
// Version   : v1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera m_camera;
    private Transform m_cameraTrans;
    private float m_cameraSize;
    [SerializeField]
    private Transform m_followTarget;
    [SerializeField]
    private Vector3 m_offset;

    private Vector3 m_cameraPos;
    //private Vector3 m_halfSize;
    //private Vector3 m_viewSize;

    void Awake()
    {
        m_camera = GetComponent<Camera>();
        m_cameraSize = m_camera.orthographicSize;
        m_cameraTrans = m_camera.transform;
    }

    void LateUpdate()
    {
        if(m_followTarget == null)
            return;

        m_cameraPos = GetTargetPos();
        if (Mathf.Abs(m_cameraPos.x - m_cameraTrans.position.x) >= 0.05f || Mathf.Abs(m_cameraPos.y - m_cameraTrans.position.y) >= 0.05f || m_cameraSize != m_camera.orthographicSize)
        {
            m_cameraSize = m_camera.orthographicSize;
            UpdateCameraPosition(m_cameraPos, true);
        }
    }


    private Vector3 GetTargetPos()
    {
        //m_halfSize.y = m_camera.orthographicSize;
        //m_halfSize.x = m_halfSize.y * (Screen.width * 1.0f / Screen.height);
        //m_viewSize = m_halfSize * 2;

       // Vector3 position = new Vector3(m_offset.x, m_offset.y, m_cameraTrans.position.z);
        if (m_followTarget != null)
        {
           // position = m_followTarget.position + m_offset;
                //new Vector3(m_followTarget.position.x, m_followTarget.position.y + m_offset, m_cameraTrans.position.z);
            return m_followTarget.position + m_offset;
        }

        //if (position.x < m_halfSize.x)
        //{
        //    position.x = m_halfSize.x;
        //}
        //else if (position.x > closeMapSize.x - halfSizeClose.x)//做场景限制
        //{
        //    position.x = closeMapSize.x - halfSizeClose.x;
        //}

        //if (position.y < m_halfSize.y)
        //{
        //    position.y = m_halfSize.y;
        //}
        //else if (position.y > closeMapSize.y - halfSizeClose.y)
        //{
        //    position.y = closeMapSize.y - halfSizeClose.y;
        //}

        return m_offset;
        //return position;
    }


    public void SetFollowTarget(Transform target, Vector3 offset)
    {
        this.m_followTarget = target;
        this.m_offset = offset;
        //m_offset.z = -10;
    }

    private void UpdateCameraPosition(Vector3 position, bool animate)
    {
        if (animate)
        {
            m_cameraTrans.position = Vector3.Lerp(m_cameraTrans.position, position, 0.1f);
        }
        else
        {
            m_cameraTrans.position = position;
        }

        //Vector2 percentage = Vector2.zero;
        //float tmpX = closeMapSize.x - m_viewSize.x;
        //if (tmpX == 0)
        //{
        //    percentage.x = 0;
        //}
        //else
        //{
        //    percentage.x = (m_cameraTrans.position.x - m_halfSize.x) / tmpX;
        //}

        //float tmpY = closeMapSize.y - m_viewSize.y;
        //if (tmpY != 0)
        //{
        //    percentage.y = (m_cameraTrans.position.y - m_halfSize.y) / tmpY;
        //}

        //vista.position = new Vector3(halfSizeVista.x + percentage.x * (vistaMapSize.x - viewSizeVista.x), halfSizeVista.y + percentage.y * (vistaMapSize.y - viewSizeVista.y));

        //CameraMove();
    }

    public void LookAtTarget()
    {
        m_cameraPos = GetTargetPos();
        UpdateCameraPosition(m_cameraPos, false);
    }

}
