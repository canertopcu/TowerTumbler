using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingViewController : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI loadingTxt = null;
    [SerializeField] private Slider loadingSlider = null;

    public void OnShow()
    {
        loadingSlider.value = 0;
        StartCoroutine(CRFillLoadingSlider());
    }

    private IEnumerator CRFillLoadingSlider()
    {
        float fillingTime = 0.5f;
        FindObjectOfType<SceneLoader>().LoadScene(fillingTime + 0.1f);
        float t = 0;
        while (t < fillingTime)
        {
            t += Time.deltaTime;
            float factor = t / fillingTime;
            loadingSlider.value = Mathf.Lerp(0, 0.99f, factor);
            yield return null;
        }
    }


    public void SetLoadingText(string text)
    {
        loadingTxt.text = text;
    }
}
