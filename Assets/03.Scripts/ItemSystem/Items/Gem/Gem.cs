using FlyRabbit.EventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class Gem : Item
{
    [SerializeField]
    private Collider2D m_PlayerIgnoreCollider;
    private Animator m_Animator;

    private int m_BlingBlingAnimationId;
    private ContactFilter2D m_ContactFilter;
    private readonly RaycastHit2D[] m_RaycastHits = new RaycastHit2D[2];
    private readonly float m_FallingVelocity = 6.5f;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();

        m_BlingBlingAnimationId = Animator.StringToHash("BlingBling");

        m_ContactFilter = new ContactFilter2D()
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = ~((1 << 6) | (1 << 7))
        };
    }

    private void Start()
    {
        StartCoroutine(BlingBling());
    }

    private void FixedUpdate()
    {
        if (IsFrozen == true)
        {
            return;
        }
        Falling();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Fox>() == null)
        {
            return;
        }

        MapManager.Instance.RemoveItem(this);
        AudioManager.PlaySFX(AudioPath.SFX.Collection);
    }


    private IEnumerator BlingBling()
    {
        while (true)
        {
            m_Animator.Play(m_BlingBlingAnimationId);
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    /// <summary>检测并下落</summary>
    private void Falling()
    {
        int count = m_PlayerIgnoreCollider.Raycast(Vector2.down, m_ContactFilter, m_RaycastHits);
        if (count <= 0)
        {
            return;
        }
        Vector3 destination = new Vector3(transform.position.x, m_RaycastHits[0].point.y + 0.5f, 0);
        //确保不会往上“下落”
        if (destination.y >= transform.position.y)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, destination, m_FallingVelocity * Time.fixedDeltaTime);
    }
}
