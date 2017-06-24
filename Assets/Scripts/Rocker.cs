using UnityEngine;

public class Rocker : MonoBehaviour
{
    public float Freq = 1f;
    public float Amp = 1f;

    void Update()
    {
        Vector3 angles = this.transform.eulerAngles;
        angles.z = Mathf.Sin(Freq * Time.time) * Amp;
        this.transform.rotation = Quaternion.Euler(angles);
    }
}