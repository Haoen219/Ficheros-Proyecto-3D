using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     // Start is called before the first frame update
    public float speed;
    private CharacterController characterController;
    public float jumpSpeed;
    public float ySpeed;
    public float rotation;
    private float? lastGrounded;
    private float? jumpButtonPresssTime;
    public float jumpButtonPeriod;

    private float dashTimer = 0;
    public float dashCooldown;
    
    public float accelerationDash;
    private bool hasDashed = false;

    public Transform WallController;
    public bool onWall;
    public LayerMask layerMask;
    public Vector3 boxSizes = new Vector3(0.5f,0.5f,0.5f);



    void Start()
    {
        characterController = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontal, 0, 0);
        moveDirection.Normalize();
        float magnitude = moveDirection.magnitude;
        magnitude = Mathf.Clamp01(magnitude);


        if(Input.GetKeyDown(KeyCode.Z) && characterController.isGrounded){
            if(dashTimer<=0){
                dashTimer = dashCooldown;
                magnitude *= accelerationDash;
                hasDashed = true;
            }
        }
        if(dashTimer>0){
            dashTimer -= Time.deltaTime;
            magnitude *= accelerationDash;
        }else if(hasDashed){
            hasDashed = false;
            magnitude /= accelerationDash;
        }
        
        characterController.Move(moveDirection * speed * Time.deltaTime * magnitude);

        ySpeed += Physics.gravity.y * Time.deltaTime;
        
        Vector3 velocity = moveDirection * magnitude;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);


        if(characterController.isGrounded)
        {
            lastGrounded = Time.time;
            if(Input.GetButtonDown("Jump"))
            {
                jumpButtonPresssTime = Time.time;
            }
        }
                
        if(Time.time - lastGrounded <= jumpButtonPeriod)
        {
            ySpeed = -0.5f;
            velocity.y = ySpeed;
            characterController.Move(velocity * Time.deltaTime);

            if(Time.time - jumpButtonPresssTime <= jumpButtonPeriod)
            {
                ySpeed = jumpSpeed;
                jumpButtonPresssTime = null;
                lastGrounded = null;
            }

        }
        // Collider[] hitColliders = Physics.OverlapBox(WallController.position,boxSizes,Quaternion.identity,layerMask);
        // int direction = 1;
        // Debug.Log(hitColliders.Length);
        // if(hitColliders.Length > 0 && !characterController.isGrounded){
        //     velocity.x = -velocity.x;
        //     velocity.y = ySpeed;
        //     // characterController.Move(velocity *Time.deltaTime);
        //     direction = -1;
        // }

        if(moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotation * Time.deltaTime);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(WallController.position, boxSizes);
    }
}