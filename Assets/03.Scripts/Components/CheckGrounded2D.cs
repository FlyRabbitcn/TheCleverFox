using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class CheckGrounded2D : MonoBehaviour
{
    [SerializeField]
    [Range(0,90)]
    private float m_MaxGroundAngle = 45f;//多少度以下的斜坡会被视作地面

    private float m_MaxGroundAngleCos;

    private Vector2 m_GroundNormal;//地面法线

    /// <summary>
    /// 接触地面吗？
    /// </summary>
    public bool Grounded { get; private set; }
    private void Check(Collision2D collision)
    {
        Vector3 normal = Vector2.zero;
        bool TouchGround = false;
        for (int i = 0; i < collision.contactCount; i++)
        {
            normal = collision.GetContact(i).normal;
            if (normal.y >= m_MaxGroundAngleCos)
            {
                TouchGround = true;
                break;
            }
        }
        if (TouchGround)
        {
            m_GroundNormal = normal;
            Grounded = true;
        }
        else
        {
            m_GroundNormal = Vector2.zero;
        }
    }
    #region Unity消息
    private void Awake()
    {
        m_MaxGroundAngleCos = Mathf.Cos(m_MaxGroundAngle * Mathf.Deg2Rad);
    }
    private void FixedUpdate()
    {
        if (m_GroundNormal == Vector2.zero)
        {
            Grounded = false;
        }
        m_GroundNormal = Vector2.zero;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Check(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Check(collision);
    }
    #endregion
}