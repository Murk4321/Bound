using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Transform cameraTransform;  
    public float rotationSpeed = 5f; 

    private void Update()
    {
        transform.position = cameraTransform.position;
        Quaternion targetRotation = cameraTransform.rotation;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
