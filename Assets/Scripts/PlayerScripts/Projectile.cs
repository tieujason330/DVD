using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

public class Projectile : MonoBehaviour
{
    private RangedWeapon _weapon;
    public GameObject _projectileContainer;
    public Vector3 _initialPosition;
    private bool _hasFired = false;

    void Awake()
    {
        _weapon = GetComponentInParent<RangedWeapon>();
        _projectileContainer = gameObject.transform.parent.gameObject;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (_hasFired)
	    {
	        transform.position += Time.deltaTime*_weapon._projectileSpeed*transform.forward;

	        if (Vector3.Distance(transform.position, _initialPosition) >= _weapon._projectileRange)
	        {
	            Unfired();
	        }
	    }
	}

    void OnTriggerEnter(Collider collider)
    {
        _weapon.GiveDamage(collider.gameObject.GetComponent<BaseWorldCharacter>());
        Unfired();
    }

    public void Fired()
    {
        _hasFired = true;
        gameObject.SetActive(true);
        gameObject.transform.parent = null;
        _initialPosition = _projectileContainer.transform.position;
        //gameObject.transform.position = _projectileContainer.transform.position;
    }

    void Unfired()
    {
        _hasFired = false;
        //need to make sure gameobject goes back to container w/ proper transform settings (is there a better way?)
        gameObject.transform.parent = _projectileContainer.transform;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.rotation = _projectileContainer.transform.rotation;
        gameObject.SetActive(false);
        _weapon.AddProjectile(this);
    }
}
