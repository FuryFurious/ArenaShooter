using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance { get; private set; }

    public AWeapon DefaultWeaponPrefab;

    public FollowPlayer PlayerCam;

    [SerializeField]
    private Material[] playerMaterial;
    
    public Material GetMaterial(int index)
    {
        return playerMaterial[index];
    }

    void Awake()
    {
        Debug.Assert(!Instance);
        Instance = this;
    }
}
