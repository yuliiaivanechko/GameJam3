using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour, IDataPersistance, IInteractable
{
    private static readonly int AnimateOut = Animator.StringToHash("animateOut");

    private Animator _canvasAnimator;

    [SerializeField]
    private SpriteRenderer _interactSign;

    private void Start()
    {
        _canvasAnimator = FindAnyObjectByType<Canvas>()?.GetComponent<Animator>();
    }

    public void Interact(GameObject player)
    {
        DataPersistanceManager.instance.SaveGame();
        player.GetComponent<Health>().ResetHealth();
        player.GetComponent<FearController>().ResetKilledEnemies();
        StartCoroutine(LoadSceneAFterTransition());
    }

    public void ChangeState(bool canInteract)
    {
        _interactSign.enabled = canInteract;
    }

    public void LoadData(GameData data, string prevScene)
    {
    }

    public void SaveData(ref GameData data)
    {
        data.sceneName = SceneManager.GetActiveScene().name;
    }

    private IEnumerator LoadSceneAFterTransition()
    {
        _canvasAnimator?.SetBool(AnimateOut, true);
        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
