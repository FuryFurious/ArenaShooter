using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

[Serializable]
public class HealthChangedEvent : UnityEvent<HealthManager, int>
{ 
}
