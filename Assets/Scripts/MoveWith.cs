using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWith : MonoBehaviour
{
    public Transform followPosition;
    void Update()
    {
        transform.position = followPosition.position;
    }
}
