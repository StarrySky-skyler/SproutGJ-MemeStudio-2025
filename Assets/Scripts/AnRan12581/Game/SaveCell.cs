using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Tsuki.Managers;
public class SaveCell : MonoBehaviour
{
    public Button enter;
    public TMP_Text time;
    public TMP_Text load;
    public Image load_slider;

    public void LoadData(UserData userdata)
    {
        this.time.text = userdata.time;
        this.load.text = $"½ø¶È:{(userdata.process * 10f).ToString("F0")}%";
        load_slider.fillAmount = userdata.process / 10f;
        enter.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("ChatLevel" + userdata.level);
        });
    }

}
