using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class Items : MonoBehaviour
{
    Action<string> _creatItemsCallback;
    // Start is called before the first frame update
    void Start()
    {
        _creatItemsCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };
    }

    public void CreatItems()
    {
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
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(ObjectHolder.Instance.Web.GetItem(itemId, getItemInfoCallback));

            //Wait untill the callback is called from WEB (info finisherd downloading)
            yield return new WaitUntil(() => isDone == true);

        }
        yield return null;
    }
}
