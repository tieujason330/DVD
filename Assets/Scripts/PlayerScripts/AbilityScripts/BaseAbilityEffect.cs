using UnityEngine;
using System.Collections;

public class BaseAbilityEffect : MonoBehaviour {

    private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _emissionModule;
    private ActiveAbility _activeAbility;

    public GameObject _activationParent;

    void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _emissionModule = _particleSystem.emission;
        _activeAbility = GetComponentInParent<ActiveAbility>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (!_particleSystem.IsAlive())
	    {
	        Reset();
	    }
	}

    //void OnEnable()
    //{
    //    Execute();
    //}

    //void OnDisable()
    //{
    //    Reset();
    //}

    public void Execute()
    {
        if (_particleSystem.isPlaying)
            Reset();

        SetParent(_activationParent);
        
        _particleSystem.Simulate(0.0f, true, true);
        _emissionModule.enabled = true;
        _particleSystem.Play();
    }

    public void Reset()
    {
        _emissionModule.enabled = false;
        _particleSystem.Stop();
        ResetParent();
    }

    private void SetParent(GameObject newParent)
    {
        if (_activationParent == null)
            return;
        gameObject.transform.parent = newParent.transform;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up);
        gameObject.transform.localScale = newParent.transform.localScale;
    }

    private void ResetParent()
    {
        SetParent(_activeAbility.gameObject);
    }
}
