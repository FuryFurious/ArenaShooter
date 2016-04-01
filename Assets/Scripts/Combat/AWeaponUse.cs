using UnityEngine;
using System.Collections;

public abstract class AWeaponUse : MonoBehaviour 
{
    public abstract void Init(AWeapon spawner, Vector3 spawnPos, Vector3 lookDirection);
}
