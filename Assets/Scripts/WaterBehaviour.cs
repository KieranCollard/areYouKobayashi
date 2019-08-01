using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    private Transform topOfWater;
    private ParticleSystem waterParticles;

    public int sloshSpeed = 60;
    public int maxDifference = 25;
    public int spillAngle = 45;

    Vector3 startingRotation;
    // Start is called before the first frame update
    void Start()
    {
        topOfWater = transform.Find("topOfWater");
        waterParticles = topOfWater.Find("waterParticles").GetComponent<ParticleSystem>();

        if(topOfWater == null)
        {
            Debug.Log("The top of water object was null. Make sure this prefab has a child named topOfWater");
        }
        if (waterParticles == null)
        {
            Debug.Log("The water particles was null. Make sure this prefab has a child named waterParticles that is a particle emitter");
        }

        startingRotation = this.transform.rotation.eulerAngles;
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
        if(Mathf.Abs((startingRotation - this.transform.rotation.eulerAngles).x) > spillAngle)
        {
            if (waterParticles.isPlaying == false)
            {
                waterParticles.Play();
            }
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
