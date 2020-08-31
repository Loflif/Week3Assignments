using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static FollowCamera PlayerCamera { get; private set; }

    public static Transform PlayerTransform { get; private set; }

    private void Awake()
    {
        PlayerCamera = Camera.main.GetComponent<FollowCamera>();
        PlayerTransform = FindObjectOfType<PlayerInput>().transform;
    }
}
