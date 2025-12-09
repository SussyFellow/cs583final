using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float range = 100f; //Shooting Range
    public Camera fpsCam;      //Reference to our camera
    public GameObject gun;
    LineRenderer laserRender;
    public float shootCooldown;
    public float laserRenderTime;
    bool readyShoot;
    public AudioSource laserAudio;

    void Awake()
    {
        laserRender = GetComponent<LineRenderer>();
        laserRender.enabled = false;
        readyShoot = true;
    }    

    void Update()
    {
        //Fire1 is left-click by default in Unity
        if (Input.GetButtonDown("Fire1") && readyShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        readyShoot = false;

        if (laserAudio != null)
        {
            laserAudio.Play();
        }

        RaycastHit hit; //store info about what we hit

        Vector3[] laserPoints = new Vector3[2];
        laserPoints[0] = gun.transform.position + gun.transform.forward * 0.4f;

        //Shooting a ray from the camera's position forward in a straight line
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("I hit: " + hit.transform.name);

            laserPoints[1] = hit.point;

            //Checking if the object we hit has the Target script
            Target target = hit.transform.GetComponent<Target>();
            
            //If it does it will take damage
            if (target != null)
            {
                target.TakeDamage(hit);
            }
        }
        else
        {
            laserPoints[1] = fpsCam.transform.position + fpsCam.transform.forward * range;
        }

        laserRender.SetPositions(laserPoints);

        laserRender.enabled = true;
        Invoke(nameof(ResetShoot), shootCooldown);
        Invoke(nameof(UndrawLaser), laserRenderTime);

    }


    void UndrawLaser()
    {
        laserRender.enabled = false;
    }

    void ResetShoot()
    {
        readyShoot = true;
    }
}