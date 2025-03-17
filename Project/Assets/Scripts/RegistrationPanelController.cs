using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistrationPanelController : MonoBehaviour
{
    public GameObject registrationPanel; // Assign your panel in the Inspector

    void Start()
    {
        registrationPanel.SetActive(false); // Hide panel initially
    }

    public void ShowRegistrationPanel()
    {
        registrationPanel.SetActive(true); // Show panel when button is clicked
    }
}
