using AnRan;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveCell : MonoBehaviour
{
    public Button enter;
    public TMP_Text time;
    public TMP_Text load;
    public Image load_slider;

    public void LoadData(UserData userdata)
    {
        time.text = userdata.time;
        load.text = $"����:{(userdata.process * 10f).ToString("F0")}%";
        load_slider.fillAmount = userdata.process / 10f;
        enter.onClick.AddListener(() =>
        {
            GameManager.Instance.selectSaveData = userdata;
            SceneManager.LoadScene("Select"); //ѡ���ͼ
        });
    }
}
