using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] TMP_InputField xInput;
    [SerializeField] TMP_InputField yInput;
    public GamePanelManager gamePanel;
    public RestartPanelManager restartPanelManager;

    public void Play()
    {
        GameManager.Instance.GenerateRandomGame(new Vector2Int(
        int.Parse(xInput.text),
        int.Parse(yInput.text)
        ));
        mainMenuPanel.SetActive(false);
        gamePanel.gameObject.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.ClearGrid();
        gamePanel.gameObject.SetActive(false);
        restartPanelManager.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);
    }

    public void LoseScreen(int totalTime, int hexes)
    {
        restartPanelManager.gameObject.SetActive(true);
        restartPanelManager.UpdatePanel(totalTime, hexes);
    }
}