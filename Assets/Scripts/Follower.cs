using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    [SerializeField] Player m_Player;
    [SerializeField] NavMeshAgent m_Agent;
    const float FOLLOW_DISTANCE = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NavMeshHit navMeshHit;
        Vector3 RandomPoint = m_Player.transform.position + Random.insideUnitSphere * FOLLOW_DISTANCE;
        if (NavMesh.SamplePosition(RandomPoint, out navMeshHit, FOLLOW_DISTANCE, -1))
        {
            m_Agent.SetDestination(navMeshHit.position);
        }
    }
}
