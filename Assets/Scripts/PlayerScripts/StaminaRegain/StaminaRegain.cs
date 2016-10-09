using UnityEngine;
using System.Collections;

public class StaminaRegain : MonoBehaviour
{

    private PlayerCombat _player;

    public GameObject _regainRegionStart;
    public GameObject _regainRegionEnd;

    public GameObject _regainMove;
    public GameObject _timerEnd;

    private float _regainRegionStartX;
    private float _regainRegionEndX;

    void Awake()
    {
        _player = GetComponentInParent<PlayerCombat>();

        _regainRegionStartX = _regainRegionStart.transform.localPosition.x;
        _regainRegionEndX = _regainRegionEnd.transform.localPosition.x;
    }

	// Use this for initialization
	void Start ()
    {
        _regainMove.transform.position = _timerEnd.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
	{

	    var currentX = _regainMove.transform.localPosition.x;
        if (currentX < _regainRegionStartX && currentX > _regainRegionEndX)
	    {
            if (_player._playerMain.InputRightArm || _player._playerMain._inputLeftArm)
                _player.StaminaGainBack();
	    }
        _regainMove.transform.position = _timerEnd.transform.position;
    }

    void OnEnable()
    {
        _regainMove.transform.position = _timerEnd.transform.position;
    }
}
