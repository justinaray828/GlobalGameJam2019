using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class MoveableMagnet : MonoBehaviour
{
    public int Index;
    
    private Outline _Outline;
    private MagnetSlot _SlotHoldingMagnet;

    private Vector3 _StartingPosition;
    
    void Start()
    {
        _Outline = GetComponent<Outline>();
        _Outline.enabled = false;
        _StartingPosition = transform.position;
    }
    
    public MagnetSlot GetHoldingSlot()
    {
        return _SlotHoldingMagnet;
    }
    
    public void SetSlotHoldingMagnet(MagnetSlot inSlot, bool inDontCallSlot = false)
    {
        _SlotHoldingMagnet = inSlot;

        if (_SlotHoldingMagnet != null &&
            inDontCallSlot == false)
        {
            _SlotHoldingMagnet.SetMagnetInSlot(this, inDontCallMagnet: true);
            transform.position = _SlotHoldingMagnet.transform.position;
            transform.position = new Vector3(_SlotHoldingMagnet.transform.position.x, _SlotHoldingMagnet.transform.position.y, _SlotHoldingMagnet.transform.position.z - 0.1f);
        }
        else
        {
            transform.position = _StartingPosition;
        }
    }

    public bool IsInMagnetSlot()
    {
        return _SlotHoldingMagnet != null;
    }

    public void SetSelectState(bool inSelectState)
    {
        _Outline.enabled = inSelectState;
    }
}
