using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float heightDamp = 2.0f;
    public float rotationDamp = 3.0f;
    public Vector3 startingPos; 

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position; 
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target) return;

        float requiredRotationAngle = target.eulerAngles.y;
        float requiredHeight = target.position.y + height;
        float initialRotationAngle = transform.position.y;
        float initialHeight = transform.position.y;

        initialRotationAngle = Mathf.LerpAngle(initialRotationAngle, requiredRotationAngle, rotationDamp + Time.deltaTime);
        initialHeight = Mathf.Lerp(initialHeight, requiredHeight, heightDamp * Time.deltaTime);

        var initialRotation = Quaternion.Euler(0, initialRotationAngle, 0);

        transform.position = target.position;
        transform.position -= Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, initialHeight, transform.position.z);
        transform.LookAt(target); 
    }
}
