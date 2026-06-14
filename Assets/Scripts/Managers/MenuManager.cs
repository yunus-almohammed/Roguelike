using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string levelMapSceneName = "LevelMap";

    [Header("Shop UI")]
    [SerializeField] private GameObject shopPanel;

    private void Start()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(levelMapSceneName);
    }

    public void OnShopButtonClicked()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Shop button clicked. Shop panel is not assigned yet.");
        }
    }

    public void OnCloseShopButtonClicked()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }
}
