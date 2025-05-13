using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChessUiManager : MonoBehaviour
{
    [SerializeField] private GameObject UiParent;
    [SerializeField] private TextMeshProUGUI resultText;

    public void HideUI()
    {
        UiParent.SetActive(false);
    }

    public void OnGameFinished(string winner)
    {
        UiParent.SetActive(true);
        resultText.text = string.Format("{0} won", winner);
    }
}
