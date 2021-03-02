using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int maxLeft;
    public int maxRight;
    public int movementLength;
    public float currentSpeed = 0;
    public float maxSpeed;
    public float acceleration;
    public bool startRun;
    public bool right = false;
    public bool left = false;
    private Rigidbody rb;
    private Vector3 end;
    public int i = 0;
    public bool isDead = false;

    public ParticleSystem Saprks;
    public ParticleSystem Confetti;

    public GameObject RightTurn;
    public GameObject LeftTurn;

    //PhoneStick
    public Vector2 startTouchPos, endTouchPos;
    public bool isFingerLift = false;
    

    // Start is called before the first frame update
    void Start()
    {
        maxLeft = 0 - movementLength;
        maxRight = 0 + movementLength;
        rb = GetComponent<Rigidbody>();
        RightTurn = GameObject.Find("RotateRightPivot");
        LeftTurn = GameObject.Find("RotateLeftPivot");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            isFingerLift = false;
            startTouchPos = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            isFingerLift = true;
            endTouchPos = Input.GetTouch(0).position;
        }

        transform.position = Vector3.MoveTowards(transform.position, end, 50 * Time.deltaTime);
        
        if (!isDead && isFingerLift)
        {
            
            if ((endTouchPos.x < startTouchPos.x) && transform.position.x > maxLeft)
            {
                isFingerLift = false;
                end = transform.position - new Vector3(movementLength, 0, 0);
                LeftTurn.GetComponent<Animator>().Play("Player_Rotate_Left");
            }

            else if ((endTouchPos.x > startTouchPos.x) && transform.position.x < maxRight)
            {
                isFingerLift = false;
                end = transform.position + new Vector3(movementLength, 0, 0);
                RightTurn.GetComponent<Animator>().Play("Player_Rotation_Anim");
            }
        }
        /*
        if (!isDead)
        {
            // move the player
            transform.position = Vector3.MoveTowards(transform.position, end, 50 * Time.deltaTime);

            // to where?
            if (Input.GetKeyDown("left") && transform.position.x > maxLeft)
            {
                end = transform.position - new Vector3(movementLength, 0, 0);
                LeftTurn.GetComponent<Animator>().Play("Player_Rotate_Left");
            }


            if (Input.GetKeyDown("right") && transform.position.x < maxRight)
            {
                end = transform.position + new Vector3(movementLength, 0, 0);
                RightTurn.GetComponent<Animator>().Play("Player_Rotation_Anim");
            }
        }
        */
        

    }



}
