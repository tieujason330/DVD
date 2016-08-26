using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class BaseAIUnit : BaseWorldCharacter
{
    // Unit info
    public float _minAttackDamage = 0.0f;
    public float _maxAttackDamage = 0.0f;

    public BaseWorldCharacter _followCharacter = null;

    protected Animator _animator;

    void Awake()
    {
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract void PerformAction(string _action);
    public abstract void PerformOwnAction(string _action);
    public abstract void DetectCharacter(BaseWorldCharacter character);
}
