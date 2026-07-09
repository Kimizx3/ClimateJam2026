using System;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    protected MovementComponent _movementComponent;
    protected LookComponent _lookComponent;

    protected virtual void Awake()
    {
        _movementComponent = GetComponent<MovementComponent>();
        _lookComponent = GetComponent<LookComponent>();
    }
}
