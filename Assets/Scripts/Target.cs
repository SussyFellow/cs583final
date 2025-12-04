using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    public enum targetType
    {
        Enemy,
        Button,
        GridSphere,
        Other
    }
    public targetType type;

    //For Button
    [Header("For Buttons Only")] public UnityEvent onShot;

    //For Grid Spheres
    [Header("For Grid Spheres Only")] 
    public bool isGridSphereOn = false;
    public PuzzleGridManager gridManager;
    public Color colorOn = Color.white;
    public Color colorOff = Color.black;
    
    public void TakeDamage(RaycastHit hit)
    {

        switch (type)
        {
            case targetType.Enemy:
                //TODO: fill in this with relevant code 
                EnemyMain script = hit.transform.GetComponent<EnemyMain>();
                script.GetShot(hit);
                break;
            case targetType.Button:
                onShot.Invoke();
                break;
            case targetType.GridSphere:
                ToggleGridSphere();
                break;
            default:
                //Getting the Renderer component so we can change the material color
                Renderer render = GetComponent<Renderer>();
                //Changing the color to a random color
                render.material.color = Random.ColorHSV();
                break;
        } 
    }
    
    
    
    //This function handles switching the colors and notifing the manager
    void ToggleGridSphere()
    {
        isGridSphereOn = !isGridSphereOn; //Flip sphere on or off

        //Updating Sphere color
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            r.material.color = isGridSphereOn ? colorOn : colorOff;
        }
        
        //Telling manager to check if correct
        if (gridManager != null)
        {
            gridManager.CheckPuzzle();
        }
    }
}