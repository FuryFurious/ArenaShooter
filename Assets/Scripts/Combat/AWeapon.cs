using UnityEngine;
using System.Collections;

public abstract class AWeapon : MonoBehaviour 
{
    public enum FireMode { Primary, Secondary }


    public abstract void OnClick(FireMode mode);

    public abstract void OnPress(FireMode mode);

    public abstract void OnRelease(FireMode mode);

}
