using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class MagnetSlot : MonoBehaviour
{
    private Outline _Outline;

    private MoveableMagnet _MagnetInSlot;
    
    void Start()
    {
        _Outline = GetComponent<Outline>();
        _Outline.enabled = false;
    }
    
    public MoveableMagnet GetMagnetInSlot()
    {
        return _MagnetInSlot;
    }

    public void SetMagnetInSlot(MoveableMagnet inMagnet, bool inDontCallMagnet = false)
    {
        _MagnetInSlot = inMagnet;

        if (_MagnetInSlot != null &&
            inDontCallMagnet == false)
        {
            _MagnetInSlot.SetSlotHoldingMagnet(this, inDontCallSlot: true);
            _MagnetInSlot.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
        }
    }

    public bool IsSlotFull()
    {
        return _MagnetInSlot != null;
    }
    
    public void SetSelectState(bool inSelectState)
    {
        _Outline.enabled = inSelectState;
    }

    public bool DoesContainCorrectIndex(int inSlotIndex)
    {
        if (_MagnetInSlot == null)
        {
            return false;
        }

        return _MagnetInSlot.Index == inSlotIndex;
    }

    public void Reset()
    {
        if (_MagnetInSlot != null)
        {
            _MagnetInSlot.Reset();
        }
        
        _MagnetInSlot = null;

        if (_Outline != null)
        {
            _Outline.enabled = false;
        }
    }
}
