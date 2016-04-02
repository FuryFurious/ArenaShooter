using UnityEngine;
using System.Collections;

public class PlayerStartPoint : MonoBehaviour
{
    public int PlayerIndex = -1;

    void Start()
    {
        WorldManager.Instance.RegisterPlayerStartPoint(this);
    }
}
