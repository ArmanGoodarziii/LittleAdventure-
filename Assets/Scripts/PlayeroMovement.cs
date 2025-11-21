using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayeroMovement : MonoBehaviour
{
    public InputSystem_Actions actions;
    private Rigidbody2D rb;
    private Animator animator;
    private PhotonView pv;
    public GameObject cameraObject;

    [SerializeField]private float speed;
    [SerializeField]private float jump;

    [Header("Collision")]
    [SerializeField]private float checkGroundDistance;
    [SerializeField]private LayerMask layerGround;

    private bool isGrounded;
    private float move;
    private float xScale;

    public GameObject canvasObject;
    public GameObject canvasButton;
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

        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;

        userName.text = GetComponent<PhotonView>().Controller.NickName;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        canvasObject.transform.position = new Vector3(transform.position.x, transform.position.y + yPos, transform.position.z);
        
        if (pv != null && pv.IsMine)
        {
            rb.linearVelocityX = move * speed;
            HandleFlip();

            cameraObject.SetActive(true);
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position , new Vector3(transform.position.x, transform.position.y , cameraObject.transform.position.z), 1 * Time.deltaTime);
        }
        else
        {
            cameraObject.SetActive(false);
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
