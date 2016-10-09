using UnityEngine;
using System.Collections;

public class BaseAbilityEffect : MonoBehaviour {

    private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _emissionModule;
    private ActiveAbility _activeAbility;

    public GameObject _parent;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
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

    public void Execute()
    {
        if (_particleSystem.isPlaying)
        {
            Reset();
        }
        //gameObject.transform.parent = null;
        //gameObject.transform.position = GetComponentInParent<PlayerMain>().transform.position;
        //if (!_particleSystem.isPlaying)
        //{
        _particleSystem.Simulate(0.0f, true, true);
        _emissionModule.enabled = true;
        _particleSystem.Play();
        //}
        //else
        //{
        //    _emissionModule.enabled = false;
        //    _particleSystem.Stop();
        //}

        
    }

    public void Reset()
    {
        _emissionModule.enabled = false;
        _particleSystem.Stop();
        //gameObject.transform.parent = _activeAbility.gameObject.transform;
        //gameObject.transform.localPosition = Vector3.zero;
    }
}
