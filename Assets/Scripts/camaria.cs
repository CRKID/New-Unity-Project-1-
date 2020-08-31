using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camaria : MonoBehaviour
{
    // Start is called before the first frame update
    public float MinX;
    public float MinY;
    public Transform targettrasform;
    //
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {

        float x = targettrasform.position.x;
        float y = targettrasform.position.y;
        Vector3 vector3 = new Vector3(x,y, transform.position.z);
        transform.position = vector3;
    }
}
