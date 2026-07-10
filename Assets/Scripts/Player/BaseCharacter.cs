using System;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    protected MovementComponent movementComponent;
    protected LookComponent lookComponent;
    protected Kodak kodak;

    protected virtual void Awake()
    {
        kodak = GetComponent<Kodak>();
        movementComponent = GetComponent<MovementComponent>();
        lookComponent = GetComponent<LookComponent>();
    }
}
