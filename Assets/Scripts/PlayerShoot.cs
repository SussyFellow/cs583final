using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float range = 100f; //Shooting Range
    public Camera fpsCam;      //Reference to our camera

    void Update()
    {
        //Fire1 is left-click by default in Unity
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit; //store info about what we hit

        //Shooting a ray from the camera's position forward in a straight line
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("I hit: " + hit.transform.name);

            //Checking if the object we hit has the Target script
            Target target = hit.transform.GetComponent<Target>();
            
            //If it does it will take damage
            if (target != null)
            {
                target.TakeDamage(hit);
            }
        }
    }
}