using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerGUI : MonoBehaviour
{
    private HealthBar _playerHealthBar;
    private StaminaBar _playerStaminaBar;
    private CombatBar _playerCombatBar;
    public PlayerMain _player;

    void Awake()
    {
        _playerHealthBar = GetComponentInChildren<HealthBar>();
        _playerStaminaBar = GetComponentInChildren<StaminaBar>();
        _playerCombatBar = GetComponentInChildren<CombatBar>();
    }

	// Use this for initialization
	void Start ()
	{
        _playerHealthBar.SetCharacter(_player);
        _playerStaminaBar.SetCharacter(_player);
        _playerCombatBar.SetCharacter(_player);
        
    }
	
	// Update is called once per frame
	void Update ()
	{

	}
}
