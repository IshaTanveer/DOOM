using System;
using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public enum Particles {MuzzleFlash, ImpactHit};
    [SerializeField] private ParticleSystem[] particlesSystem;
    public static ParticleManager Instance {get; private set;}
    [SerializeField] ParticleSystem ImpactParticles;
    private void Awake() {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PlayParticles(Particles particles)
    {
        particlesSystem[(int)particles].Play();
    }
    public void InstantiateParticles(Vector3 position, Vector3 normal)
    {
        ParticleSystem impact = Instantiate(ImpactParticles, position, Quaternion.LookRotation(normal));
        StartCoroutine(DestroyParticles(impact));
    }
    IEnumerator DestroyParticles(ParticleSystem impact)
    {
        yield return new WaitForSeconds(1f);
        Destroy(impact.gameObject);
    }
}
