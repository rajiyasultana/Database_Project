using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    public TMP_InputField UsernameInput;
    public TMP_InputField PasswordInput;
    public Button LoginButton;
    //public Items items;
    void Start()
    {
        //items = GetComponent<Items>();
        LoginButton.onClick.AddListener(() =>
        {
            StartCoroutine(ObjectHolder.Instance.Web.Login(UsernameInput.text, PasswordInput.text));
            
        });
    }
    
}
