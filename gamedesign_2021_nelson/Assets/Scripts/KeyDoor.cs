using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    [SerializeField] private KeyCards.KeyType keyType;

    public KeyCards.KeyType GetKeyType()
    {
        return keyType;
    }
    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}
