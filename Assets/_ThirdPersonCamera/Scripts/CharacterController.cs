﻿using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    public abstract void Move(Vector2 pMovementInput);

    public abstract void Jump();

    public abstract void Aim(Vector2 pAimInput);
}
