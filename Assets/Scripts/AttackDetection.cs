﻿using System;
using UnityEngine;
using System.Collections;

public class AttackDetection : MonoBehaviour {

    public BaseAIUnit _myCharacter;
    public float _attackDetectionRadius = 0.0f;

    void Awake()
    {
        GetComponent<SphereCollider>().radius = _attackDetectionRadius;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider collider)
    {
        try
        {
            if (collider.gameObject.GetComponent<BaseWorldCharacter>() != null &&
                collider.gameObject.GetComponent<BaseWorldCharacter>()._faction != _myCharacter._faction)
            {
                _myCharacter.AttackDetectionCharacter(collider.gameObject.GetComponent<BaseWorldCharacter>());
            }
        }
        catch (NullReferenceException ex)
        {

        }
    }

    void OnTriggerExit(Collider collider)
    {
        _myCharacter.AttackDetectionCharacter(null);
    }
}