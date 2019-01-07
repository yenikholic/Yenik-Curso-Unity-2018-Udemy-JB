using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Character playerChar;
    public float speed;
    public float runSpeed;
    public float walkSpeed;
    private bool isCrouching = false;
    private bool isWalking = true;
    private bool isGrounded = true;
    private int jumpNumber = 1;
    private int jumpsLeft;
    public float jumpForce;
    private Vector3 moveDirection;
    public float gravity;
    private Rigidbody rb;
    float h;
    float v;

    // Start is called before the first frame update
    void Start()
    {        
        rb = GetComponent<Rigidbody>();
        speed = walkSpeed;
        runSpeed = walkSpeed * 2;
        jumpsLeft = jumpNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void FixedUpdate()
    {
        isWalking = !Input.GetKey(KeyCode.LeftShift);
        isCrouching = !Input.GetKey(KeyCode.LeftControl);

        if (!isWalking) speed = runSpeed;
        else speed = walkSpeed;

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (Input.GetAxis("Mouse X") > 0){
            transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
        }
        if (isGrounded)
        {
            moveDirection = new Vector3(h, 0, v);
            moveDirection = moveDirection * speed ;
            rb.AddForce(moveDirection);
            if (Input.GetButton("Jump") && jumpsLeft > 0)
            {
                isGrounded = false;
                jumpsLeft--;
                moveDirection.y = jumpForce;
                rb.AddForce(0, moveDirection.y, 0);
            }

        }

        moveDirection.y -= gravity * Time.deltaTime;
        rb.AddForce(moveDirection);         
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain" || collision.gameObject.tag == "Platform")
        {
            isGrounded = true;
            jumpsLeft = jumpNumber;
        } 
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain" || collision.gameObject.tag == "Platform")
        {
            isGrounded = false;
        }
    }
}
