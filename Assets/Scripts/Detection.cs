using UnityEngine;
using System.Collections;

public class Detection : MonoBehaviour
{

    public BaseAIUnit _myCharacter;
    public float _detectionRadius = 0.0f;

    void Awake()
    {
        GetComponent<SphereCollider>().radius = _detectionRadius;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _myCharacter.DetectCharacter(collider.gameObject.GetComponent<BaseWorldCharacter>());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        _myCharacter.DetectCharacter(null);
    }
}
