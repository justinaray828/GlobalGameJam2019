using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
public class DistortionLevel
{
	public float LDMax;
	public float LDMin;
	public float VIGValue;
	public float CGSatValue;
}

public class DistortionEffects : MonoBehaviour
{
    public PostProcessVolume PPV;

	private LensDistortion _LD;
	private Vignette _VIG;
	private ColorGrading _CG;

	private float _LDMin = -50f;
	private float _LDMax = 50f;
	private float _LDValue = 0f;
	private bool _LDForward = true;
	
	private float _VIGValue = 0f;
	
	private float _CGSatValue = 0f;

	public List<DistortionLevel> _Levels = new List<DistortionLevel>();
	private int _DistortionLevel;

	public static DistortionEffects Instance;
	
    // Start is called before the first frame update
    void Start()
    {
	    _LD = PPV.profile.GetSetting<LensDistortion>();
	    _VIG = PPV.profile.GetSetting<Vignette>();
	    _CG = PPV.profile.GetSetting<ColorGrading>();

	    SetDistortionLevel(0);

	    Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
	    if (_LDForward == true)
	    {
		    _LD.intensity.Interp(_LDMin, _LDMax, _LDValue);

		    _LDValue += Time.deltaTime / 10;

		    if (_LDValue >= 1)
		    {
			    _LDValue = 0;
			    _LDForward = false;
		    }
	    }
	    else
	    {
		    _LD.intensity.Interp(_LDMax, _LDMin, _LDValue);
		    
		    _LDValue += Time.deltaTime / 10;
		    
		    if (_LDValue >= 1)
		    {
			    _LDValue = 0;
			    _LDForward = true;
		    }
	    }

	    _VIG.intensity.Interp(_VIG.intensity.value, _VIGValue, Time.deltaTime);
	    
	    _CG.saturation.Interp(_CG.saturation.value, _CGSatValue, Time.deltaTime);
    }

	public void SetDistortionLevel(int inLevel)
	{
		if (inLevel >= _Levels.Count ||
		    inLevel < 0)
		{
			return;
		}
		
		_DistortionLevel = inLevel;
		DistortionLevel distortionLevel = _Levels[inLevel];
		_LDMax = distortionLevel.LDMax;
		_LDMin = distortionLevel.LDMin;
		_VIGValue = distortionLevel.VIGValue;
		_CGSatValue = distortionLevel.CGSatValue;
	}

	public int GetDistortionLevel()
	{
		return _DistortionLevel;
	}
}
