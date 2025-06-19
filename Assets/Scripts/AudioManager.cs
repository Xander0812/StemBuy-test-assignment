using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public static Action PlayButtonClickAction = default;

    private void Start()
    {
        PlayButtonClickAction += ButtonClick;
    }

    private void ButtonClick()
    {
        audioSource.Play();
    }

    private void OnDestroy()
    {
        PlayButtonClickAction -= ButtonClick;
    }
}
