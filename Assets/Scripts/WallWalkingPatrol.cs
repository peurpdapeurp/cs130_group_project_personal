using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalkingPatrol : MonoBehaviour
{

    Transform myTransform;
    float speed = 5.0f;
    bool isWalking = true;
    Vector3 curNormal = Vector3.up;
    Vector3 hitNormal = Vector3.zero;
 
    private void Start()
    {
        myTransform = transform;
    }

    private void Update()
    {
        switch (isWalking)
        {
            case true:
                // check for wall
                RaycastHit rayHit;
                if (Physics.Raycast(myTransform.position, myTransform.forward * 1.0f, rayHit, 0.5))
                {
                    hitNormal = rayHit.normal;
                    isWalking = false;
                }
                Debug.DrawRay(myTransform.position, myTransform.forward, Color.red);    // show forward ray    

                // check for no floor    
                Vector3 checkRear = myTransform.position + (-myTransform.forward * 0.25f);
                if (Physics.Raycast(checkRear, -myTransform.up, rayHit, 1.0f))
                {
                    // there is a floor!
                }
                else
                {
                    // find the floor around the corner
                    Vector3 checkPos = myTransform.position + (myTransform.forward * 0.5f) + (-myTransform.up * 0.51f);
                    Debug.DrawRay(checkPos, -myTransform.forward * 1.5f, Color.green);    // show floor check ray
                    if (Physics.Raycast(checkPos, -myTransform.forward, rayHit, 1.5f))
                    {
                        Debug.Log("HitNormal " + rayHit.normal);
                        hitNormal = rayHit.normal;
                        isWalking = false;
                    }
                }
                Debug.DrawRay(myTransform.position, -myTransform.up * 1.0f, Color.red);    // show down ray
                                                                                            // move forward
                MoveForward();
                break;

            case false:
                curNormal = Vector3.Lerp(curNormal, hitNormal, 4.0f * Time.deltaTime);
                Quaternion grndTilt = Quaternion.FromToRotation(Vector3.up, curNormal);
                transform.rotation = grndTilt;
                float check = (curNormal - hitNormal).sqrMagnitude;
                if (check < 0.001)
                {
                    grndTilt = Quaternion.FromToRotation(Vector3.up, hitNormal);
                    transform.rotation = grndTilt;
                    isWalking = true;
                }
                break;
        }
    }

    private void MoveForward()
    {
        myTransform.position += transform.forward * speed * Time.deltaTime;
    }
}
