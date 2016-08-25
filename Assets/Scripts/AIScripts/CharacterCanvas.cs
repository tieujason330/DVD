using UnityEngine;
using System.Collections;

public class CharacterCanvas : MonoBehaviour
{

    public Camera m_Camera;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation*Vector3.forward,
            m_Camera.transform.rotation*Vector3.up);
    }
}