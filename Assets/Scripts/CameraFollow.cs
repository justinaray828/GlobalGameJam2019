using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Follow;
    
    public bool MaintainX;
    public bool MaintainY;
    public bool MaintainZ;
    
    public bool RelativeX;
    public bool RelativeY;
    public bool RelativeZ;

    private float _RelativeX = 0f;
    private float _RelativeY = 0f;
    private float _RelativeZ = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (RelativeX == true)
        {
            _RelativeX = transform.position.x - Follow.position.x;
        }
        
        if (RelativeY == true)
        {
            _RelativeY = transform.position.y - Follow.position.y;
        }
        
        if (RelativeZ == true)
        {
            _RelativeZ = transform.position.z - Follow.position.z;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float followX = transform.position.x;
        if (MaintainX == false)
        {
            followX = Follow.position.x;
        }
        
        float followY = transform.position.y;
        if (MaintainY == false)
        {
            followY = Follow.position.y;
        }
        
        float followZ = transform.position.z;
        if (MaintainZ == false)
        {
            followZ = Follow.position.z;
        }
        
        transform.position = new Vector3(followX + _RelativeX, followY + _RelativeY, followZ + _RelativeZ);
    }
}
