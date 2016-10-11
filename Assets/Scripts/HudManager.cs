using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    // assign in inspector
    public Image healthBar;
    public Image moralityBar;
    public Image moralityBarInverse;
    public Image sanityBar;
    public Image sanityBarInverse;
    public float lerpSpeed;

    // color lerp
    public Color healthFullColor;
    public Color moralityFullColor;
    public Color sanityFullColor;
    public Color lowColor;

    PlayerManager playerManagerScript;

	void Start ()
    {
        playerManagerScript = GetComponent<PlayerManager>();
    }
	
	void Update ()
    {
        UpdateHealthBar();
        UpdateMoralityBar();
        UpdateSanityBar();
    }

    private void UpdateHealthBar()
    { 
        // fillAmount lerp
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)playerManagerScript.CurrentHp / (float)playerManagerScript.MaxHp, Time.deltaTime * lerpSpeed);

        // color lerp
        healthBar.color = Color.Lerp(lowColor, healthFullColor, (float)playerManagerScript.CurrentHp / (float)playerManagerScript.MaxHp);
    }

    private void UpdateMoralityBar()
    {
        // fillAmount lerp
        moralityBar.fillAmount = Mathf.Lerp(moralityBar.fillAmount, (float)playerManagerScript.Morality / 100f, Time.deltaTime * lerpSpeed);
        //moralityBarInverse.fillAmount = Mathf.Lerp(moralityBarInverse.fillAmount, 1f - (float)playerManagerScript.Morality / 100f, Time.deltaTime * lerpSpeed);

        // color lerp
        //moralityBar.color = Color.Lerp(lowColor, moralityFullColor, (float)playerManagerScript.Morality / 100f);
        //moralityBarInverse.color = Color.Lerp(lowColor, moralityFullColor, 1f - (float)playerManagerScript.Morality / 100f);
    }

    private void UpdateSanityBar()
    {
        // fillAmount lerp
        sanityBar.fillAmount = Mathf.Lerp(sanityBar.fillAmount, (float)playerManagerScript.Sanity / (float)100f, Time.deltaTime * lerpSpeed);
        //sanityBarInverse.fillAmount = Mathf.Lerp(sanityBarInverse.fillAmount, 1f - (float)playerManagerScript.Sanity / 100f, Time.deltaTime * lerpSpeed);

        // color lerp
        //sanityBar.color = Color.Lerp(lowColor, sanityFullColor, (float)playerManagerScript.Sanity / 100f);
        //sanityBarInverse.color = Color.Lerp(lowColor, sanityFullColor, 1f - (float)playerManagerScript.Sanity / 100f);
    }
}
