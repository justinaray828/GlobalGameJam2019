using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach script to Parent UI Canvas and child a menu to access. The script contains useful methods
/// for the menu to access and use.
/// </summary>
public class MenuUI : MonoBehaviour
{ 
    [Header("Add Child Menu to toggle")]
    [SerializeField] private GameObject menu;

    private bool menuToggleBool = false;
    private string cancelInputString = "Cancel"; //Unity input manager axis

    void Update()
    {
        if( Input.GetButtonDown(cancelInputString) )
        {
            menu.SetActive(menuToggleBool);
            menuToggleBool = !menuToggleBool;
        }
    }

}
