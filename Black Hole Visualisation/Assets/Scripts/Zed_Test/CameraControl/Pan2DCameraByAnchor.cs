using UnityEngine;

public class ZEDBodyAnchorPan2D : MonoBehaviour
{
    public enum AnchorType { Neck, HipsMidpoint, LeftHip, RightHip }
    public AnchorType anchor = AnchorType.Neck;

    public Transform leftHipTransform;   // assign for LeftHip or HipsMidpoint
    public Transform rightHipTransform;  // assign for RightHip or HipsMidpoint
    public Transform neckTransform;      // assign for Neck anchor

    [Header("Tuning")]
    public float sensitivity = 1.0f;
    public float smoothTime = 0.15f;
    public Vector2 deadzone = new Vector2(0.01f, 0.01f);

    Vector3 velocity;

    void LateUpdate()
    {
        Transform anchorTransform = null;

        switch (anchor)
        {
            case AnchorType.Neck:
                anchorTransform = neckTransform;
                break;
            case AnchorType.LeftHip:
                anchorTransform = leftHipTransform;
                break;
            case AnchorType.RightHip:
                anchorTransform = rightHipTransform;
                break;
            case AnchorType.HipsMidpoint:
                if (leftHipTransform != null && rightHipTransform != null)
                {
                    Vector3 mid = (leftHipTransform.position + rightHipTransform.position) * 0.5f;
                    // Create a temporary position vector
                    Vector3 targetPos = new Vector3(mid.x, mid.y, transform.position.z);
                    PanCamera(targetPos);
                    return;
                }
                break;
        }

        if (anchorTransform != null)
        {
            Vector3 targetPos = new Vector3(anchorTransform.position.x,
                                            anchorTransform.position.y,
                                            transform.position.z);
            PanCamera(targetPos);
        }
    }

    void PanCamera(Vector3 targetPos)
    {
        Vector3 camPos = transform.position;
        Vector2 delta = new Vector2(targetPos.x - camPos.x, targetPos.y - camPos.y);

        if (Mathf.Abs(delta.x) < deadzone.x) targetPos.x = camPos.x;
        if (Mathf.Abs(delta.y) < deadzone.y) targetPos.y = camPos.y;

        transform.position = Vector3.SmoothDamp(camPos,
                                                 camPos + (targetPos - camPos) * sensitivity,
                                                 ref velocity, smoothTime);
    }
}
