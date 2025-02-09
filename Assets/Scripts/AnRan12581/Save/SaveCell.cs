using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Tsuki.Entities.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class SaveCell : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Sprite normal;
    public Sprite highlight;
    public Button enter;
    public TMP_Text title;
    public TMP_Text time;
    public TMP_Text load;
    public Image load_slider;
    private Image img;
    private AudioEntity _audio;

    void Start()
    {
        img = GetComponent<Image>();
        _audio = GameObject.FindWithTag("Audio").GetComponent<AudioEntity>();
    }

    public void LoadData(UserData userdata)
    {
        title.text = "Level" + userdata.level.ToString();
        this.time.text = userdata.time;
        this.load.text = $"����:{(userdata.process * 10f).ToString("F0")}%";
        load_slider.fillAmount = userdata.process / 10f;
        enter.onClick.AddListener(() =>
        {
            _audio.PlaySfx("Load a Save");
            AnRan.GameManager.Instance.selectSaveData = userdata;
            SceneManager.LoadScene("Select");//ѡ���ͼ
    
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        img.sprite = highlight;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.sprite = normal;
    }
}
