using System;
using UnityEditor;
using UnityEngine;
    
public class SingleDirectionMovement : Movement
{
    [Header("Single Direction Movement")]
    public Vector3 direction;
    [SerializeField, Tooltip("Have the object face towards the direction it is going.")]
    private bool faceTowardsDirection = true;
    [Header("Debugging")]
    [SerializeField, Tooltip("Set how much the direction line length gets multiplied.")]
    private float directionLineMultiplier = 2f;

    protected override void Update()
    {
        base.Update();

        if (moving)
        {
            MoveSingleDirection();
        }
    }
    
    private void MoveSingleDirection()
    {
        if (faceTowardsDirection) transform.forward = direction.normalized;
        transform.transform.position += direction.normalized * (currentSpeed * Time.deltaTime);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.magenta;
        Handles.DrawLine(transform.position, transform.position + direction.normalized * directionLineMultiplier);
    }
    #endif
}