using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotVariableModifier : MonoBehaviour
{
    public float checkInterval = 0.2f;
    public float seekerAC = 4;
    public float seekerACAlpha = 10;

    private DotBehaviour thisBehaviour;
    private DotBehaviour otherBehaviour;
    private float averageMoveSpeed;
    private float averageRotation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        thisBehaviour = GetComponent<DotBehaviour>();
        otherBehaviour = other.GetComponent<DotBehaviour>();
        averageMoveSpeed = (thisBehaviour.moveSpeed*thisBehaviour.dst.x + otherBehaviour.moveSpeed*otherBehaviour.dst.x) / (otherBehaviour.dst.x + thisBehaviour.dst.x);
        averageRotation = (thisBehaviour.currentRotationZ*thisBehaviour.dst.x + otherBehaviour.currentRotationZ*otherBehaviour.dst.x) / (otherBehaviour.dst.x + thisBehaviour.dst.x);

    }

    private Vector3 tmpVec;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (otherBehaviour != null && thisBehaviour != null)
        {

            if ((thisBehaviour.moveSpeed - averageMoveSpeed) > 0.1)
            {
                thisBehaviour.moveSpeed -= Time.fixedDeltaTime * seekerAC * Mathf.Abs(thisBehaviour.currentAcceleration);
            }
            else if ((thisBehaviour.moveSpeed - averageMoveSpeed) < (-0.1))
            {
                thisBehaviour.moveSpeed += Time.fixedDeltaTime * seekerAC * Mathf.Abs(thisBehaviour.currentAcceleration);
            }
            if ((otherBehaviour.moveSpeed - averageMoveSpeed) > 0.1)
            {
                otherBehaviour.moveSpeed -= Time.fixedDeltaTime * seekerAC * Mathf.Abs(otherBehaviour.currentAcceleration);
            }
            else if ((otherBehaviour.moveSpeed - averageMoveSpeed) < (-0.1))
            {
                otherBehaviour.moveSpeed += Time.fixedDeltaTime * seekerAC * Mathf.Abs(otherBehaviour.currentAcceleration);
            }

            if(Mathf.Abs((float)thisBehaviour.transform.eulerAngles.z - averageRotation) > 20)
            {
                tmpVec.x = 0;
                tmpVec.y = 0;
                tmpVec.z = averageRotation;
                thisBehaviour.transform.eulerAngles = tmpVec;
            }
            if(Mathf.Abs((float)otherBehaviour.transform.eulerAngles.z - averageRotation) > 20)
            {
                tmpVec.x = 0;
                tmpVec.y = 0;
                tmpVec.z = averageRotation;
                otherBehaviour.transform.eulerAngles = tmpVec;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Stopped");
    }

}
