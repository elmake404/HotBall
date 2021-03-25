using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    #region StaticComponent
    public static bool IsStartGeme, IsGameFlow,IsWinGame,IsLoseGame;
    #endregion

    [SerializeField]
    private GameObject _menuUI, _inGameUI, _wimIU, _lostUI;
    private CameraMoveControl _cameraMove;
    private BallLife _ballLife;
    [SerializeField]
    private Image _hotBar;
    [SerializeField]
    private float _delayBeforeStart = 1.3f;

    

    private void Awake()
    {
        _hotBar.fillAmount = 1;
        _ballLife = FindObjectOfType<BallLife>();
        _cameraMove = FindObjectOfType<CameraMoveControl>();
        IsWinGame = false;
        IsLoseGame = false;
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
        if (!_inGameUI.activeSelf && IsStartGeme)
        {
            _menuUI.SetActive(false);
            _inGameUI.SetActive(true);
        }
        if (!_wimIU.activeSelf&& IsWinGame)
        {
            _inGameUI.SetActive(false);
            _wimIU.SetActive(true);
        }
        if (!_lostUI.activeSelf && IsLoseGame)
        {
            _inGameUI.SetActive(false);
            _lostUI.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        _hotBar.fillAmount = Mathf.LerpUnclamped(_hotBar.fillAmount,_ballLife.GetTemperature(),0.1f);
    }
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(_delayBeforeStart);
        IsGameFlow = true;
    }
    public void BeginningGame()
    {
        _cameraMove.StartMoveCamera(_delayBeforeStart);
        StartCoroutine(StartGame());
    }

}
