using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Transform myCamera;

    public float speed = 5f;
    public float sensitivity = 2f;

    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private float yVelocity;
    private float rotationX = 0f;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        myCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 inputDir = (right * horizontal + forward * vertical).normalized;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded)
        {
            if (inputDir.magnitude > 0.1f)
                moveDirection = inputDir;
            else
                moveDirection = Vector3.zero;

            if (yVelocity < 0)
                yVelocity = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
                if (moveDirection == Vector3.zero)
                    moveDirection = Vector3.zero;
            }
        }
        else
        {
            if (inputDir.magnitude > 0.1f)
                moveDirection = inputDir;
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 moveXZ = moveDirection * speed;
        Vector3 finalMove = new Vector3(moveXZ.x, yVelocity, moveXZ.z);
        controller.Move(finalMove * Time.deltaTime);
    }
}