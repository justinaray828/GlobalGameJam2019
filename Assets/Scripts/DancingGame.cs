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
        FindObjectOfType<AudioManager>().ToDanceMusic();
    }

    private void OnDisable()
    {
        FindObjectOfType<AudioManager>().FadeOut("puzzleDance");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
