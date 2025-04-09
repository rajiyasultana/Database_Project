using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;
using TMPro;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
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
            string id = jsonArray[i].AsObject["ID"];

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

            //Instansiate gameobject
            GameObject itemGo = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            Item item = itemGo.AddComponent<Item>();

            item.ID = id;
            item.ItemID = itemId;

            itemGo.transform.SetParent(this.transform, false);
            itemGo.transform.localScale = Vector3.one;
            itemGo.transform.localPosition = Vector3.zero;

            //Fill Information
            itemGo.transform.Find("Name").GetComponent<TMP_Text>().text = itemInfoJson["Name"];
            itemGo.transform.Find("Price").GetComponent<TMP_Text>().text = itemInfoJson["Price"];
            itemGo.transform.Find("Description").GetComponent<TMP_Text>().text = itemInfoJson["Description"];

            //Create call back to get sprite from web.cs
            Action<Sprite> getItemIconCallback = (downloadedSprite) =>
            {
                itemGo.transform.Find("Image").GetComponent<Image>().sprite = downloadedSprite;

            };
            StartCoroutine(ObjectHolder.Instance.Web.GetItemIcon(itemId, getItemIconCallback));

            //Set sell button
            itemGo.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                string idInInventory = id;
                string iId = itemId;
                string userId = ObjectHolder.Instance.UserInfo.UserID;

                StartCoroutine(ObjectHolder.Instance.Web.SellItem(idInInventory, itemId, userId));
            });
        }
    }
}
