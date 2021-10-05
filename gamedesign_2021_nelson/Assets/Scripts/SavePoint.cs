using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    GameObject player;
    Transform text;
    BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        text = transform.Find("Text");
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("yeyeyeyey");
        if (collision.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(true);

            if (Input.GetKey("x"))
            {
                player = GameObject.FindWithTag("Player");
                SaveSystem.SavePlayer(player.GetComponent<PlayerController>());
                Debug.Log(Application.persistentDataPath);
            }
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}