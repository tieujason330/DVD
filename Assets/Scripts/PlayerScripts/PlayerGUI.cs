using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerGUI : MonoBehaviour
{
    public HealthBar _playerHealthBar;
    public PlayerCharacter _player;

    void Awake()
    {
    }

	// Use this for initialization
	void Start ()
	{
        _playerHealthBar.SetCharacter(_player);
    }
	
	// Update is called once per frame
	void Update ()
	{

	}
}
