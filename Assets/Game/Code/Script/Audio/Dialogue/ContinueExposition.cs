using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.InputSystem; 

public class ContinueExposition : MonoBehaviour
{
    [SerializeField] public VoidEvent expositionOne;
    [SerializeField] public VoidEvent expositionTwo;
    [SerializeField] public VoidEvent expositionThree;
    [SerializeField] public VoidEvent expositionFour;
    [SerializeField] public VoidEvent expositionFive;
    [SerializeField] public VoidEvent expositionSix;

    [SerializeField] private Window[] expositionWindows;

    [SerializeField] private UnityEngine.UI.Image blackFadeImage;
    private int onWhichExposition =0;
    private bool skipping = false;
    [SerializeField] private float fadeDuration = 1f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        expositionWindows[0].windowObject.SetActive(true);
        expositionOne.Invoke();
        StartCoroutine(Skipping());
    }

    // Update is called once per frame
    private void Update()
	{
		if(Mouse.current.leftButton.isPressed && skipping==false)
        {
            if(onWhichExposition==0)
            {
                expositionWindows[onWhichExposition].windowObject.SetActive(false);
                expositionWindows[onWhichExposition+1].windowObject.SetActive(true);
                onWhichExposition++;
                expositionTwo.Invoke();
                StartCoroutine(Skipping());
            }
            else if(onWhichExposition==1)
            {
                expositionWindows[onWhichExposition].windowObject.SetActive(false);
                expositionWindows[onWhichExposition+1].windowObject.SetActive(true);
                onWhichExposition++;
                expositionThree.Invoke();
                StartCoroutine(Skipping());
            }
            else if(onWhichExposition==2)
            {
                expositionWindows[onWhichExposition].windowObject.SetActive(false);
                expositionWindows[onWhichExposition+1].windowObject.SetActive(true);
                onWhichExposition++;
                expositionFour.Invoke();
                StartCoroutine(Skipping());
            }
            else if(onWhichExposition==3)
            {
                expositionWindows[onWhichExposition].windowObject.SetActive(false);
                expositionWindows[onWhichExposition+1].windowObject.SetActive(true);
                onWhichExposition++;
                expositionFive.Invoke();
                StartCoroutine(Skipping());
            }
            else if(onWhichExposition==4)
            {
                expositionWindows[onWhichExposition].windowObject.SetActive(false);
                expositionWindows[onWhichExposition+1].windowObject.SetActive(true);
                onWhichExposition++;
                expositionSix.Invoke();
                StartCoroutine(Skipping());
            }
            else if(onWhichExposition==5)
            {
                //expositionWindows[onWhichExposition+1].windowObject.SetActive(false);
                StartCoroutine(LoadMenu());
                
                //onWhichExposition++;
            }
        }
				
	}
    IEnumerator Skipping()
    {
            Debug.Log("skipped");
            skipping=true;
            yield return new WaitForSeconds(2);
            skipping=false;
    }
    IEnumerator LoadMenu()
    {
        expositionWindows[5].windowObject.SetActive(false);
        skipping=true;
        yield return new WaitForSeconds(1);

        float fraction = 1 / fadeDuration;

		float t = 0.0f;
		while(t < 1.0f) {
			OnFade(t);
			t += Time.deltaTime * fraction;
			yield return null;
		}
		OnFade(1.0f);

        SceneManager.LoadScene("MainMenu");
        skipping=false;
    }
    public void MoveToNext()
    {
        if(onWhichExposition !=5)
        {
        expositionWindows[onWhichExposition].windowObject.SetActive(false);
        expositionWindows[onWhichExposition+1].windowObject.SetActive(true);
        onWhichExposition++;
        }
        else
        {
            StartCoroutine(LoadMenu());
        }
    }
    private void OnFade(float value)
	{
        expositionWindows[6].windowObject.SetActive(true);
		blackFadeImage.color = new Color(blackFadeImage.color.r, blackFadeImage.color.g, blackFadeImage.color.b, Mathf.SmoothStep(0.0f, 1.0f, value));
	}

    [System.Serializable]
	struct Window
	{
		public string windowName; 
		public GameObject windowObject;
	}
}
