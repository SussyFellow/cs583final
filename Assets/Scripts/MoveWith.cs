using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWith : MonoBehaviour
{
    //this script is just intended to have one object set its position to another object's position every frame
    public Transform followPosition;
    void Update()
    {
        transform.position = followPosition.position;
    }
}
