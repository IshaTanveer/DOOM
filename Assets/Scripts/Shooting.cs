using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    //raycast variables
    [SerializeField] Transform reticle;
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
    GunRecoil gunRecoil;
    void Awake()
    {
        gunRecoil = FindFirstObjectByType<GunRecoil>();
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

        gunRecoil.Recoil();
        AudioManager.Instance.PlaySounds(AudioManager.Sound.Shoot);
        ParticleManager.Instance.PlayParticles(ParticleManager.Particles.MuzzleFlash);

        Ray rayOrigin = Camera.main.ScreenPointToRay(reticle.position);
        RaycastHit hit;
        //if(Physics.Raycast(firePoint_Camera.position, firePoint_Camera.forward, out hit, range, layerMask))
        if(Physics.Raycast(rayOrigin, out hit, range, layerMask))
        {
            if(hit.collider.TryGetComponent(out DamageEnemy damageEnemy))
            {
                ParticleManager.Instance.InstantiateParticles(hit.point, hit.normal);
                damageEnemy.TakeHit(25f);
                Debug.Log(damageEnemy.name);
            }
        }
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
