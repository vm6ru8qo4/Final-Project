using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    public Camera Cam;
    public NavMeshAgent Agent;
    [SerializeField] GameObject m_Prepill;
    const float PILL_INTERVAL = 10f;
    const float PILL_DISTANCE = 5f;
    float m_PillTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                Agent.SetDestination(hit.point);
            }
        }
        //UpdatePill();
    }
    void UpdatePill()
    {
        if(m_PillTimer > PILL_INTERVAL)
        {
            m_PillTimer = 0;
            NavMeshHit navMeshHit;
            while(!NavMesh.SamplePosition(
                Random.insideUnitSphere * Random.Range(0,PILL_DISTANCE),out navMeshHit,PILL_DISTANCE, -1)) { }
            Vector3 pos = navMeshHit.position;
            pos.y = 1.25f;
            GameObject.Instantiate(m_Prepill).transform.position = pos;
        }
        else
        {
            m_PillTimer += Time.deltaTime;
        }
    }
}
