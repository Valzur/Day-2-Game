using UnityEngine;
using TMPro;

public class RestartPanelManager : MonoBehaviour
{
    [SerializeField] TMP_Text restartText;

    public void UpdatePanel(int time, int hexes)
    {
        restartText.text = "You've collected a total of " + hexes + " hexes, in a total of " + time + " seconds, Which looks pretty strong to me, aren't you tempted to look even more stronger?";
    }
}
