using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Inherit from this class to define "Bite" behaviiour
// Bite defins what an object should do when the mouth object interacts with it
// Mouth cannot bite an object that does not have this component attached

public abstract class Edible : MonoBehaviour
{
    public abstract void Bite();
}
