using UnityEngine;
using System.Collections;

public abstract class ASkill : MonoBehaviour 
{
    public abstract void Init(SkillActivator spawner, Vector3 spawnPos, Vector3 lookDirection);
}
