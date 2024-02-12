using System;
using UnityEngine;

public sealed class CodeLockPawn : Pawn
{

    [SerializeField] private Transform _head;
    [SerializeField] private int code;

    public override Vector3 GetCameraPosition() => _head.position;
    public override Quaternion GetCameraRotation() => _head.rotation;

    public override void PossessedTick()
    {

    }

}
