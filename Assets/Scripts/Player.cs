using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] bool m_Invincible = false;
    public bool IsInvincible { get { return m_Invincible; } }
    const float INVINCIBLE_INTERVAL = 10;
    float m_InvincibleTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_InvincibleTimer > INVINCIBLE_INTERVAL)
        {
            m_Invincible = false;
            this.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            m_InvincibleTimer += Time.deltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        var pill = collision.gameObject.GetComponent<Pill>();
        if(pill != null)
        {
            Destroy(pill.gameObject);
            m_Invincible = true;
            this.GetComponent<MeshRenderer>().material.color = Color.black;
            m_InvincibleTimer = 0;
        }
        var ghost = collision.gameObject.GetComponent<Ghost>();
        if(ghost != null)
        {
            if (m_Invincible)
            {
                GameObject.Destroy(ghost.gameObject);
            }
            else
            {
                GameObject.Destroy(this.gameObject);
                Debug.Log("Game over!");
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }
}
