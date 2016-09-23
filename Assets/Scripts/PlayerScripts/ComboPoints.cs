using UnityEngine;
using System.Collections;

public class ComboPoints : MonoBehaviour {
    
    public GameObject _comboFill;

    public float _currentBarValue;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowComboPoints()
    {
        _comboFill.transform.localScale = new Vector3(_currentBarValue, _comboFill.transform.localScale.y, _comboFill.transform.localScale.z);
    }

    public void SetComboPoints(float current, float max)
    {
        if ((int) current == 0)
            _currentBarValue = 0;
        else
            _currentBarValue = current / max;
        ShowComboPoints();
    }
}
