using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
    void Start()
    {
        // A correct website page.
        //StartCoroutine(GetUsers("http://localhost/UnityBackend/GetUsers.php"));
        //StartCoroutine(Login("testUser", "12345"));
        //StartCoroutine(RegisterUser("testUser3", "123456"));

    }

    public IEnumerator GetItemIcon(string itemID, System.Action<Sprite> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);

        string uri = "http://localhost/UnityBackend/GetItemIcon.php";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form)) // Use Post instead of Get
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //reasults as byte array
                    byte[] bytes = webRequest.downloadHandler.data;

                    //create texture2D
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(bytes);

                    //create sprite (to be placed in UI)
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    callback(sprite);

                    break;
            }
        }
    }


    public IEnumerator GetUsers(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
            form.AddField("loginUser", username);
            form.AddField("loginPass", password);

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackend/Login.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    ObjectHolder.Instance.UserInfo.SetCredentials(username, password);
                    ObjectHolder.Instance.UserInfo.SetID(www.downloadHandler.text);

                    
                    if(www.downloadHandler.text.Contains("Wrong Password.") || www.downloadHandler.text.Contains("Username does not exists"))
                    {
                            Debug.Log("Try Again");
                    }
                    else
                    {
                        //IF we logged in correctly
                        ObjectHolder.Instance.UserProfile.SetActive(true);
                        ObjectHolder.Instance.Login.gameObject.SetActive(false);
                        ItemManager items = FindObjectOfType<ItemManager>();
                        items.CreateItems();
                        

                    }
                }
            }
    }

    public IEnumerator RegisterUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackend/RegisterUser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetItemsIDs(string userID, System.Action<string> callback)
    {
        Debug.Log("GetItemsIDs function is being called");
        Debug.Log("userID parameter received: " + userID);
        WWWForm form = new WWWForm();
        form.AddField("userID", userID); 

        string uri = "http://localhost/UnityBackend/GetItemsIDs.php";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form)) // Use Post instead of Get
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
                string jsonArray = webRequest.downloadHandler.text;
                if (callback != null)
                {
                    callback(jsonArray);
                }
                else
                {
                    Debug.LogWarning("Callback in GetItemsIDs was null!");
                }

            }
            else
            {
                Debug.LogError("Error fetching items: " + webRequest.error);
            }
        }
    }

    public IEnumerator GetItem(string itemID, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);

        string uri = "http://localhost/UnityBackend/GetItem.php";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form)) // Use Post instead of Get
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string jsonArray = webRequest.downloadHandler.text;
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    callback(jsonArray);

                    break;
            }
        }
    }

    public IEnumerator SellItem(string ID, string itemID, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", ID);
        form.AddField("itemID", itemID);
        form.AddField("userID", userID);

        string uri = "http://localhost/UnityBackend/SellItem.php";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form)) // Use Post instead of Get
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    
                    break;
            }
        }
    }


}
