using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Publicos & Váriaveis
    [Header("PlayerConfigs")]
    public float speed = 5f;
    public float sensitivity = 2f;

    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;

    [Header("Effects")]
    public GameObject SpeedEffect;
    public GameObject JumpEffect;
    public GameObject HealthEffect;


    [Header("Audios")]
    public AudioClip explosionCrystalAudio;

    // Privados & Váriaveis


    [Header("GroundConfig")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private int SpeedLayer = 9;
    private int HealthLayer = 10;
    private int JumpLayer = 11;

    private float originalSpeed;
    private float originalJumpForce;

    private Coroutine speedRoutine;
    private Coroutine jumpRoutine;

    private CharacterController controller;
    private Transform myCamera;
    private float yVelocity;
    private float rotationX = 0f;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        CrystalsTimerController timer = GetComponent<CrystalsTimerController>();
        myCamera = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalSpeed = speed;
        originalJumpForce = jumpForce;
    }

    void Update()
    {
        if (transform.position.y <= -5)
        {
            SceneManager.LoadScene("DeathScene");
        }

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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        int layerObj = hit.gameObject.layer;

        switch (layerObj)
        {
            case int l when l == SpeedLayer:
                SpeedUpgrade(hit.gameObject);
                break;

            case int l when l == HealthLayer:
                InvencibleUpgrade(hit.gameObject);
                break;

            case int l when l == JumpLayer:
                JumpUpgrade(hit.gameObject);
                break;

            default:
                break;
        }
    }

    private void SpeedUpgrade(GameObject crystal)
    {
        if (SpeedEffect != null)
            Instantiate(SpeedEffect, crystal.transform.position, Quaternion.identity);

        if (explosionCrystalAudio != null)
            AudioSource.PlayClipAtPoint(explosionCrystalAudio, crystal.transform.parent.position, 1f);

        if (speedRoutine != null) StopCoroutine(speedRoutine);
        speedRoutine = StartCoroutine(SpeedUpgradeRoutine(30, originalSpeed * 2));
        StartCoroutine(RespawnCrystal(crystal, 30f));
    }

    private void JumpUpgrade(GameObject crystal)
    {
        if (JumpEffect != null)
            Instantiate(JumpEffect, crystal.transform.position, Quaternion.identity);

        if (explosionCrystalAudio != null)
            AudioSource.PlayClipAtPoint(explosionCrystalAudio, crystal.transform.parent.position, 1f);

        if (jumpRoutine != null) StopCoroutine(jumpRoutine);
        jumpRoutine = StartCoroutine(JumpUpgradeRoutine(30, originalJumpForce + 5f));
        StartCoroutine(RespawnCrystal(crystal, 30f));
    }

    private void InvencibleUpgrade(GameObject crystal)
    {
        if (HealthEffect != null)
            Instantiate(HealthEffect, crystal.transform.position, Quaternion.identity);

        if (explosionCrystalAudio != null)
            AudioSource.PlayClipAtPoint(explosionCrystalAudio, crystal.transform.parent.position, 1f);

        PlayerLife life = GetComponent<PlayerLife>();
        CrystalsTimerController timer = GetComponent<CrystalsTimerController>();
        timer.HealthTimer(15f);
        if (life != null)
        {
            life.HealFull();
            life.ActivateInvincibility(15f);
        }
        StartCoroutine(RespawnCrystal(crystal, 30f));
    }

    IEnumerator SpeedUpgradeRoutine(float duration, float speedUpgrade)
    {
        speed = speedUpgrade;
        CrystalsTimerController timer = GetComponent<CrystalsTimerController>();
        timer.SpeedTimer(duration);
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
    }

    IEnumerator JumpUpgradeRoutine(float duration, float jumpUpgrade)
    {
        jumpForce = jumpUpgrade;
        CrystalsTimerController timer = GetComponent<CrystalsTimerController>();
        timer.JumpTimer(duration);
        yield return new WaitForSeconds(duration);
        jumpForce = originalJumpForce;
    }

    IEnumerator RespawnCrystal(GameObject crystal, float time)
    {
        crystal.transform.parent.gameObject.SetActive(false);

        yield return new WaitForSeconds(time);

        crystal.transform.parent.gameObject.SetActive(true);
    }
}