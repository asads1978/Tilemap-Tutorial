using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float playerSpeed = .5f;
    

    float lastMove;
    GameManager gameManager;

    void Start() 
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSpeed + lastMove < Time.time)

        if(Input.GetAxis("Horizontal") < 0)
        {
            MovePlayer(Vector3Int.left);       
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
                MovePlayer(Vector3Int.right);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
                MovePlayer(Vector3Int.down);
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
                MovePlayer(Vector3Int.up);
        }
    }

    void MovePlayer(Vector3Int direction)
    {
        Vector3Int targetPosition = new Vector3Int((int)gameObject.transform.position.x,(int)gameObject.transform.position.y,0) + direction;

        if (IsInBounds(targetPosition))
            {
                gameObject.transform.position += direction;
                lastMove = Time.time;
            }      
    }

    bool IsInBounds(Vector3Int position)
    {
        if(position.x < 0 || position.x >= gameManager.GetWidth() || position.y < 0 || position.y >= gameManager.GetHeight())
        {
            return false;
        }

        return true;
    }
}
