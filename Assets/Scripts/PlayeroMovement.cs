using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;

public class PlayeroMovement : MonoBehaviour
{
    public InputSystem_Actions actions;
    private Rigidbody2D rb;
    private Animator animator;
    private PhotonView pv;

    [SerializeField]private float speed;
    [SerializeField]private float jump;

    [Header("Collision")]
    [SerializeField]private float checkGroundDistance;
    [SerializeField]private LayerMask layerGround;

    private bool isGrounded;
    private float move;
    private float xScale;

    public GameObject canvasObject;
    public float yPos;
    public Text userName;

    void Awake()
    {
        actions = new InputSystem_Actions();
        pv = GetComponent<PhotonView>();
    }
    void OnEnable()
    {
        if(pv != null && pv.IsMine)
        {
            actions.Player.Enable();
            actions.Player.Move.performed += Movement;
            actions.Player.Jump.performed += Jumping;

            actions.Player.Move.canceled += Movement;
            actions.Player.Jump.canceled += Jumping;
        }
        
    }
    void OnDisable()
    {
        if (pv != null && pv.IsMine)
        {
            actions.Player.Disable();
            actions.Player.Move.performed -= Movement;
            actions.Player.Jump.performed -= Jumping;

            actions.Player.Move.canceled -= Movement;
            actions.Player.Jump.canceled -= Jumping;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        xScale = transform.localScale.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        canvasObject.transform.position = new Vector3(transform.position.x, transform.position.y + yPos, transform.position.z);
        
        if (pv != null && pv.IsMine)
        {
            rb.linearVelocityX = move * speed;
            HandleFlip();
        }
        HandleAnimations();
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, checkGroundDistance, layerGround);
    }
    private void HandleFlip()
    {
        if (move > 0.1f)
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);

        else if (move < -0.1f)
            transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
    }


    public void Movement(InputAction.CallbackContext context)
    {
        if (pv == null || !pv.IsMine)
            return;

        move = context.ReadValue<Vector2>().x;
    }
    public void Jumping(InputAction.CallbackContext context)
    {
        if (pv == null || !pv.IsMine)
            return;

        if (context.performed && isGrounded)
        {
            
            rb.linearVelocityY = jump;
        }
        
    }
    public void HandleAnimations()
    {
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - checkGroundDistance));
    }
}
