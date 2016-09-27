using UnityEngine;
using System.Collections;

public class ComboTimer : MonoBehaviour
{
    PlayerCombat _myCharacter;
    public GameObject _comboTime;

    public float _currentBarValue;
    public float _initialBarValue;

    void Awake()
    {
        _myCharacter = GetComponentInParent<PlayerCombat>();
    }

    // Use this for initialization
    void Start()
    {
        if (_myCharacter != null)
            _currentBarValue = _myCharacter._attackCurrentComboTimer / _myCharacter._attackInitialComboTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_myCharacter != null)
        {
            float newBarValue = _myCharacter._attackCurrentComboTimer / _myCharacter._attackInitialComboTimer;
            _currentBarValue = newBarValue;
            SetComboTime();
        }
    }

    public void SetComboTime()
    {
        _comboTime.transform.localScale = new Vector3(_currentBarValue, _comboTime.transform.localScale.y, _comboTime.transform.localScale.z);
    }

    //public void SetCharacter(BaseWorldCharacter character)
    //{
    //    _myCharacter = character;
    //}
}

