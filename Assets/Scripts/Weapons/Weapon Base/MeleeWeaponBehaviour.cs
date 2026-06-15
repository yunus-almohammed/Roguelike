using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base script of all melee behaviours [To be placed on a prefab of a weapon that is melee]
/// </summary>
public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    public float destroyAfterSeconds;

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

}