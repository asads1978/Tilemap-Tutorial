using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float playerSpeed = .5f;

    float lastMove;

    // Update is called once per frame
    void Update()
    {
        if (playerSpeed + lastMove < Time.time)

        if(Input.GetAxis("Horizontal") < 0)
        {
            gameObject.transform.position += Vector3Int.left;
            lastMove = Time.time;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            gameObject.transform.position += Vector3Int.right;
            lastMove = Time.time;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            gameObject.transform.position += Vector3Int.down;
            lastMove = Time.time;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            gameObject.transform.position += Vector3Int.up;
            lastMove = Time.time;
        }
    }
}
