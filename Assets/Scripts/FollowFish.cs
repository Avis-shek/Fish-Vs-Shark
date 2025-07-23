using UnityEngine;

public class FollowFish: MonoBehaviour
{
    public Transform fish;
    public Vector3 offset;

    void LateUpdate()
    {
        if (fish != null)
        {
            transform.position = fish.position + offset;
            transform.rotation = Quaternion.identity;
        }
    }
}
