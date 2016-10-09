using UnityEngine;
using System.Collections;

public class StaminaBar : MonoBehaviour {

    PlayerMain _myCharacter;
    public GameObject _staminaBarFill;
    public GameObject _staminaBarFillEndPoint;

    public GameObject _potentialStaminaGainBarFill;

    public float _currentStaminaBarValue;
    public float _potentialStaminaGainBarValue;

    void Awake()
    {
        _myCharacter = GetComponentInParent<PlayerMain>();
    }

    // Use this for initialization
    void Start()
    {
        if (_myCharacter != null)
        {
            _currentStaminaBarValue = _myCharacter._currentStamina/_myCharacter._initialStamina;
            _potentialStaminaGainBarValue = _myCharacter._potentialStaminaRegain/_myCharacter._initialStamina;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_myCharacter != null)
        {
            float newStaminaBarValue = _myCharacter._currentStamina / _myCharacter._initialStamina;
            float newPotentialBarValue = _myCharacter._potentialStaminaRegain/_myCharacter._initialStamina;

            _currentStaminaBarValue = newStaminaBarValue > 1.0f ? 1.0f : newStaminaBarValue;
            _potentialStaminaGainBarValue = newPotentialBarValue;

            SetStaminaBar();
        }
    }

    public void SetStaminaBar()
    {
        _staminaBarFill.transform.localScale = new Vector3(_currentStaminaBarValue, 
            _staminaBarFill.transform.localScale.y,
            _staminaBarFill.transform.localScale.z);

        _potentialStaminaGainBarFill.transform.localScale = new Vector3(_potentialStaminaGainBarValue,
            _potentialStaminaGainBarFill.transform.localScale.y, 
            _potentialStaminaGainBarFill.transform.localScale.z );
        _potentialStaminaGainBarFill.transform.position = _staminaBarFillEndPoint.transform.position;
    }

    public void SetPotentialStaminaGainBar()
    {
        
    }

    public void SetCharacter(PlayerMain character)
    {
        _myCharacter = character;
    }
}
