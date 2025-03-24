using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
     public Transform player;
    public float Xaxis = 0f;
    public float Yaxis = 0f;
    public float Zaxis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player){
        transform.position = player.transform.position + new Vector3(Xaxis,Yaxis,Zaxis);
        }
    }
    
}
