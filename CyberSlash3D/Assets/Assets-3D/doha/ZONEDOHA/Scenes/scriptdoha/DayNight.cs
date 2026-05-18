using UnityEngine;

public class DayNight : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Light light;
    public float rotationSpeed;
    private void update()
    {
        light.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

    }

}
