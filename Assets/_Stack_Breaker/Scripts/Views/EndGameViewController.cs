using CBGames;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndGameViewController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI levelResultTxt = null;
    [SerializeField] private TextMeshProUGUI nextLevelTxt = null;
    [SerializeField] private GameObject NextButton = null;
    [SerializeField] private GameObject ReplyButton = null;
    //[SerializeField] private RectTransform homeButtonViewTrans = null;
    //[SerializeField] private RectTransform nativeShareButtonViewTrans = null;

    public void OnShow()
    {
        if (IngameManager.Instance.IngameState == IngameState.Ingame_CompletedLevel)
        {
            levelResultTxt.text = "LEVEL COMPLETED !"; 

            ReplyButton.SetActive(false);
            NextButton.SetActive(true); 

        }
        else if (IngameManager.Instance.IngameState == IngameState.Ingame_GameOver)
        {
            levelResultTxt.text = "LEVEL FAILED !";
            levelResultTxt.color = Color.red;

            ReplyButton.SetActive(true);
            NextButton.SetActive(false);
        }

        RectTransform levelResultTxtTrans = levelResultTxt.rectTransform;
        ViewManager.Instance.MoveRect(levelResultTxtTrans, levelResultTxtTrans.anchoredPosition, new Vector2(0, levelResultTxtTrans.anchoredPosition.y), 0.5f);
        // RectTransform nextLevelTxtTrans = nextLevelTxt.rectTransform;
        // ViewManager.Instance.MoveRect(homeButtonViewTrans, homeButtonViewTrans.anchoredPosition, new Vector2(30, homeButtonViewTrans.anchoredPosition.y), 0.5f);
        // ViewManager.Instance.MoveRect(nativeShareButtonViewTrans, nativeShareButtonViewTrans.anchoredPosition, new Vector2(-30, nativeShareButtonViewTrans.anchoredPosition.y), 0.5f);
        nextLevelTxt.text = "NEXT LEVEL: " + PlayerPrefs.GetInt(PlayerPrefsKey.SAVED_LEVEL_PPK).ToString();
    }


    private void OnDisable()
    {
        levelResultTxt.rectTransform.anchoredPosition = new Vector2(-700, levelResultTxt.rectTransform.anchoredPosition.y);
        //nextLevelTxt.rectTransform.anchoredPosition = new Vector2(700, nextLevelTxt.rectTransform.anchoredPosition.y);
       
        // homeButtonViewTrans.anchoredPosition = new Vector2(-150, homeButtonViewTrans.anchoredPosition.y);
        // nativeShareButtonViewTrans.anchoredPosition = new Vector2(150, nativeShareButtonViewTrans.anchoredPosition.y);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("Ingame", 0.3f);
        }
    }

    public void HomeBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ViewManager.Instance.LoadScene("Home", 0.3f);
    }

    public void NativeShareBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ServicesManager.Instance.ShareManager.NativeShare();
    }
}
