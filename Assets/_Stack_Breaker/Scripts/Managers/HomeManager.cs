using UnityEngine.SceneManagement;
using UnityEngine;
using CBGames;
using UnityEngine.EventSystems;

public class HomeManager : MonoBehaviour {

    [SerializeField] private MeshRenderer mainCharacterMeshRender = null;
    [SerializeField] private Transform bottomPillarTrans = null;

    private float jumpVelocity = 20;
    private float fallingSpeed = -60;
    private float minScale = 0.85f;
    private float maxScale = 1.25f;
    private float scalingFactor = 2;
    private float currentJumpVelocity = 0;
    private void Start()
    { 
        Application.targetFrameRate = 60;
        ViewManager.Instance.OnLoadingSceneDone(SceneManager.GetActiveScene().name);

        //Report to leaderboard
        string username = PlayerPrefs.GetString(PlayerPrefsKey.SAVED_USER_NAME_PPK);
        if (!string.IsNullOrEmpty(username))
        {
            ServicesManager.Instance.LeaderboardManager.SetPlayerLeaderboardData();
        }
    }


    private void Update()
    {
        //Rotate the bottom pillar
        bottomPillarTrans.eulerAngles += Vector3.up * 30f * Time.deltaTime;

        mainCharacterMeshRender.transform.position = mainCharacterMeshRender.transform.position + Vector3.up * (currentJumpVelocity * Time.deltaTime + fallingSpeed * Time.deltaTime * Time.deltaTime / 2);

        if (currentJumpVelocity < fallingSpeed)
            currentJumpVelocity = fallingSpeed;
        else
            currentJumpVelocity = currentJumpVelocity + fallingSpeed * Time.deltaTime;

        if (currentJumpVelocity < 0)
        {
            //Adjust scale
            Vector3 scale = mainCharacterMeshRender.transform.localScale;
            if (scale.x > minScale)
            {
                scale.x -= scalingFactor * Time.deltaTime;
            }
            else
                scale.x = minScale;
            mainCharacterMeshRender.transform.localScale = scale;

            //Calculate the distance
            float bottomY = (mainCharacterMeshRender.transform.position + Vector3.down * (mainCharacterMeshRender.bounds.size.y / 2f)).y;
            float distance = bottomY - bottomPillarTrans.position.y;
            if (distance <= 0.1f)
            {
                currentJumpVelocity = jumpVelocity;
            }
        }
        else
        {
            //Adjust scale
            Vector3 scale = mainCharacterMeshRender.transform.localScale;
            if (scale.x < maxScale)
            {
                scale.x += scalingFactor * Time.deltaTime;
            }
            else
                scale.x = maxScale;
            mainCharacterMeshRender.transform.localScale = scale;

        }
    }
}
