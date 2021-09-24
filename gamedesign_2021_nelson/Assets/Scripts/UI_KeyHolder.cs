using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_KeyHolder : MonoBehaviour
{
    [SerializeField] private PlayerController keyHolder;

    private Transform container;
    private Transform keyTemplate;

    private void Awake()
    {
        //hides the UI container for key in Awake
        container = transform.Find("container");
        keyTemplate = container.Find("keyTemplate");
        keyTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        keyHolder.OnKeysChanged += KeyHolder_OnKeysChanged;
    }
    //updates on keys changed (player has picked up, and opened a door)
    private void KeyHolder_OnKeysChanged(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }
    //updates the UI
    private void UpdateVisual()
    {
        keyTemplate.gameObject.SetActive(true);
        //cleans up old keys
        foreach (Transform child in container)
        {
            if (child == keyTemplate)
            {
                continue;
            }
            else
            {
                Destroy(child.gameObject);          
            }
        }
        List<KeyCards.KeyType> keyList = keyHolder.GetKeyList();
        //shows current keys holded in a list THE KEYS ARE STACKABLE
        for (int i=0; i<keyList.Count; i++)
        {
            //keyTemplate.gameObject.SetActive(true);
            KeyCards.KeyType keyType = keyList[i];
            Transform keyTransform = Instantiate(keyTemplate, container);
            keyTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(50 * i, 0);
            Image keyImage = keyTransform.Find("Image").GetComponent<Image>();
            switch (keyType)
            {
                //checks the color picked up and recolours the templatekey sprite accordingly
                default:
                case KeyCards.KeyType.Yellow:   keyImage.color = Color.yellow;  break;
                case KeyCards.KeyType.Blue:     keyImage.color = Color.blue;    break;
                case KeyCards.KeyType.Red:      keyImage.color = Color.red;     break;
            }
        }
        //if the keylist is empty, clear the UI, showing nothing
        if (keyList.Count == 0)
        {
            keyTemplate.gameObject.SetActive(false);
        }
    }
}
