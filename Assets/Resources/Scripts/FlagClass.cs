using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagClass : MonoBehaviour
{
    public string levelname;
    public GameObject player;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.N))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameController.leveltitle = levelname;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player) {
        if (GetComponent<BoxCollider2D>().IsTouching(player.GetComponent<Collider2D>()))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    }
}
