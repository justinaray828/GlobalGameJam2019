using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;

    public float Speed;

    public Transform Model;

    private float _DesiredZDepth = -5f;
    private float _CurrentZDepth = -5f;
    private bool _StartedChangingDepth = false;

    private const string INTERATABLEOBJECTTAG = "InteractableObject";

    public static PlayerMovement Instance;

    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        
        Controller.SimpleMove(new Vector3(horizontal * Speed, 0f, vertical * Speed));

        ClickInteraction();
    }

    /// <summary>
    /// Raycast mouse position on left click and interact with object if present
    /// </summary>
    private void ClickInteraction()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray mousRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layersToIgnore = 1 << 9;
            layersToIgnore = ~layersToIgnore;
            
            RaycastHit mouseHit;
            Physics.Raycast(mousRay, out mouseHit, float.MaxValue, layersToIgnore);

            if (mouseHit.collider != null)
            {
                if (mouseHit.transform.tag == INTERATABLEOBJECTTAG)
                {
                    mouseHit.transform.GetComponent<InteractableObject>().Interact();
                }
            }
        }
    }
}
