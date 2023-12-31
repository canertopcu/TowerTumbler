﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CBGames;
using System.Text.RegularExpressions;
using System.Linq;
using TMPro;

public class LeaderboardViewController : MonoBehaviour
{
    [SerializeField] private GameObject noInternetConectionView = null;
    [SerializeField] private GameObject setUsernameView = null;
    [SerializeField] private TMP_InputField usernameInputField = null;
    [SerializeField] private TextMeshProUGUI errorTxt = null;
    [SerializeField] private GameObject leaderboardView = null;
    [SerializeField] private TextMeshProUGUI localUsernameTxt = null;
    [SerializeField] private TextMeshProUGUI localLevelTxt = null;
    [SerializeField] private RectTransform contentTrans = null;
    [SerializeField] private LeaderboardItemController leaderboardItemControlPrefab = null;


    private List<LeaderboardItemController> listLeaderboardItemControl = new List<LeaderboardItemController>();

    public void OnShow()
    {
        errorTxt.gameObject.SetActive(false);
        ServicesManager.Instance.LeaderboardManager.CheckConnectedInternet((isConnect) =>
        {
            if (isConnect)
            {
                noInternetConectionView.SetActive(false);

                if(!ServicesManager.Instance.LeaderboardManager.IsSetUsername()) //Didn't set username
                {
                    setUsernameView.SetActive(true);
                    leaderboardView.SetActive(false);
                }
                else //Already have username -> show leaderboard
                {
                    localUsernameTxt.text = string.Empty;
                    localLevelTxt.text = string.Empty;

                    setUsernameView.SetActive(false);
                    leaderboardView.SetActive(true);

                    CreateItemsAndSetLocalUser();
                }
            }
            else
            {
                noInternetConectionView.SetActive(true);
            }
        });
    }



    public void ConfirmBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        Regex regex = new Regex(@"^[A-z][A-z|\.|\s]+$");
        if (!regex.IsMatch(usernameInputField.text))
        {
            errorTxt.gameObject.SetActive(true);
            errorTxt.text = "Please Choose A Different Username !";
        }
        else
        {
            string username = usernameInputField.text.Trim();
            usernameInputField.text = username;
            ServicesManager.Instance.LeaderboardManager.CheckUsernameExists(username, (isExists) =>
            {
                if (isExists)
                {
                    errorTxt.gameObject.SetActive(true);
                    errorTxt.text = "The Username Already Exists !";
                }
                else
                {
                    errorTxt.gameObject.SetActive(false);
                    setUsernameView.SetActive(false);
                    leaderboardView.SetActive(true);
                    PlayerPrefs.SetString(PlayerPrefsKey.SAVED_USER_NAME_PPK, usernameInputField.text);
                    ServicesManager.Instance.LeaderboardManager.SetPlayerLeaderboardData();

                    CreateItemsAndSetLocalUser();                   
                }
            });
        }
    }


    private void CreateItemsAndSetLocalUser()
    {
        foreach (LeaderboardItemController o in listLeaderboardItemControl)
        {
            o.gameObject.SetActive(false);
        }
        ServicesManager.Instance.LeaderboardManager.GetPlayerLeaderboardData((data) =>
        {
            int maxItem = data.Count;
            if (ServicesManager.Instance.LeaderboardManager.MaxUser != -1)
            {
                maxItem = (ServicesManager.Instance.LeaderboardManager.MaxUser > data.Count) ? data.Count : ServicesManager.Instance.LeaderboardManager.MaxUser;
            }
            StartCoroutine(CRCreatingLeaderboardItems(data, maxItem));
        });
    }
    private IEnumerator CRCreatingLeaderboardItems(List<PlayerLeaderboardData> data,int maxItem)
    {
        for (int i = 0; i < maxItem; i++)
        {
            //Create items
            LeaderboardItemController itemController = GetLeaderboardItemControl();
            itemController.transform.SetParent(contentTrans);
            itemController.gameObject.SetActive(true);
            itemController.OnSetup(i + 1, data[i]);

            //Set local user
            if (data[i].Name.Equals(PlayerPrefs.GetString(PlayerPrefsKey.SAVED_USER_NAME_PPK)))
            {
                localUsernameTxt.text = (i + 1).ToString() + "." + " " + data[i].Name;
                localLevelTxt.text = "Level: " + data[i].Level.ToString();
            }

            yield return new WaitForSeconds(0.05f);
        }
    }



    public void CloseBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        //ViewManager.Instance.HomeViewController.OnSubViewClose();
        gameObject.SetActive(false);
    }




    private LeaderboardItemController GetLeaderboardItemControl()
    {
        //Find in the list
        LeaderboardItemController item = listLeaderboardItemControl.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (item == null)
        {
            //Didn't find one -> create new one
            item = Instantiate(leaderboardItemControlPrefab, Vector3.zero, Quaternion.identity);
            item.gameObject.SetActive(false);
            listLeaderboardItemControl.Add(item);
        }

        return item;
    }
}
