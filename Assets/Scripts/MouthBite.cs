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
        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {

            GameObject.Find("Clicked").GetComponent<UnityEngine.UI.Text>().text = "Detected click";
            foreach (Edible edible in edibles.Values)
            {
                GameObject.Find("Overlap printout").GetComponent<UnityEngine.UI.Text>().text = "Eating";
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
        Debug.Log("Attached edible script" + edibleScript);
        if (edibleScript != null)
        {
            Debug.Log("Item entered mouth " + other.gameObject.name);
            edibles.Add(other.GetInstanceID(), edibleScript);
            GameObject.Find("Grab Attempt").GetComponent<UnityEngine.UI.Text>().text = other.gameObject.name;
        }
    }

    //forget on exit
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Item exited mouth " + other.name);
        Debug.Log(edibles.Values);
        if (edibles.ContainsKey(other.GetInstanceID()))
        {
            edibles.Remove(other.GetInstanceID());
            GameObject.Find("Grab Attempt").GetComponent<UnityEngine.UI.Text>().text = "";
        }
    }
}
