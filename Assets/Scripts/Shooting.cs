using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    //raycast variables
    [SerializeField] Transform firePoint_Camera;
    [SerializeField] float range = 100f;
    [SerializeField] LayerMask layerMask;
    // input variables
    InputSystem_Actions inputSystem_Actions;
    bool isAttackPressed;
    //shooting variables 
    [SerializeField] bool isSemiAutomatic = false;
    [SerializeField] float shootDelay = 0.3f;
    float nextShootTime;
    // particles
    GameObject muzzleParticle; 
    GameObject HitParticle;
    //automatic
    void Awake()
    {
        inputSystem_Actions = new InputSystem_Actions();
        inputSystem_Actions.Player.Attack.performed += ReadAttackInput;
        inputSystem_Actions.Player.Attack.canceled += ReadAttackInput;
    }
    void ReadAttackInput(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();
        //callback is called only when the input change so wew ill do semi automatic hooting here
        //semiautomatic shooting: only when fire per click like a shotgun 
        if(isSemiAutomatic && isAttackPressed) HandleShooting();
    }
    void Update() 
    {
        //Automatic shooting: keep firing when input is held
        if (!isSemiAutomatic && isAttackPressed) HandleShooting();
    }
    void HandleShooting()
    {
        //shoot
        if(Time.time < nextShootTime) return;
        nextShootTime = Time.time + shootDelay;
        RaycastHit hit;
        if(Physics.Raycast(firePoint_Camera.position, firePoint_Camera.forward, out hit, range, layerMask))
        {
            if(hit.collider.TryGetComponent(out DamageEnemy damageEnemy))
            {
                damageEnemy.TakeHit(25f);
                Debug.Log(damageEnemy.name);
            }
            Debug.DrawRay(firePoint_Camera.position, firePoint_Camera.forward * hit.distance, Color.yellow);
        }
        else Debug.DrawRay(firePoint_Camera.position, firePoint_Camera.forward * range, Color.red);
    }
    void OnEnable()
    {
        inputSystem_Actions.Player.Enable();
    }
    void OnDisable()
    {
        inputSystem_Actions.Player.Disable();
    }
}
