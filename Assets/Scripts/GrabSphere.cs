using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// very basic 'grab' of objects we overlap
// set there position to our posiiton
public class GrabSphere : MonoBehaviour
{
    Dictionary<int, Transform> grabbedObjects;
    Dictionary<int, Transform> overlappedObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            grabbedObjects = overlappedObjects;
        }
        else if(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            grabbedObjects.Clear();
        }

        foreach(Transform trans in grabbedObjects.Values)
        {
            trans.position = this.transform.position;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<VrGrabber.VrgGrabbable>())
        {
            overlappedObjects.Add(other.transform.GetInstanceID(), other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        overlappedObjects.Remove(other.transform.GetInstanceID());
    }
}
