﻿using UnityEngine;
using System.Collections;

public class ActiveAbility : BaseAbility
{
    public int _activeCost;
    private BaseAbilityEffect _activeEffect;

    void Awake()
    {
        _activeEffect = GetComponentInChildren<BaseAbilityEffect>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void Execute()
    {
        if (_activeEffect != null)
            _activeEffect.Execute();
    }
}
