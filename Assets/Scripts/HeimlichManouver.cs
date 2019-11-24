using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeimlichManouver : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject playerBody;
    SphereCollider collisionArea;
    Rigidbody rigidBody;
    public GameObject hotDogPrefab;
    int hotDogMaxBites =1; //used so that we can scale hot dog to smaller size
    public int vomitSpeed =10;

    //have to hit yourself hard to perform a heimlich
    public float miniumVelocity = 1.0f;
    //percant of a heinlich succeeding (0...1)
    public float baseChanceOfSuccess = 0.5f;

    Mouth mouthReference;

    // Start is called before the first frame update
    void Start()
    {
        if (rightHand == null && leftHand == null)
        {
            Debug.LogError("The heinlich script must be able to track at least one hand");
        }
        if (playerBody == null)
        {
            Debug.LogError("The heinlich manouver must be able to track the players body");
        }

        mouthReference = playerBody.GetComponent<Mouth>();
        if (mouthReference == null)
        {
            mouthReference = playerBody.GetComponentInChildren<Mouth>();
            if (mouthReference == null)
            {
                Debug.LogError("The hemlich was not able to find mouth Script. This is required to check for choking");
            }

        }
        if(hotDogPrefab == null)
        {
            Debug.LogError("Must have a reference to the hot dog prefab to spawn if we spit one out");
        }
        else
        {
            hotDogMaxBites = hotDogPrefab.GetComponent<HotDogEdible>().totalNumberOfBites;
        }
        //always be a trigger to avoid interferance with other objects
        collisionArea = this.gameObject.AddComponent<SphereCollider>();
        collisionArea.isTrigger = true;
        //we need OnCollisionevents, but onyl when we are active 
        rigidBody = this.GetComponent<Rigidbody>();
        if(rigidBody == null)
        {
            rigidBody = this.gameObject.AddComponent<Rigidbody>();
        }

        //internally we will compare squared values
        miniumVelocity *= miniumVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        //only enable heinlich trigger when in choking state
        if (mouthReference.isChoking == false)
        {
            collisionArea.enabled = false;
        }
        else
        {
            collisionArea.enabled = true;
            GameObject.Find("Player Warnings").GetComponent<UnityEngine.UI.Text>().text = "Try the heimlich to remove stop choking";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject.Find("Player Warnings").GetComponent<UnityEngine.UI.Text>().text = "Detected a collision in heinlich script";
        if (collision.gameObject == leftHand || collision.gameObject == rightHand)
        {
            if (collision.relativeVelocity.sqrMagnitude >= miniumVelocity)
            {
                //todo it could be fun to increase chance of success based on velocity
                if(Random.Range(0.0f, 1.0f) > baseChanceOfSuccess)
                {
                    //shoot out a peice of food
                    GameObject hotDog = GameObject.Instantiate(hotDogPrefab);
                    //easiest way to obtain correct scale is to call 'bite' a number of times and let it scale itself
                    HotDogEdible dog = hotDog.GetComponent<HotDogEdible>();
                    for (int i = 0; i < hotDogMaxBites -1; ++i)
                    {
                        dog.Bite();
                    }
                    hotDog.transform.GetComponent<Rigidbody>().AddForce(playerBody.transform.forward * vomitSpeed, ForceMode.Impulse);
                    GameObject.Find("Player Warnings").GetComponent<UnityEngine.UI.Text>().text = "Succsessful heimlich";
                }
            }
        }
    }
}
