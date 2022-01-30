using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicCharacter", menuName = "ScriptableObjects/BasicCharacterData")]
public class CharacterData : ScriptableObject
{
    public int life = 100;
    public int damage = 25;
}
