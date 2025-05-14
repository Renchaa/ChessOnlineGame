using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChessUiManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private NetworkManager networkManager;

    [Header("Buttons")]
    [SerializeField] private UnityEngine.UI.Button blackTeamButton;
    [SerializeField] private UnityEngine.UI.Button whiteTeamButton;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI connectionStatusText;

    [Header("Screen GameObjects")]
    [SerializeField] private GameObject gameoverScreen;
    [SerializeField] private GameObject connectScreen;
    [SerializeField] private GameObject teamSelectionScreen;
    [SerializeField] private GameObject gameModeSelectionScreen;

    [Header("Other UI")]
    [SerializeField] private TMP_Dropdown gameLevelSelection;

    private void Awake()
    {
        gameLevelSelection.AddOptions(Enum.GetNames(typeof(ChessLevel)).ToList());
        OnGameLauched();
    }
    public void OnGameLauched()
    {
        DisableAllScreens();
        gameModeSelectionScreen.SetActive(true);
    }
    public void OnMultiplayerModeSelected()
    {
        connectionStatusText.gameObject.SetActive(true);
        DisableAllScreens();
        connectScreen.SetActive(true);
    }

    public void OnConnect()
    {
        networkManager.SetPlayerLevel((ChessLevel)gameLevelSelection.value);
        networkManager.Connect();
    }
    public void OnSingleplayerModeSelected()
    {
        DisableAllScreens();
    }
    private void DisableAllScreens()
    {
        gameoverScreen.SetActive(false);
        connectScreen.SetActive(false);
        teamSelectionScreen.SetActive(false);
        gameModeSelectionScreen.SetActive(false);
    }

    public void SetConnectionStatus(string status)
    {
        connectionStatusText.text = status;
    }

    public void ShowTeamSelectionScreen()
    {
        DisableAllScreens();
        teamSelectionScreen.SetActive(true);
    }
    public void selectTeam(int team)
    {
        networkManager.SelectTeam(team);
    }

    public void RestrictTeamChoice(TeamColor occupiedTeam)
    {
        var buttonToDeactivate = occupiedTeam == TeamColor.White ? whiteTeamButton : blackTeamButton;
        buttonToDeactivate.interactable = false;
    }

    public void OnGameStarted()
    {
        DisableAllScreens();
        connectionStatusText.gameObject.SetActive(false);
    }

    internal void OnGameFinished(string winner)
    {
        gameoverScreen.SetActive(true);
        resultText.text = string.Format("{0} won,", winner);
    }
}
