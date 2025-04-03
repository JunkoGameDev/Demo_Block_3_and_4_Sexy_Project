using UnityEngine;
using UnityEngine.Rendering;

public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float smoothTime;

    private void Awake()
    {
        
    }
    void LateUpdate()
    {
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        transform.LookAt(player);
    }
}
