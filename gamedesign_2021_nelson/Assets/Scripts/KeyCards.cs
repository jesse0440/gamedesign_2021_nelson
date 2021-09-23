using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCards : MonoBehaviour
{
    //Below is the option to choose what colour the keycard is.
    //e.g. Red key is only for red doors (for future proof)
    [SerializeField] private KeyType keyType;
    public enum KeyType
    {
        Yellow,
        Red,
        Blue
    }
    //returns the current keytype
    public KeyType GetKeyType()
    {
        return keyType;
    }
}
