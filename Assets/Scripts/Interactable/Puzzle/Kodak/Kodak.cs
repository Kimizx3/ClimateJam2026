using System;
using UnityEngine;

public class Kodak : MonoBehaviour
{
    public KodakEquip kodakEquip;

    private void Awake()
    {
        kodakEquip = GetComponent<KodakEquip>();
    }
}
