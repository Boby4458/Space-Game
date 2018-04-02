using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (fileName = "New Ship", menuName = "New Ship")]
public class Spaceship : ScriptableObject{

    public string name;
    public string rarity;
    public Mesh spaceShipModel;

}
