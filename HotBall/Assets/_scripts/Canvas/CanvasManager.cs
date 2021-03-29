using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    #region StaticComponent
    public static bool IsStartGeme, IsGameFlow, IsWinGame, IsLoseGame;
    #endregion

    [SerializeField]
    private GameObject _menuUI, _inGameUI, _wimIU, _lostUI;
    private CameraMoveControl _cameraMove;
    private BallLife _ballLife;
    [SerializeField]
    private Image _hotBar, _progresBar;
    private Transform _finish;
    [SerializeField]
    private Text _levelTextCurrent, _levelTextTarget, _levelTextWin;

    [SerializeField]
    private float _delayBeforeStart = 1.3f;
    private float _distensFinish, _currentDistensFinish;



    private void Awake()
    {
        if (PlayerPrefs.GetInt("Level") <= 0)
        {
            PlayerPrefs.SetInt("Level", 1);
        }
        _levelTextCurrent.text = PlayerPrefs.GetInt("Level").ToString();
        _levelTextTarget.text = (PlayerPrefs.GetInt("Level")+1).ToString();
        _levelTextWin.text = "Level " + PlayerPrefs.GetInt("Level");

        _finish = FindObjectOfType<FinishScripts>().transform;

        _progresBar.fillAmount = 0;
        _hotBar.fillAmount = 1;

        _ballLife = FindObjectOfType<BallLife>();
        _cameraMove = FindObjectOfType<CameraMoveControl>();
        IsWinGame = false;
        IsLoseGame = false;
        _distensFinish = ((Vector2)_ballLife.transform.position - (Vector2)_finish.position).sqrMagnitude;
    }
    private void Start()
    {
        if (!IsStartGeme)
        {
            _menuUI.SetActive(true);
        }
        else
        {
            BeginningGame();
        }
    }

    private void Update()
    {
        if (!_inGameUI.activeSelf && IsStartGeme && IsGameFlow)
        {
            _menuUI.SetActive(false);
            _inGameUI.SetActive(true);
        }
        if (!_wimIU.activeSelf && IsWinGame)
        {
            _inGameUI.SetActive(false);
            _wimIU.SetActive(true);
            PlayerPrefs.SetInt("Scenes", PlayerPrefs.GetInt("Scenes")+ 1);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);

        }
        if (!_lostUI.activeSelf && IsLoseGame)
        {
            _inGameUI.SetActive(false);
            _lostUI.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        _hotBar.fillAmount = Mathf.LerpUnclamped(_hotBar.fillAmount, _ballLife.GetTemperature(), 0.1f);
        _progresBar.fillAmount = Mathf.LerpUnclamped(_progresBar.fillAmount, GetCurentDistens(), 0.1f);
    }
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(_delayBeforeStart);
        IsGameFlow = true;
    }
    private float GetCurentDistens()
    {
        _currentDistensFinish = ((Vector2)_ballLife.transform.position - (Vector2)_finish.position).sqrMagnitude;
        return 1f - _currentDistensFinish / _distensFinish;
    }
    public void BeginningGame()
    {
        _cameraMove.StartMoveCamera(_delayBeforeStart);
        StartCoroutine(StartGame());
    }

}
