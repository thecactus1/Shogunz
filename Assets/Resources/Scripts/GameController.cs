using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerstat
{
    public int playerid;
    public int wins;
    public int controllernum;
    public bool ready;
    public string character;
    public bool alive;
}

public class mode
{
    public int playermax;
    public int playermin;
}

public class level
{
    public string name;
    public string desc;
}

public class GameController : MonoBehaviour {
    public float wintimer;
    public static int characteramount;
	public static int minutes;
	public static int seconds;
	public static int totalminutes;
	public static int totalseconds;
	public static int death;
    public static string leveltitle;
    public static Dictionary<int, level> levellist;
    public static Dictionary<int, playerstat> playerids;
    public static Dictionary<int, bool> controllerused;
    public static Dictionary<string, mode> GameMode;
    public static string gamemode;
    public playerstat a, b, c, d;
    public mode challenge, duel;
    public HUD hud;

    public enum HUD
    {
        Solo,
        Duel
    }

	// Use this for initialization
	void Awake () {
        
        GameObject[] controllers = GameObject.FindGameObjectsWithTag("GameController");
        if (controllers.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    level Createlevel(string title, string description)
    {
        level lv = new level();
        lv.name = title;
        lv.desc = description;
        return lv;
    }

    void SetUpLevels()
    {
        levellist = new Dictionary<int, level>();

        levellist[0] = Createlevel("Arena1", "Temple Arena\n\nVertical Combat! Keep your distance from the floor!\nBy Conner");
    }

    void Start()
    {
        wintimer = 5f;
        gamemode = "Challenge";
        SetUpModes();
        SetUpLevels();
        characteramount = 2;
        playerids = new Dictionary<int, playerstat>();
        controllerused = new Dictionary<int, bool>();
        SetUpPlayerID();
        if (totalminutes == null)
            totalminutes = 0;
        if (totalseconds == null)
            totalseconds = 0;
        minutes = 0;
        seconds = 0;
        if (death == null)
            death = 0;
    }

    public static int Players()
    {
        int p = 0;
        for(int i = 1; i < 4; ++i)
        {
            if (playerids[i].ready)
                ++p;
        }
        return p;
    }
    public static int Alive()
    {
        int p = 0;
        for (int i = 1; i < 4; ++i)
        {
            if (playerids[i].alive)
                ++p;
        }
        return p;
    }

    public static int Maxwins()
    {
        int maxwins = 0;
        for (int i = 1; i < 4; ++i)
        {
            if (playerids[i].wins > maxwins)
                maxwins = playerids[i].wins;
        }
        return maxwins;
    }

    public static int Chicken()
    {
        int player = 0;
        int maxwins = 0;
        for (int i = 1; i < 4; ++i)
        {
            if (playerids[i].wins > maxwins)
            {
                maxwins = playerids[i].wins;
                player = playerids[i].playerid;
            }
        }
        return player;
    }

    public static playerstat Lastone()
    {
        for (int i = 1; i < 4; ++i)
        {
            if (playerids[i].alive)
                return playerids[i];
        }
        return null;
    }


    mode Createmode(mode i, int minplayer, int maxplayer)
    {

        i = new mode();
        i.playermin = minplayer;
        i.playermax = maxplayer;
        return i;
    }

    playerstat Createstat(playerstat i, int id)
    {
        
        i = new playerstat();
        i.character = "Akira";
        i.controllernum = -1;
        i.playerid = id;
        i.ready = false;
        i.wins = 0;
        i.alive = false;
        return i;
    }

    void SetUpModes()
    {
        GameMode = new Dictionary<string, mode>();
        GameMode["Challenge"] = Createmode(challenge, 1, 1);
        GameMode["Duel"] = Createmode(duel, 2, 2);
    }

    public void SetUpPlayerID()
    {
        
        playerids[1] = Createstat(a, 1);
        playerids[2] = Createstat(b, 2);
        playerids[3] = Createstat(c, 3);
        playerids[4] = Createstat(d, 4);
        for (int i = 0; i < 4; ++i)
        {
            controllerused[i] = false;
        }

    }
	
	// Update is called once per frame
	void Update () {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (SceneManager.GetActiveScene().name == "Menu")
            Restart();
        if (SceneManager.GetActiveScene().name == "End")
            End();
        if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
		if (seconds > 59) {
			++minutes;
			seconds = 0;
		}
        if (totalseconds > 59)
        {
            totalseconds = 0;
            ++ totalminutes;
        }
        if(gamemode == "Challenge")
		    Text ();
        if (gamemode == "Duel")
            Win();
        else
        {
            TextMesh text = GameObject.Find("Win").GetComponent<TextMesh>();
            text.text = "";
        }
	}
    void End()
    {
        TextMesh text = GameObject.FindGameObjectWithTag("Finish").GetComponent<TextMesh>();
        if (text)
        {
            string line1 = "Thank you for playing!";
            string line2 = "Total time: " + totalminutes + ":" + totalseconds;
            if (totalseconds < 10)
            {
                line2 = "Total Time: " + totalminutes + ":0" + totalseconds;
            }
            string line3 = "Total Deaths: " + death;
            string line4 = "Press X to return to the menu";
            text.text = line1 + "\n" + line2 + "\n" + line3 + "\n\n" + line4;
        }
    }

    void Restart()
    {
        totalminutes = 0;
        totalseconds = 0;
        minutes = 0;
        seconds = 0;
        death = 0;
    }

	void Text(){
		TextMesh text = GameObject.FindGameObjectWithTag ("HUD").GetComponent<TextMesh>();
        if (text == null)
            return;
		string line1 = "";
		string line3 = "";
		line1 = "Time: " + minutes + ":" + seconds;
		if (seconds < 10)
			line1 = "Time: " + minutes + ":0" + seconds;
		line3 = "Total Time: " + totalminutes + ":" + totalseconds;
		if (totalseconds < 10)
			line3 = "Total Time: " + totalminutes + ":0" + totalseconds;
		string line2 = "Deaths: " + death;
		text.text = leveltitle + "\n"+line3 + "\n" + line1 + "\n" +line2;
	}

    void Win()
    {
        int livingplayers = Alive();
        TextMesh text = GameObject.Find("Win").GetComponent<TextMesh>();
        if (text)
        {
            text.text = "";
            Debug.Log(livingplayers);
            if (text == null || livingplayers - 1 > 1)
                return;
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            for(int i = 0; i<bullets.Length; ++i)
            {
                Destroy(bullets[i]);
            }
            int player = Lastone().playerid;
            int p1win = playerids[1].wins + 1;
            int p2win = playerids[2].wins + 1;
            int p3win = playerids[3].wins + 1;
            int p4win = playerids[4].wins + 1;
            string line1 = "Player " + player + " Wins!";
            string line2 = "Player 1: " + playerids[1].wins;
            if (player == 1)
                line2 = "Player 1: " + p1win;
            string line3 = "Player 2: " + playerids[2].wins;
            if (player == 2)
                line3 = "Player 2: " + p2win;
            string line4 = "Player 3: " + playerids[3].wins;
            if (player == 3)
                line4 = "Player 3: " + p3win;
            if (!playerids[3].ready)
                line4 = "";
            string line5 = "Player 4: " + playerids[4].wins;
            if (player == 2)
                line5 = "Player 4: " + p4win;
            if (!playerids[4].ready)
                line5 = "";
            string line6 = "";
            if (Maxwins() == 4)
            {
                line6 = "Player " + Chicken() + " takes the cake!";
            }
            text.text = line1 + "\n" + line2 + "\n" + line3 + "\n" + line4 + "\n" + line5 + "\n\n" + line6;
            wintimer -= Time.deltaTime;
            if (wintimer < 0)
            {
                ++playerids[Lastone().playerid].wins;
                int wins = Maxwins();
                if (wins != 5)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                    SceneManager.LoadScene("InputGetTest");
                    SetUpPlayerID();
                }
                wintimer = 5;
            }
        }
        
    }

    public void Spawn(){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemies.Length; ++i) {
			Destroy(enemies[i]);
		}
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; ++i)
        {
            Destroy(players[i]);
        }
        GameObject[] list = GameObject.FindGameObjectsWithTag ("Spawner");
		for (int i = 0; i < list.Length; ++i) {
			spawn spawner = list [i].GetComponent<spawn> ();
			spawner.Spawn ();
		}
	}
}
