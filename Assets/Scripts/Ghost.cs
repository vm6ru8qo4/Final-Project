using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    [SerializeField] Player m_Player;
    [SerializeField] NavMeshAgent m_Agent;
    const float WANDER_INTERVAL = 1f;
    const float WANDER_DISTANCE = 5f;
    float m_WanderTimer = -1;

    const float TRACKING_RANGE = 64f;

    const float ESCAPE_INTERVAL = 1f;
    const float ESCAPE_DISTANCE = 5f;
    float m_EscapeTimer = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((m_Player.transform.position - this.transform.position).sqrMagnitude < TRACKING_RANGE)
        {
            if (m_Player.IsInvincible)
            {
                //Runaway
                if (m_EscapeTimer == -1 || m_EscapeTimer > ESCAPE_INTERVAL)
                {
                    Escape();
                }
                else
                {
                    m_EscapeTimer += Time.deltaTime;
                }
            }
            else
            {
                m_Agent.SetDestination(m_Player.transform.position);
            }
            
            m_WanderTimer = -1;
        }
        else
        {
            if (m_WanderTimer == -1 || m_WanderTimer > WANDER_INTERVAL)
            {
                Wander();
            }
            else
            {
                m_WanderTimer += Time.deltaTime;
            }
            m_EscapeTimer = -1;
        }
        
    }

    void Wander()
    {
        NavMeshHit navMeshHit;
        Vector3 RandomPoint = this.transform.position + Random.insideUnitSphere * WANDER_DISTANCE;
        if(NavMesh.SamplePosition(RandomPoint, out navMeshHit, WANDER_DISTANCE, -1))
        {
            m_Agent.SetDestination(navMeshHit.position);
            m_WanderTimer = 0;
        }
        else
        {
            m_WanderTimer = -1;//re wander
        }
    }
    void Escape()
    {
        NavMeshHit navMeshHit;
        Vector3 Point = this.transform.position + (this.transform.position - m_Player.transform.position).normalized*ESCAPE_DISTANCE;
        if (NavMesh.SamplePosition(Point, out navMeshHit, ESCAPE_DISTANCE, -1))
        {
            m_Agent.SetDestination(navMeshHit.position);
            m_EscapeTimer = 0;
        }
        else
        {
            m_EscapeTimer = -1;//re wander
        }
    }
}
