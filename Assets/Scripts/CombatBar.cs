using UnityEngine;
using System.Collections;

public class CombatBar : MonoBehaviour {

    PlayerMain _myCharacter;
    public GameObject _combatBarFill;

    public float _currentBarValue;

    void Awake()
    {
        _myCharacter = GetComponentInParent<PlayerMain>();
    }

    // Use this for initialization
    void Start()
    {
        if (_myCharacter != null)
            _currentBarValue = _myCharacter._currentCombatPoints / _myCharacter._initialCombatPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (_myCharacter != null)
        {
            float newBarValue = _myCharacter._currentCombatPoints / _myCharacter._initialCombatPoints;
            if (!_currentBarValue.Equals(newBarValue))
            {
                _currentBarValue = newBarValue;

            }
            SetCombatBar();
        }
    }

    public void SetCombatBar()
    {
        _combatBarFill.transform.localScale = new Vector3(_currentBarValue, _combatBarFill.transform.localScale.y, _combatBarFill.transform.localScale.z);
    }

    public void SetCharacter(PlayerMain character)
    {
        _myCharacter = character;
    }
}
