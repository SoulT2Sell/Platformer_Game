using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("Shake Camera")]
    [SerializeField] private Vector2 shakeVelocity;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        instance = this;   
        
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void CameraShake(float shakeDir)
    {
        impulseSource.DefaultVelocity = new Vector2(shakeVelocity.x * shakeDir, shakeVelocity.y);
        impulseSource.GenerateImpulse();
    }
}
