using UnityEngine;
using TMPro;

public class GamePanelManager : MonoBehaviour
{
    [SerializeField] TMP_Text timeTillNextEnemyText;
    [SerializeField] TMP_Text totalTimeText;
    [SerializeField] TMP_Text hexesText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text fpsText;
    
    void Update() => fpsText.text = "FPS: " + 1 / Time.deltaTime;

    public void UpdateHexes(int hexes) => hexesText.text = "Hexes: " + hexes;
    public void UpdateTotalTime(int totalTime) => totalTimeText.text = "Total Time Wasted: " + totalTime + "S";
    public void UpdateTimeTillNextEnemy(int time) => timeTillNextEnemyText.text = "Time left till next spawn: " + time + "S";
    public void UpdateLives(int lives) => livesText.text = "Lives: " + lives;

}
