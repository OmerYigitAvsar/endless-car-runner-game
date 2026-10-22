using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private bool followTargetX;

    private Vector3 offset;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError(
                "CameraController: Takip edilecek target atanmamış.",
                this
            );

            enabled = false;
            return;
        }

        offset =
            transform.position -
            target.position;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        float targetX = followTargetX
            ? target.position.x + offset.x
            : transform.position.x;

        Vector3 targetPosition = new Vector3(
            targetX,
            transform.position.y,
            target.position.z + offset.z
        );

        float smoothValue =
            1f -
            Mathf.Exp(
                -followSpeed *
                Time.deltaTime
            );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothValue
        );
    }
}