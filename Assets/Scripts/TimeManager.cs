using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    public float totalTime = 60f;
    float remainingTime;
    Scrollbar scrollbar;

    void Start()
    {
        remainingTime = totalTime;

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform timeScroll = canvas.transform.Find("TimeScrollbar");
            if (timeScroll != null)
            {
                scrollbar = timeScroll.GetComponent<Scrollbar>();
            }
        }
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime < 0)
        {
            remainingTime = 0;
        }
        SyncTimeGUI();
    }

    public void AddTime(float deltaTime)
    {
        remainingTime += deltaTime;
    }

    void SyncTimeGUI()
    {
        if (scrollbar != null)
        {
            scrollbar.size = remainingTime / totalTime;
        }
    }
}