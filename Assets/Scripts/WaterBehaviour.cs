using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    private Transform topOfWater;
    private ParticleSystem waterParticles;
    private Transform bottomMarker;

    public int sloshSpeed = 60;
    public int maxDifference = 25;
    public int spillAngle = 45;
    //how fast the water should empty out of the glass

    public float spillSpeed = 1;
    private float percentFull = 1.0f;

    Vector3 startingRotation;
    Vector3 topOFWaterStartingPosition;
    // Start is called before the first frame update
    void Start()
    {
        topOfWater = transform.Find("topOfWater");
        waterParticles = topOfWater.Find("waterParticles").GetComponent<ParticleSystem>();
        bottomMarker = transform.Find("bottomMarker");

        if(topOfWater == null)
        {
            Debug.Log("The top of water object was null. Make sure this prefab has a child named topOfWater");
        }
        if (waterParticles == null)
        {
            Debug.Log("The water particles was null. Make sure this prefab has a child named waterParticles that is a particle emitter");
        }
        if(bottomMarker == null)
        {
            Debug.Log("The bottom marker could not be found. THis object is needed for easy lookup of the bottom of the glass");
        }

        startingRotation = this.transform.rotation.eulerAngles;
        topOFWaterStartingPosition = topOfWater.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //wate in a cup https://www.youtube.com/watch?v=OBnCd1bzCcw 
       
        //glass has icnorrect rotations
        Vector3 localRotationCopy = transform.localRotation.eulerAngles;
        localRotationCopy.x += 90;
        Quaternion inverse = Quaternion.Inverse(Quaternion.Euler(localRotationCopy));
        Vector3 finalRotation = Quaternion.RotateTowards(topOfWater.transform.localRotation, inverse, sloshSpeed * Time.deltaTime).eulerAngles;

        finalRotation.x = ClampRotation(finalRotation.x, maxDifference);
        finalRotation.y = ClampRotation(finalRotation.y, maxDifference);
        

        topOfWater.transform.localEulerAngles = finalRotation;
        //if we are tipper over far enough start the particles
        if(Mathf.Abs((startingRotation - this.transform.rotation.eulerAngles).x) > spillAngle && percentFull > 0)
        {
            if (waterParticles.isPlaying == false)
            {
                waterParticles.Play();
            }

            // move the top of water down.
            // and empty
            percentFull -= (spillSpeed * Time.deltaTime);
            float lerpedZ  = Mathf.Lerp(topOFWaterStartingPosition.z, bottomMarker.localPosition.z, 1 - percentFull);
            topOfWater.transform.localPosition = new Vector3(topOfWater.transform.localPosition.x, topOfWater.transform.localPosition.y, lerpedZ);
            
        }
        else
        {
            if(waterParticles.isPlaying)
            {
                waterParticles.Stop();
            }
        }
    }

    private float ClampRotation(float value, float difference)
    {
        float returnValue = 0.0f;
        if(value > 180 )
        {
            returnValue = Mathf.Clamp(value, 360 - difference, 360);
        }
        else
        {
            returnValue = Mathf.Clamp(value, 0, difference);
        }
        return returnValue;
    }
}
