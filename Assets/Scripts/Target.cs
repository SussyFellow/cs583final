using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public void TakeDamage()
    {
        //Getting the Renderer component so we can change the material color
        Renderer render = GetComponent<Renderer>();
        
        //Changing the color to a random color
        render.material.color = Random.ColorHSV();
    }
}