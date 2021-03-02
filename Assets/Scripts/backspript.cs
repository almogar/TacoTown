
using UnityEngine;

public class backspript : MonoBehaviour
{
    private GameObject GameMNG;
    // Start is called before the first frame update
    void Start()
    {
        GameMNG = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z >= 5 && transform.position.z <=  6 && (transform.position.x <= -8 || transform.position.x >= 8))
        { // finish his road
            transform.position += new Vector3(0, 0, 2); // goes to where all the happy obstcale goes
            GameMNG.GetComponent<gameManager>().freeBack(gameObject);
        }
    }
}
