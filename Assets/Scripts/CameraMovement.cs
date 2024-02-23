using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // script taken and edited from my platformer tutorial project 


    // target will store the data of the object we want to move towards
    // we use serialize field so we can set this object from the editor
    [SerializeField] Transform target;

    // offset is the modifier by which we look at our target
    // example: if we wanted to look right of our target, we'd set x > 0
    [SerializeField] Vector3 offset;

    // this is the amount of smoothing (also called damping) applied to our camera
    // a smoothness of 0 means the camera follows the target exactly
    [SerializeField] float smoothness;

    // we are creating a shortcut variable to access a 0 vector (0, 0, 0)
    Vector3 velocity = Vector3.zero;

    // this is the final position to which we move (after the offset)
    Vector3 targetPos;

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    public void UpdateTarget(Transform new_target)
    {
        target = new_target;
    }

    // we will call this each frame to move the camera
    private void UpdatePosition()
    {
        // apply any offset
        targetPos = target.position + offset;

        // smooth the position to a new variable
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothness);

        // set our position to the smoothed position
        transform.position = smoothedPosition;
    }
}
