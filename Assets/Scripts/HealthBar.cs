using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public BaseWorldCharacter _character;
    public GameObject _healthBarFill;

    public float _currentBarValue;
    private bool _initialized = false;

    // Use this for initialization
    void Start()
    {
        if (_character != null)
            _initialized = true;
        _currentBarValue = _character._currentHealth / _character._initialHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float newBarValue = _character._currentHealth / _character._initialHealth;
        if (!_currentBarValue.Equals(newBarValue))
        {
            _currentBarValue = newBarValue;
            
        }
        SetHealthBar();
    }

    //public void InitializeHealthBar(BaseWorldCharacter character)
    //{
    //    _character = character;
    //    _currentBarValue = _character._currentHealth / _character._initialHealth;
    //    SetHealthBar();
    //    this.gameObject.SetActive(true);
    //    _initialized = true;
    //}

    public void SetHealthBar()
    {
        _healthBarFill.transform.localScale = new Vector3(_currentBarValue, _healthBarFill.transform.localScale.y, _healthBarFill.transform.localScale.z);
    }
}
