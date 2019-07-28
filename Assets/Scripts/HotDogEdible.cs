using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotDogEdible : Edible
{
    public int totalNumberOfBites = 3;
    private int bitesRemaining;
    // Start is called before the first frame update
    void Start()
    {
        bitesRemaining = totalNumberOfBites;
    }
#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Bite();
        }
    }
#endif
    public override void Bite()
    {
        --bitesRemaining;
        if(bitesRemaining <=0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Vector3 currentScale = this.transform.localScale;
            // get shorther based on hoe many bites remaining
            /// TODO figure out a better effect. Maybe a bite cutout? 
            Vector3 scale = new Vector3(currentScale.x, currentScale.y, currentScale.z / totalNumberOfBites);
            this.transform.localScale = scale;
        }
    }
}
