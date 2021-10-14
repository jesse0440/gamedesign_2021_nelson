using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_KeyHolder : MonoBehaviour
{
    // Variables needed in this script
    PlayerController keyHolder;
    Transform container;
    Transform keyTemplate;

    void Awake()
    {
        container = transform.Find("container");
        keyTemplate = container.Find("keyTemplate");
    }

    void Start()
    {
        // Assign the player's script
        keyHolder = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        // Add a function to this function
        keyHolder.OnKeysChanged += KeyHolder_OnKeysChanged;

        // Hide the UI container for keys in Start
        keyTemplate.gameObject.SetActive(false);
    }

    // Updates on keysChanged activated
    private void KeyHolder_OnKeysChanged(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    // Update the UI
    private void UpdateVisual()
    {
        keyTemplate.gameObject.SetActive(true);
        
        // Clean up old keys
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

        // List of keys
        List<KeyCards.KeyType> keyList = keyHolder.GetKeyList();

        // Show keys owned in a list
        for (int i=0; i<keyList.Count; i++)
        {
            KeyCards.KeyType keyType = keyList[i];
            Transform keyTransform = Instantiate(keyTemplate, container);
            keyTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(50 * i, 0);
            Image keyImage = keyTransform.Find("Image").GetComponent<Image>();

            switch (keyType)
            {
                // Checks the color to assign to the key
                case KeyCards.KeyType.Yellow:
                    keyImage.color = Color.yellow;
                    break;
                case KeyCards.KeyType.Blue:
                    keyImage.color = Color.blue;
                    break;
                case KeyCards.KeyType.Red:
                    keyImage.color = Color.red;
                    break;
                default:
                    break;
            }
        }

        // If the key list is empty hide the UI
        if (keyList.Count == 0)
        {
            keyTemplate.gameObject.SetActive(false);
        }
    }
}