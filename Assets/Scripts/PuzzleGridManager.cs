using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleGridManager : MonoBehaviour
{
    [Header("Configuration")]
    public List<Target> gridSpheres;     //Drag grid spheres here
    public List<bool> solutionPattern;   //Check = ON (White), Uncheck = OFF (Black)

    [Header("Events")]
    public UnityEvent OnSolved;          //Open Door
    public UnityEvent OnUnsolved;        //Close Door

    public void CheckPuzzle()
    {
        if (gridSpheres.Count != solutionPattern.Count)
        {
            Debug.LogError("Error: Sphere count does not match Solution count!");
            return;
        }

        for (int i = 0; i < gridSpheres.Count; i++)
        {
            //Checking if the sphere's state matches the solution key
            if (gridSpheres[i].isGridSphereOn != solutionPattern[i])
            {
                OnUnsolved.Invoke(); 
                return; 
            }
        }

        OnSolved.Invoke();
    }
}