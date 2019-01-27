using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseImageInput : MonoBehaviour
{
    private System.Action<CloseImageInput> _OnClose;
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) 
        {
            if (_OnClose != null)
            {
                _OnClose(this);
            }
            Destroy(gameObject);
        }
    }

    public void RegisterOnClose(System.Action<CloseImageInput> onClose)
    {
        if (onClose != null)
        {
            _OnClose += onClose;
        }
    }

    public void UnregisterOnClose(System.Action<CloseImageInput> onClose)
    {
        if (onClose != null)
        {
            _OnClose -= onClose;
        }
    }
}
