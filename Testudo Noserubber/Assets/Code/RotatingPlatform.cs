using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public int speed = 100;
    public bool counterClockwise = true;
  

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, (counterClockwise ? 1 : -1 ) * speed * Time.deltaTime));
    }

}
