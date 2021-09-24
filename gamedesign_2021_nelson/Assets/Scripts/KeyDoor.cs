using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    //Choose the key used for this foor, default is yellow
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
