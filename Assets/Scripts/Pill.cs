using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.Invoke("DestroySelf", 5f);
    }

    // Update is called once per frame
    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
