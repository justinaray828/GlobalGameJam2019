using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadedToAndFromBlackManager : MonoBehaviour
{
    public Image BlackFadeImage;
    
    public static FadedToAndFromBlackManager Instance;

    private bool _Fading = false;
    private bool _FadeToBlack = false;
    private float _FadeAlpha = 0f;

    private Action FinishedFading;
    
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (_Fading == true)
        {
            if (_FadeToBlack == true)
            {
                if (_FadeAlpha < 1f)
                {
                    _FadeAlpha += Time.deltaTime;
                }
                else
                {
                    _FadeAlpha = 1f;
                    _Fading = false;
                    
                    if (FinishedFading != null)
                    {
                        FinishedFading();
                    }
                }
            }
            else
            {
                if (_FadeAlpha > 0f)
                {
                    _FadeAlpha -= Time.deltaTime;
                }
                else
                {
                    _FadeAlpha = 0f;
                    _Fading = false;

                    if (FinishedFading != null)
                    {
                        FinishedFading();
                    }
                }
            }
            
            Color currentColor = BlackFadeImage.color;
            BlackFadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, _FadeAlpha);
        }
        
    }

    public void FadeToBlack()
    {
        _FadeAlpha = 0f;
        _Fading = true;
        _FadeToBlack = true;
    }
    
    public void FadeFromBlack()
    {
        _FadeAlpha = 1f;
        _Fading = true;
        _FadeToBlack = false;
    }

    public void RegisterForFinishedFading(Action inAction)
    {
        if (inAction != null)
        {
            FinishedFading += inAction;
        }
    }
    
    public void UnregisterForFinishedFading(Action inAction)
    {
        if (inAction != null)
        {
            FinishedFading -= inAction;
        }
    }
}
