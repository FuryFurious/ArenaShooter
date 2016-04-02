using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Slider LifeBar;

    public Image LifeBarBackground;
    public Image LifeBarFill;

    public Image HudBackground;
    public Image ScoreBackground;

    public Text ScoreText;
    public Image ScoreIcon;


    void Start()
    {
        SetScoreText(0);
    }

	public void SetColorScheme(Color color)
    {
        LifeBarFill.color = color;

        float colorVal = 0.6f;
        Color backgroundColor = color * new Color(colorVal, colorVal, colorVal, 1.0f);
        LifeBarBackground.color = backgroundColor;
        HudBackground.color = backgroundColor;
        ScoreBackground.color = backgroundColor;
        ScoreText.color = Color.white;
        ScoreIcon.color = Color.white;
    }

    public void SetScoreText(int score)
    {
        if (ScoreText)
            ScoreText.text = score.ToString();
    }
}
