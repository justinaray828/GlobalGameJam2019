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
    private bool iswalking = false;

    private const string INTERATABLEOBJECTTAG = "InteractableObject";

    public static PlayerMovement Instance;

    public AudioManager am;

    void Start()
    {
        Instance = this;
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        
        Controller.SimpleMove(new Vector3(horizontal * Speed, 0f, vertical * Speed));

        Model.transform.forward = Controller.velocity.normalized;
        Model.transform.rotation = Quaternion.Euler(new Vector3(0f, Model.transform.rotation.eulerAngles.y, Model.transform.rotation.eulerAngles.z));

        if (horizontal > 0f || horizontal < 0f || vertical < 0f || vertical > 0f)
        {
            if (!iswalking)
            {
                am.PlayWalk();
                iswalking = true;
            }
        }
        else
        {
            if (iswalking)
            {
                am.StopWalk();
                iswalking = false;
            }
        }

        ClickInteraction();
    }

    private InteractableObject _LastInteractable = null;

    /// <summary>
    /// Raycast mouse position on left click and interact with object if present
    /// </summary>
    private void ClickInteraction()
    {
        Ray mousRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layersToIgnore = 1 << 9;
        layersToIgnore = ~layersToIgnore;
            
        RaycastHit mouseHit;
        Physics.Raycast(mousRay, out mouseHit, float.MaxValue, layersToIgnore);

        InteractableObject interactable = null;
        if (mouseHit.collider != null)
        {
            if (mouseHit.transform.tag == INTERATABLEOBJECTTAG)
            {
                interactable = mouseHit.transform.GetComponent<InteractableObject>();
            }
        }

        if (interactable != _LastInteractable)
        {
            if (_LastInteractable != null)
            {
                _LastInteractable.SetHover(false);
            }

            bool didSetNewInteractable = false;
            if (interactable != null)
            {
                didSetNewInteractable = interactable.SetHover(true);
            }

            if (didSetNewInteractable == true)
            {
                _LastInteractable = interactable;
            }
        }
        else
        {
            bool didSetNewInteractable = false;
            if (_LastInteractable != null)
            {
                didSetNewInteractable = _LastInteractable.SetHover(true);
            }
            
            if (didSetNewInteractable == false)
            {
                _LastInteractable = null;
            }
        }
        
        if (Input.GetMouseButtonUp(0) &&
            interactable != null)
        {
            interactable.Interact();
        }
    }
}
