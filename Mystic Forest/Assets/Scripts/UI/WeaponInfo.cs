using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A ScriptableObject is a data container that you can use to save large amounts of data, 
 * independent of class instances. 
 * One of the main use cases for ScriptableObjects is to reduce your Project’s memory usage by avoiding copies of values. 
 * This is useful if your Project has a Prefab 
 * that stores unchanging data in attached MonoBehaviour scripts.*/

[CreateAssetMenu(menuName = "New Weapon")] 
public class WeaponInfo : ScriptableObject
{
    public GameObject weaponPrefab;
    public float weaponCooldown;
    public int weaponDam;
    public float weaponRange;

}
    
