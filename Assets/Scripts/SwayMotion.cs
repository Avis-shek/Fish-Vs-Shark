using UnityEngine;

public class SwayMotion : MonoBehaviour
{
    public float swayAmplitude = 10f; // degrees of rotation
    public float swaySpeed = 2f;      // speed of sway

    private float initialZ;

    void Start()
    {
        initialZ = transform.eulerAngles.z;
    }

    void Update()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmplitude;
        float angle = initialZ + sway;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

