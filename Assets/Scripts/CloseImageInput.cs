using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseImageInput : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Debug.Log(!Input.GetMouseButtonDown(0));
        if (Input.anyKey && !Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) 
        {
            Destroy(gameObject);
        }
    }
}
