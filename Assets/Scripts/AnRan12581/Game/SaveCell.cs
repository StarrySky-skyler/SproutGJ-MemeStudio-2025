using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SaveCell : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text time;
    public TMP_Text load;
    public Image load_slider;

    public void SaveData(string title,string time,float load)
    {
        this.title.text = title;
        this.time.text = time;
        this.load.text = $"½ø¶È:{load.ToString("F0")}%";
        load_slider.fillAmount = load;
    }

}
