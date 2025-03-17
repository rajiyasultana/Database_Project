using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Register : MonoBehaviour
{
    public TMP_InputField UsernameInput;
    public TMP_InputField PasswordInput;
    public TMP_InputField ConfirmPasswordInput;
    public Button RegisterButton;
    public TextMeshProUGUI ErrorMessage;


    private void Start()
    {
        RegisterButton.onClick.AddListener(ValidateAndRegister);
    }
    void ValidateAndRegister()
    {
        string username = UsernameInput.text;
        string password = PasswordInput.text;
        string confirmPassword = ConfirmPasswordInput.text;

        if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            if (ErrorMessage != null) ErrorMessage.text = "All fields must be filled!";
            return;
        }

        if(password != confirmPassword)
        {
            if (ErrorMessage != null) ErrorMessage.text = "Passwords do not match!";
            return;
        }

        if (ErrorMessage != null) ErrorMessage.text = "";
        StartCoroutine(ObjectHolder.Instance.Web.RegisterUser(UsernameInput.text, PasswordInput.text));
       
    }

   

}
