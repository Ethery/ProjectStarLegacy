using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Fading : MonoBehaviour
{
    [Tooltip("Courbe de changement de la transparence du canvas")]
    public AnimationCurve curve;

    private Canvas[] childs;
    private Image img;
    private Text progress;
    private string scene;
    private bool fadeOut = false,
				 init = false;
    private AsyncOperation async;


    private void Awake()
	{
        if (!init)
        {
            DontDestroyOnLoad(this);
            childs = transform.GetComponentsInChildren<Canvas>(true);
            foreach (Canvas c in childs)
            {
                if (c.name == "LoadingScreen")
                {
                    c.gameObject.SetActive(false);
                    progress = c.transform.FindChild("Progress").GetComponent<Text>();
                }
                else if (c.name == "BlackScreen")
                {
                    c.gameObject.SetActive(false);
                    img = c.transform.GetComponentInChildren<Image>();
                }
            }
            StartCoroutine(FadeIn());
            init = true;
			Time.timeScale = 1f;
		}
    }

	public void FadeTo(string scene)
	{
		Time.timeScale = 0f;
		this.scene = scene;
		StartCoroutine(FadeOut());
        StartCoroutine(LoadScene());
    }

	public void Update()
	{
        if(fadeOut)
        {
            fadeOut = false;
            async.allowSceneActivation = true;
        }
        if (SceneManager.GetActiveScene().name == scene && async.allowSceneActivation)
        {
			Time.timeScale = 1f;
            StartCoroutine(FadeIn());
            scene = "";
        }
	}

	IEnumerator FadeOut()
	{
		foreach (Canvas c in childs)
		{
			if (c.name == "LoadingScreen")
			{
				c.gameObject.SetActive(false);
			}
			else if (c.name == "BlackScreen")
			{
				c.gameObject.SetActive(true);
			}
		}

		float t = 0;
		float max = curve.keys[curve.length - 1].time;

		while (t < max)
		{
			t += Time.unscaledDeltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(img.color.r, img.color.g, img.color.b, a);
			yield return 0;
		}
		foreach (Canvas c in childs)
		{
			if (c.name == "LoadingScreen")
			{
				c.gameObject.SetActive(true);
			}
			else if (c.name == "BlackScreen")
			{
				c.gameObject.SetActive(true);
			}
		}
		fadeOut = true;
		yield return 0;
	}

	IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;
		while (!async.isDone)
		{
			progress.text = "Progress:" + async.progress + "%";
			yield return progress;
		}
    }

	IEnumerator FadeIn()
	{
		img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        foreach (Canvas c in childs)
        {
            if (c.name == "LoadingScreen")
            {
                c.gameObject.SetActive(false);
            }
            else if (c.name == "BlackScreen")
            {
                c.gameObject.SetActive(true);
            }
        }
        float t = curve.keys[curve.length-1].time;

        while (t > 0f)
		{
			t -= Time.unscaledDeltaTime;
			float a = curve.Evaluate(t);
            img.color = new Color(img.color.r, img.color.g, img.color.b, a);
			yield return 0;
        }

        foreach (Canvas c in childs)
        {
            if (c.name == "LoadingScreen")
            {
                c.gameObject.SetActive(false);
            }
            else if (c.name == "BlackScreen")
            {
                c.gameObject.SetActive(false);
            }
        }
        yield return 0;
    }

}