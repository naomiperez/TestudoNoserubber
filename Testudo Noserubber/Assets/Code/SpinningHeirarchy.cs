using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningHeirarchy : MonoBehaviour
{
    public int rotateSpeed = 15;
    public float lockAxis = 0;

    private Vector2 parentCenter;
    public float parentRotateSpeed;
    private float radius;
    private float angle;
    private Vector3 force;
    public
    

    void Start()
    {
        force = new Vector3(0f, 0f, .1f);

        parentCenter = transform.parent.position;
        radius = 1.5f;
        parentRotateSpeed = 1f;
    }

    void Update()
    {

        /* center block (parent) rotation */
        angle += parentRotateSpeed * Time.deltaTime;
        var offset = new Vector2 (Mathf.Sin (angle), Mathf.Cos (angle)) * radius;
        transform.parent.position = parentCenter + offset;

        transform.RotateAround(transform.parent.position, new Vector3(0, 0, 1), rotateSpeed * Time.deltaTime);

        /* Lock X and Z axis for spinning platforms so that they always face up */
        transform.rotation = Quaternion.Euler(lockAxis, lockAxis, lockAxis);
    }

}
