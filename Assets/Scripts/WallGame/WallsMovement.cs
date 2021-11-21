using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsMovement : MonoBehaviour
{
    public float speed = 10f;

   
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.back);
    }
}
