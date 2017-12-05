using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class menuScript : MonoBehaviour {

    [SerializeField]
    Button button;

    void Start()
    {
        Button btn = button.GetComponent<Button>();
    }

    public void ButtonClicked()
    {
        SceneManager.LoadScene("mainScene");
    }
	
}
