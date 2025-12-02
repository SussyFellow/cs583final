using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public enum targetType
    {
        Enemy,
        Other
    }
    public targetType type;


    public void TakeDamage(RaycastHit hit)
    {

        switch (type)
        {
            case targetType.Enemy:
                //TODO: fill in this with relevant code 
                EnemyMain script = hit.transform.GetComponent<EnemyMain>();
                script.GetShot(hit);
                break;
            default:
                //Getting the Renderer component so we can change the material color
                Renderer render = GetComponent<Renderer>();
                //Changing the color to a random color
                render.material.color = Random.ColorHSV();
                break;
        } 
    }
}