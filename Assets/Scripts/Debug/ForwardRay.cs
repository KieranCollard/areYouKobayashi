using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.magenta);
    }
}
