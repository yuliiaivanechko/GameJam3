using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    [SerializeField]
    private Animator _canvasAnimator;

    private static readonly int AnimateOut = Animator.StringToHash("animateOut");

    public void ChangeScene()
    {
        StartCoroutine(LoadSceneAFterTransition());
    }

    private IEnumerator LoadSceneAFterTransition()
    {
        _canvasAnimator.SetBool(AnimateOut, true);
        yield return new WaitForSeconds(1f);
 
        SceneManager.LoadScene(_sceneName);
    }
}
