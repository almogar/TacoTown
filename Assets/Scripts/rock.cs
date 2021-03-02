using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock : MonoBehaviour
{
    private GameObject GameMNG;
    private Vector3 resetPosition;
    // Start is called before the first frame update
    void Start()
    {
        GameMNG = GameObject.Find("GameManager");
        resetPosition = GameMNG.GetComponent<gameManager>().startRoadPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= resetPosition.z && transform.position.x <= 2 && transform.position.x >= -2)
        { // finish his road
            transform.position += new Vector3(0, 0, 2); // goes to where all the happy obstcale goes
            GameMNG.GetComponent<gameManager>().freeObst(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Car")
        {
            GameMNG.GetComponent<gameManager>().freeObstcle(gameObject);
        }
    }
}
