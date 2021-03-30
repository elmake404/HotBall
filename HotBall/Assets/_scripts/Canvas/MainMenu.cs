using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.parent.GetComponent<CanvasManager>().BeginningGame();
            FacebookManager.Instance.LevelStart(PlayerPrefs.GetInt("Level"));

            CanvasManager.IsStartGeme = true;
            gameObject.SetActive(false);
        }
    }
}
