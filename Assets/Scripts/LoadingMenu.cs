using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingMenu : MonoBehaviour
{
    public GameObject[] Other;
    public GameObject[] Circles;
    public GameObject[] Online;
    public void Load(int i)
    {
        PlayerPrefs.SetInt("Mode", i);


        for (int a = 0; a < Other.Length; a++)
        {
            Other[a].SetActive(false);
        }
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine(LoadingOnline(i));
        }
        else StartCoroutine(LoadingOffline(i));
    }
    IEnumerator LoadingOnline(int level)
    {
        PlayerPrefs.SetInt("Internet", 1);
        Online[0].gameObject.SetActive(true);
        for (int a = 0; a < Circles.Length; a++)
        {
            Circles[a].SetActive(true);
            yield return new WaitForSeconds(Random.value);
        }

        SceneManager.LoadScene("SampleScene");
    }
    IEnumerator LoadingOffline(int level)
    {
        PlayerPrefs.SetInt("Internet", 0);

        Online[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("SampleScene");
    }
}
