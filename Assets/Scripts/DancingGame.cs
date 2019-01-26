using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(FindObjectOfType<AudioManager>() != null)
            FindObjectOfType<AudioManager>().ToDanceMusic();
    }

    private void OnDisable()
    {
        if (FindObjectOfType<AudioManager>() != null)
            FindObjectOfType<AudioManager>().FadeOut("puzzleDance");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
