using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Start()
    {
        cuttingCounter.OnCuttingProgressChanged += CuttingCounter_OnCuttingProgressChanged;
        barImage.fillAmount = 0; // makes sure the bar is empty at the start
        Hide();
    }

    private void CuttingCounter_OnCuttingProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
