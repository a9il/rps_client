using TMPro;
using UnityEngine;

public class StatInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI winCount;
    [SerializeField]
    private TextMeshProUGUI drawCount;
    [SerializeField]
    private TextMeshProUGUI loseCount;
    public void UpdateData(StatData data)
    {
        winCount.text = data.winCount.ToString();
        loseCount.text = data.loseCount.ToString();
        drawCount.text = data.drawCount.ToString();
    }
}
