using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeGameController : MonoBehaviour
{
    public List<MagnetSlot> SlotsInOrder;
    
    private MagnetSlot _SelectedSlot;
    private MoveableMagnet _SelectedMagnet;
    
    private const string GAMECHANGEINFORMATIONTAG = "GameChangeInformation";
    private GameChangeInformation _GameChangeInfo;

    void Start()
    {
        _GameChangeInfo = GameObject.FindWithTag(GAMECHANGEINFORMATIONTAG).GetComponent<GameChangeInformation>();
    }

    private void OnEnable()
    {
        foreach (MagnetSlot slot in SlotsInOrder)
        {
            slot.Reset();
        }

        _SelectedSlot = null;
        _SelectedMagnet = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray mousRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit mouseHit;
            Physics.Raycast(mousRay, out mouseHit, float.MaxValue);

            if (mouseHit.collider != null)
            {
                MoveableMagnet magnet = mouseHit.collider.GetComponent<MoveableMagnet>();
                MagnetSlot slot = mouseHit.collider.GetComponent<MagnetSlot>();

                if (magnet != null)
                {
                    bool swapping = false;
                    
                    // we selected a magnet and we had a magnet previously selected
                    if (_SelectedMagnet != null)
                    {
                        // both in slot so going to swap
                        if (magnet.IsInMagnetSlot() == true &&
                            _SelectedMagnet.IsInMagnetSlot() == true)
                        {
                            swapping = true;
                        }
                    }

                    bool moving = false;

                    // not swapping so check moving to a slot
                    if (swapping == false)
                    {
                        if (_SelectedSlot != null &&
                            _SelectedSlot.IsSlotFull() == false)
                        {
                            moving = true;
                        }
                    }

                    if (swapping == true)
                    {
                        MagnetSlot swappingTo = _SelectedMagnet.GetHoldingSlot();
                        _SelectedMagnet.SetSlotHoldingMagnet(magnet.GetHoldingSlot());
                        magnet.SetSlotHoldingMagnet(swappingTo);
                        
                        _SelectedMagnet.SetSelectState(false);
                        _SelectedMagnet = null;

                        if (_SelectedSlot != null)
                        {
                            _SelectedSlot.SetSelectState(false);
                            _SelectedSlot = null;
                        }

                        CheckWinState();
                    }
                    else if (moving == true)
                    {
                        magnet.SetSlotHoldingMagnet(_SelectedSlot);
                        
                        _SelectedSlot.SetSelectState(false);
                        _SelectedSlot = null;
                        
                        if (_SelectedMagnet != null)
                        {
                            _SelectedMagnet.SetSelectState(false);
                            _SelectedMagnet = null;
                        }

                        CheckWinState();
                    }
                    else
                    {
                        // just change selection
                        
                        if (_SelectedMagnet != null)
                        {
                            _SelectedMagnet.SetSelectState(false); 
                        }
                        
                        magnet.SetSelectState(true);
                        _SelectedMagnet = magnet;
                    }
                }
                else if (slot != null)
                {
                    bool swapping = false;
                    
                    // we selected a slot and we had a slot previously selected
                    if (_SelectedSlot != null)
                    {
                        // both slots are full so going to swap
                        if (slot.IsSlotFull() == true &&
                            _SelectedSlot.IsSlotFull() == true)
                        {
                            swapping = true;
                        }
                    }

                    bool moving = false;

                    // not swapping so check moving to a slot
                    if (swapping == false)
                    {
                        // selected magnet has no slot
                        if (_SelectedMagnet != null &&
                            _SelectedMagnet.IsInMagnetSlot() == false)
                        {
                            moving = true;
                        }
                    }

                    if (swapping == true)
                    {
                        MoveableMagnet swappingWith = _SelectedSlot.GetMagnetInSlot();
                        _SelectedSlot.SetMagnetInSlot(slot.GetMagnetInSlot());
                        slot.SetMagnetInSlot(swappingWith);

                        _SelectedSlot.SetSelectState(false);
                        _SelectedSlot = null;
                        
                        if (_SelectedMagnet != null)
                        {
                            _SelectedMagnet.SetSelectState(false);
                            _SelectedMagnet = null;
                        }

                        CheckWinState();
                    }
                    else if (moving == true)
                    {
                        slot.SetMagnetInSlot(_SelectedMagnet);
                        _SelectedMagnet.SetSelectState(false);
                        _SelectedMagnet = null;

                        if (_SelectedSlot != null)
                        {
                            _SelectedSlot.SetSelectState(false);
                            _SelectedSlot = null;
                        }

                        CheckWinState();
                    }
                    else
                    {
                        // just change selection
                        
                        if (_SelectedSlot != null)
                        {
                            _SelectedSlot.SetSelectState(false);
                        }
                        
                        slot.SetSelectState(true);
                        _SelectedSlot = slot;
                    }
                }
            }
        }
    }

    private void CheckWinState()
    {
        int index = 0;
        foreach (MagnetSlot slot in SlotsInOrder)
        {
            if (slot.DoesContainCorrectIndex(index) == false)
            {
                return;
            }

            index++;
        }
        
        _GameChangeInfo.ChangeToMainGame(true, "fridge");
    }
}
