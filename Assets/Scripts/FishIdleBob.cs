using UnityEngine;

public class FishIdleBob : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     public float bobAmplitude = 0.1f;  // How high the bobbing goes
    public float bobFrequency = 2f;    // How fast the bobbing is

    private Vector3 startPos;
    void Start()
    {
         startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}

