using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    //movement variables
    CharacterController characterController;
    PlayerInput playerInput;
    Vector2 movementInput;
    Vector3 currecntMovement;
    bool isMovementPressed;
    [SerializeField] float speed = 2.0f;
    //gravity variables
    float gravity = -9.8f;
    [SerializeField] float gravityMultiplier = 3.0f;
    float yVelocity;
    //Noise when walking Variable
    [SerializeField] CinemachineBasicMultiChannelPerlin  noise;
    [SerializeField] float bobAmplitude = 1f;
    [SerializeField] float bobFrequency = 2f;
    [SerializeField] float SmoothingSpeed = 8f;
    bool isWalking;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInput();
        playerInput.CharacterControls.Move.performed += ReadMoveInput;
        playerInput.CharacterControls.Move.canceled += ReadMoveInput;
    }
    void ReadMoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        currecntMovement.x = movementInput.x * speed;
        currecntMovement.z = movementInput.y * speed;
        isMovementPressed = movementInput.x != 0 || movementInput.y != 0;
    }
    void FixedUpdate()
    {
        HandleMovement();
        HandleGravity();
        CheckIsWalking();
        HandleNoise();
    }

    private void HandleGravity()
    {
        //simple gravity when there no jump or anything else
        yVelocity = gravity * gravityMultiplier * Time.deltaTime;
        currecntMovement.y = yVelocity;
    }

    private void HandleMovement()
    {
        //HandleAniamtion();
        //HandleRotation();
        characterController.Move(currecntMovement * Time.deltaTime);
    }
    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }
    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
    void CheckIsWalking(){
        if(characterController.velocity.magnitude > 0.1f) isWalking = true;
        else isWalking = false;
    }
    void HandleNoise()
    {
        float targetAmp = isWalking ? bobAmplitude : 0f;
        float targetFreq = isWalking ? bobFrequency : 0f;
        noise.AmplitudeGain = Mathf.Lerp(noise.AmplitudeGain, targetAmp, Time.deltaTime * SmoothingSpeed);
        noise.FrequencyGain = Mathf.Lerp(noise.FrequencyGain, targetFreq, Time.deltaTime * SmoothingSpeed);
    }
}
