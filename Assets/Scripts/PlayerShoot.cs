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
    public Light light;

    void Awake()
    {
        laserRender = GetComponent<LineRenderer>();
        laserRender.enabled = false;
        readyShoot = true;
        laserRender.positionCount = 2;
    }    

    void Update()
    {
        //Fire1 is left-click by default in Unity
        if (Input.GetButtonDown("Fire1") && readyShoot)
        {
            Shoot();
        }
        if (laserRender.enabled)
        {
            Vector3 temp = laserRender.GetPosition(1) - laserRender.GetPosition(0);
            laserRender.SetPosition(0, laserRender.GetPosition(0) + temp.normalized * 10f * Time.deltaTime);
        }
    }

    void Shoot()
    {
        readyShoot = false;
        light.enabled = true;

        if (laserAudio != null)
        {
            laserAudio.Play();
        }

        RaycastHit hit; //store info about what we hit

        laserRender.SetPosition(0, gun.transform.position + gun.transform.forward * 0.975f + gun.transform.up * 0.09f); //tip of laser gun barrel

        //Shooting a ray from the camera's position forward in a straight line
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            laserRender.SetPosition(1, hit.point);

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
            laserRender.SetPosition(1, fpsCam.transform.position + fpsCam.transform.forward * range);
        }

        laserRender.enabled = true;
        Invoke(nameof(ResetShoot), shootCooldown);
        Invoke(nameof(UndrawLaser), laserRenderTime);

    }


    void UndrawLaser()
    {
        laserRender.enabled = false;
        light.enabled = false;
    }

    void ResetShoot()
    {
        readyShoot = true;
    }
}