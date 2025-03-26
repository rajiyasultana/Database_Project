using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;

public class Items : MonoBehaviour
{
    Action<string> _creatItemsCallback;
    
    void Start()
    {
        Debug.Log("Items script started!");
        _creatItemsCallback = (jsonArrayString) =>
        {
            Debug.Log("Callback received JSON response!");
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };
        CreateItems();
    }

    public void CreateItems()
    {
        Debug.Log("CreatItems() called!");
        string userId = ObjectHolder.Instance.UserInfo.UserID;
        StartCoroutine(ObjectHolder.Instance.Web.GetItemsIDs(userId, _creatItemsCallback));
        
    }

    IEnumerator CreateItemsRoutine(string jsonArrayString)
    {
        //parsing json array string as an array
        
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //create local variables
            bool isDone = false;
            string itemId = jsonArray[i].AsObject["itemID"];
            JSONObject itemInfoJson = new JSONObject();

            //create a callback to get information from web.cs
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                Debug.Log("Item Info JSON: " + itemInfo);
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(ObjectHolder.Instance.Web.GetItem(itemId, getItemInfoCallback));

            //Wait untill the callback is called from WEB (info finisherd downloading)
            yield return new WaitUntil(() => isDone == true);

            
            //Instansiate gameobject (item prefebs)
            GameObject item = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            item.transform.SetParent(this.transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            //Fill information
            item.transform.Find("Name").GetComponent<TMP_Text>().text = itemInfoJson["Name"];
            item.transform.Find("Price").GetComponent<TMP_Text>().text = itemInfoJson["Price"];
            item.transform.Find("Description").GetComponent<TMP_Text>().text = itemInfoJson["Description"];

            //continue to the next item


        }
    }
}
