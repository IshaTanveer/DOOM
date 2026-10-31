using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using Unity.Mathematics;

public class PlayerMovement : MonoBehaviour
{
    //movement variables
    CharacterController characterController;
    InputSystem_Actions inputsystem_action;
    Vector2 movementInput;
    Vector3 targetVelocity;
    Vector3 currentMovement;
    bool isMovementPressed;
    [SerializeField] float acceleration, deceleration;
    [SerializeField] float speed = 2.0f;
    Vector3 horizontalVelocity;
    //gravity variables
    float gravity = -9.8f;
    [SerializeField] float gravityMultiplier = 3.0f;
    float yVelocity;
    //Noise when walking Variable
    [SerializeField] CinemachineBasicMultiChannelPerlin  noise;
    [SerializeField] float bobAmplitude = 1f;
    [SerializeField] float bobFrequency = 2f;
    [SerializeField] float SmoothingSpeed = 8f;
    [SerializeField] Transform cameraTransform;
    bool isWalking;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputsystem_action = new InputSystem_Actions();
        inputsystem_action.Player.Move.performed += ReadMoveInput;
        inputsystem_action.Player.Move.canceled += ReadMoveInput;
    }
    void ReadMoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        targetVelocity = new Vector3(movementInput.x, 0, movementInput.y) * speed;
        isMovementPressed = movementInput.x != 0 || movementInput.y != 0;
    }
    void Update()
    {
        HandleRotation();
        HandleGravity();
        HandleMovement();
        CheckIsWalking();
        HandleNoise();
    }
    void HandleRotation()
    {
        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
    }
    private void HandleGravity()
    {
        //simple gravity when there no jump or anything else
        yVelocity = gravity * gravityMultiplier * Time.deltaTime;
        currentMovement.y = yVelocity;
    }

    private void HandleMovement()
    {
        //HandleAniamtion();
        float momentum = isMovementPressed ? acceleration : deceleration;
        
        //make player cary movement momentum when input is released
        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, 
                            transform.TransformDirection(targetVelocity), momentum * Time.deltaTime);
        
        currentMovement.x = horizontalVelocity.x;
        currentMovement.z = horizontalVelocity.z;

        characterController.Move(currentMovement * Time.deltaTime);
    }
    void OnEnable()
    {
        inputsystem_action.Player.Enable();
    }
    void OnDisable()
    {
        inputsystem_action.Player.Disable();
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
