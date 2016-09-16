using UnityEngine;
using System.Collections;

public class ComboPoints : MonoBehaviour {

    PlayerCharacter _myCharacter;
    public GameObject _comboFill;

    public float _currentBarValue;
    public float _initialBarValue;

    void Awake()
    {
        _myCharacter = GetComponentInParent<PlayerCharacter>();
        _initialBarValue = _myCharacter._maxComboCount;
    }

    // Use this for initialization
    void Start()
    {
        //if (_myCharacter != null)
        //{
        //    SetComboPoints(0, 0);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (_myCharacter != null)
        //{
        //    ShowComboPoints();
        //}
    }

    public void ShowComboPoints()
    {
        _comboFill.transform.localScale = new Vector3(_currentBarValue, _comboFill.transform.localScale.y, _comboFill.transform.localScale.z);
    }

    public void SetComboPoints(float current, float max)
    {
        _currentBarValue = current / max;
        ShowComboPoints();
    }
}
