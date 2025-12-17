using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ButtonAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    public float pressDepth = 0.2f; //How far down it pushes
    public float speed = 5.0f;      //How fast it moves
    public float resetDelay = 0.5f; //How long it stays down

    private Vector3 startPos;
    private Vector3 pressedPos;
    private bool isAnimating = false;

    void Start()
    {
        //Getting start position
        startPos = transform.localPosition;

        Vector3 pushDirection = transform.localRotation * Vector3.down;
        
        //Calculating down position
        pressedPos = startPos - (pushDirection * pressDepth);
    }

    public void Press()
    {
        if (!isAnimating)
        {
            StartCoroutine(AnimatePress());
        }
    }

    IEnumerator AnimatePress()
    {
        isAnimating = true;

        //Move Down
        while (Vector3.Distance(transform.localPosition, pressedPos) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, pressedPos, speed * Time.deltaTime);
            yield return null;
        }

        //Wait
        yield return new WaitForSeconds(resetDelay);

        //Move Up
        while (Vector3.Distance(transform.localPosition, startPos) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, speed * Time.deltaTime);
            yield return null;
        }

        //Snapping to start
        transform.localPosition = startPos;
        isAnimating = false;
    }
}