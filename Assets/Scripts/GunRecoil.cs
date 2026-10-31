using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [SerializeField] float RecoilAmount = 0.1f;
    [SerializeField] float RecoilSpeed = 5.0f;
    [SerializeField] float maxRecoil = 0.5f;
    //Quaternion StartRotation;
    [SerializeField] RectTransform reticle;
    [SerializeField] float reticleSensitivity = 500f;

    Vector3 startPosition;
    Vector3 recoil;
    Vector3 reticleTargetPosition;
    Vector3 reticleOffset;
    Vector3 originalReticalPosition;

    void Start()
    {
        startPosition = transform.localPosition;
        if (reticle != null) originalReticalPosition = reticle.anchoredPosition;
    }
    public void Recoil()
    {
        recoil = new Vector3(Random.Range(-RecoilAmount, RecoilAmount), Random.Range(-RecoilAmount, RecoilAmount), Random.Range(-RecoilAmount, RecoilAmount));
        recoil = Vector3.ClampMagnitude(recoil, maxRecoil);
    }
    void Update()
    {
        recoil = Vector3.Lerp(recoil, Vector3.zero, RecoilSpeed * Time.deltaTime);
        transform.localPosition = startPosition + recoil;
        if(reticle != null)
        {
            reticleOffset = new Vector2(recoil.x * reticleSensitivity * 100f, recoil.y * reticleSensitivity * 100f);
            reticleTargetPosition = originalReticalPosition + reticleOffset;
            reticle.anchoredPosition = Vector2.Lerp(reticle.anchoredPosition, reticleTargetPosition, RecoilSpeed * Time.deltaTime);
        }
    }
}
