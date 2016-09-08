using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    BaseWorldCharacter _myCharacter;
    public GameObject _healthBarFill;

    public float _currentBarValue;

    void Awake()
    {
        _myCharacter = GetComponentInParent<BaseWorldCharacter>();
    }

    // Use this for initialization
    void Start()
    {
        if (_myCharacter != null)
            _currentBarValue = _myCharacter._currentHealth / _myCharacter._initialHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (_myCharacter != null)
        {
            float newBarValue = _myCharacter._currentHealth/_myCharacter._initialHealth;
            if (!_currentBarValue.Equals(newBarValue))
            {
                _currentBarValue = newBarValue;

            }
            SetHealthBar();
        }
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

    public void SetCharacter(BaseWorldCharacter character)
    {
        _myCharacter = character;
    }
}
