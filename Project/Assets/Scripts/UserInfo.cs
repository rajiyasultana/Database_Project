using UnityEngine;

public class UserInfo : MonoBehaviour
{
     public string UserID { get; private set; }
     private string UserName;
     private string UserPassword;
     private string Level;
     private string Coins;

    public void SetCredentials(string username, string userpassword)
    {
        UserName = username;
        UserPassword = userpassword;
    }

    public void SetID(string id)
    {
        UserID = id;
    }
    

}
