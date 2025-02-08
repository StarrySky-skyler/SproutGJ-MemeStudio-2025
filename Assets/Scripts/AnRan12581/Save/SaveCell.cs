using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class SaveCell : MonoBehaviour
{
    public Button enter;
    public TMP_Text title;
    public TMP_Text time;
    public TMP_Text load;
    public Image load_slider;

    public void LoadData(UserData userdata)
    {
        title.text = "Level" + userdata.level.ToString();
        this.time.text = userdata.time;
        this.load.text = $"进度:{(userdata.process * 10f).ToString("F0")}%";
        load_slider.fillAmount = userdata.process / 10f;
        enter.onClick.AddListener(() =>
        {
            AnRan.GameManager.Instance.selectSaveData = userdata;
            SceneManager.LoadScene("Select");//选择地图
    
        });
    }

}
