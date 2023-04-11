using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterType", menuName = "Characters/Character Type")]
public class CharacterScriptableObject : ScriptableObject
{

    public string characterName;
    public int maximumHealth;
    public int attackDamage;
    public SpriteRenderer sprite;
}
