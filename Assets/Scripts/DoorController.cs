using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3, 0); //Where is moves between open and closed
    public float speed = 2.0f;
    
    private bool isOpen = false;
    private Vector3 closedPos; //Starts closed
    private Vector3 openPos;

    void Start()
    {
        //Save the starting positions
        closedPos = transform.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        //Decide where we want to be right now
        Vector3 destination = isOpen ? openPos : closedPos;

        //Move towards that destination smoothly
        //Vector3.MoveTowards automatically stops when it reaches the target
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    //Call this to open
    public void OpenDoor()
    {
        isOpen = true;
    }

    //Call this to close
    public void CloseDoor()
    {
        isOpen = false;
    }

    //Call this to swap between open and closed
    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}