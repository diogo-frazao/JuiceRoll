using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private Image playerTapBar;

    [SerializeField]
    private GameObject startLevelPanel;

    private EndLevelSlowDown endLevelSlowDown;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        endLevelSlowDown = FindObjectOfType<EndLevelSlowDown>();
        Time.timeScale = 0f;
    }

    private void Update()
    {
        // On first input, deactivate start level panel
        if (Input.GetMouseButtonDown(0))
        {
            startLevelPanel.SetActive(false);
            Time.timeScale = 1f;
        }

        if (playerTapBar.isActiveAndEnabled)
        {
            float playerTapBarFillAmount = endLevelSlowDown.CurrentNumberTaps / EndLevelSlowDown.MaxNumberTaps;
            playerTapBar.fillAmount = playerTapBarFillAmount;
            playerTapBar.color = Color.Lerp(Color.red, Color.green, playerTapBarFillAmount);
        }
    }

    public void ActivatePlayerTapBar(bool value)
    {
        playerTapBar.gameObject.SetActive(value);
    }
}
