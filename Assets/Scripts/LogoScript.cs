using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScript : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    [SerializeField]
    private Animator _canvasAnimator;

    private static readonly int AnimateOut = Animator.StringToHash("animateOut");

    private void Start()
    {
        ChangeScene();
    }

    public void ChangeScene()
    {
        Debug.Log("Starting coroutine");
        StartCoroutine(LoadSceneAFterTransition());
    }

    private IEnumerator LoadSceneAFterTransition()
    {
        _canvasAnimator.SetBool(AnimateOut, true);
        yield return new WaitForSeconds(3.2f);

        SceneManager.LoadScene(_sceneName);
    }
}
