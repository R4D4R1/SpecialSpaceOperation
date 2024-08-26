//using System.Collections;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class MenuUI : MonoBehaviour
//{

//    private Button playButton;
//    private Button quitButton;

//    private void Start()
//    {
//        VisualElement root  = GetComponent<UIDocument>().rootVisualElement;

//        playButton = root.Q<Button>("PlayButton");
//        quitButton = root.Q<Button>("QuitButton");

//        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
//        quitButton.RegisterCallback<ClickEvent>(OnQuitBUttonClicked);

//    }

//    private void OnPlayButtonClicked(ClickEvent clickEvent)
//    {
//        StartCoroutine(StartGameCoroutine());
//    }

//    private void OnQuitBUttonClicked(ClickEvent clickEvent)
//    {

//    }

//    IEnumerator StartGameCoroutine()
//    {
//        ShipController.Instance.StartFly(GameManager.instance.GetStartTime());
//        yield return new WaitForSeconds(GameManager.instance.GetStartTime());
//        SpawnerInOnePlace.Instance.StartAsteroids();
//        SpawnerBetweenPlaces.Instance.StartSpecial();
//    }

//}
