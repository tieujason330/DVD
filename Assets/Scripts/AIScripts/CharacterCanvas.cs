using UnityEngine;
using System.Collections;

public class CharacterCanvas : MonoBehaviour
{

    public Camera _mainCamera;

    void Awake()
    {
        //_mainCamera = GameObject.FindGameObjectWithTag(Consts.TAG_MAIN_CAMERA).GetComponent<Camera>();
    }

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        transform.LookAt(transform.position + _mainCamera.transform.rotation*Vector3.forward,
            _mainCamera.transform.rotation*Vector3.up);
    }
}