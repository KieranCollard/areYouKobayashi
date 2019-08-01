using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// Behaviour to interact with other objects containing an matching 'edible' script
// When trigger is overlapping will attempt to findcorresponding script
// When controller input is detected will call 'bite' function on all overlapping 'edibe' objects
public class MouthBite : MonoBehaviour
{
    private Dictionary<int, Edible> edibles;
    private void Start()
    {
        edibles = new Dictionary<int, Edible>();
    }
    // Update is called once per frame
    void Update()
    {
        //OVRInput.Update();
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {

            foreach (Edible edible in edibles.Values)
            {
                edible.Bite();
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
        {
            if (edibles != null)
            {
                foreach (Edible edible in edibles.Values)
                {
                    edible.Bite();
                }
            }
        }
#endif
    }

    //track on enter
    private void OnTriggerEnter(Collider other)
    {
        var edibleScript = other.transform.GetComponent<Edible>();
        if (edibleScript != null)
        {
            edibles.Add(other.GetInstanceID(), edibleScript);
        }
    }

    //forget on exit
    private void OnTriggerExit(Collider other)
    {
        if (edibles.ContainsKey(other.GetInstanceID()))
        {
            edibles.Remove(other.GetInstanceID());
        }
    }
}
