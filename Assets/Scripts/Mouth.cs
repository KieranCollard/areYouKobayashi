using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// Behaviour to interact with other objects containing an matching 'edible' script
// When trigger is overlapping will attempt to findcorresponding script
// When controller input is detected will call 'bite' function on all overlapping 'edibe' objects
// Also contains logic of 'full mouth' 
// and TODO may trigger the chocking state
public class Mouth : MonoBehaviour
{
    //editor exposed
    //number of bites before mouth is full
    public int mouthCapcity = 2;
    // chance that choking will happen when trying to sswallow before any other factors are considered
    // value of 0 -1
    public float baseChokingChance = 0.1f;

    // Time in secconds before player loses the game from choking
    public float timeBeforeLoss = 10;
    float timeInChokeState = 0;
    private bool backingChoking;
    public bool isChoking
    {
        get
        {
            return backingChoking;
        }
        private set
        {
            backingChoking = value;
        }
    }

    //currently known edible scripts
    private Dictionary<int, Edible> edibles;

    //bites bites taken in between swallows
    private int currentBites = 0;

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
            TakeBites();
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            Swallow();
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
        {
            TakeBites();
        }
        else if (Input.GetMouseButtonDown(2))
        {
            Swallow();
        }
#endif
    }

    private void FixedUpdate()
    {
        if (isChoking)
        {
            timeInChokeState += Time.fixedDeltaTime;

            if (timeInChokeState >= timeBeforeLoss)
            {
                //TODO menu loop and proper end game state
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
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

    private void TakeBites()
    {
        Debug.Log(edibles);
        if (edibles != null)
        {
            //while there is something to bite send signal to bite it
            ArrayList array = new ArrayList();
            foreach (var value in edibles.Values)
            {

                //edibles can destroy themselves. Check that hasn;t happened
                //TODO events passing between these two objects might eliminate this check
                if (value == null)
                {
                    edibles.Remove(value.GetInstanceID());
                }
                else
                {
                    value.Bite();
                    ++currentBites;
                }
            }

            if (currentBites >= mouthCapcity)
            {
                //TODO better way to signal player
                GameObject.Find("Player Warnings").GetComponent<UnityEngine.UI.Text>().text = "Your mouth is full \n Press Button2 to swallow or you might choke";
            }
        }
    }

    private void Swallow()
    {
        // calculate a likelyhood to choke based on how full the mouth currently is. 
        // offset by how 'big' the  mouth is
        float likelyHoodOFChoking = baseChokingChance * (currentBites - mouthCapcity);
        if (100.0f - Random.Range(likelyHoodOFChoking, 100.0f) < 0.1f)
        {
            BeginChoking();
        }
        currentBites = 0;
        GameObject.Find("Player Warnings").GetComponent<UnityEngine.UI.Text>().text = "Chance of choking = " + (likelyHoodOFChoking * 100.0f).ToString() + "%";
    }

    private void BeginChoking()
    {
        isChoking = true;
        //TODO trigger feedbak to player... probably needd an event system at some point

        GameObject.Find("Player Warnings").GetComponent<UnityEngine.UI.Text>().text = "You're choking drink some water";
    }

    private void StopChoking()
    {
        isChoking = false;
        //TODO stop player feedback audio / visuals
        GameObject.Find("Player Warnings").GetComponent<UnityEngine.UI.Text>().text = "";
        timeInChokeState = 0.0f;
    }
}
