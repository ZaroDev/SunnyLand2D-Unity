using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float parallaxX = 0.3f;
    public float parallaxY = 1f;

   
    void Update()
    {
      transform.position  = new Vector2(cam.position.x * parallaxX,  cam.position.y * parallaxY);
    }
}
