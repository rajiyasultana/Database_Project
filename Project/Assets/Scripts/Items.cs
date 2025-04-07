using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;
using TMPro;

public class Items : MonoBehaviour
{
    private Action<string> _createItemsCallback;

    private void Awake()
    {
        Debug.Log("Items script started!");
        _createItemsCallback = (jsonArrayString) =>
        {
            Debug.Log("Callback received JSON response!");
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };
    }

    public void CreateItems()
    {
        Debug.Log("CreateItems() called!");
        string userId = ObjectHolder.Instance.UserInfo.UserID;
        StartCoroutine(ObjectHolder.Instance.Web.GetItemsIDs(userId, _createItemsCallback));
    }

    private IEnumerator CreateItemsRoutine(string jsonArrayString)
    {
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        if (jsonArray == null)
        {
            Debug.LogError("Invalid JSON array received.");
            yield break;
        }

        for (int i = 0; i < jsonArray.Count; i++)
        {
            bool isDone = false;
            string itemId = jsonArray[i].AsObject["itemID"];
            JSONObject itemInfoJson = new JSONObject();

            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                if (tempArray != null && tempArray.Count > 0)
                {
                    itemInfoJson = tempArray[0].AsObject;
                }
                else
                {
                    Debug.LogWarning("Invalid item info received for itemID: " + itemId);
                }
                isDone = true;
            };

            StartCoroutine(ObjectHolder.Instance.Web.GetItem(itemId, getItemInfoCallback));

            yield return new WaitUntil(() => isDone);

            GameObject item = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            item.transform.SetParent(this.transform, false);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            item.transform.Find("Name").GetComponent<TMP_Text>().text = itemInfoJson["Name"];
            item.transform.Find("Price").GetComponent<TMP_Text>().text = itemInfoJson["Price"];
            item.transform.Find("Description").GetComponent<TMP_Text>().text = itemInfoJson["Description"];
        }
    }
}
