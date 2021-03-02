using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{

    public GameObject Player;
    public GameObject MainCamera;
    public GameObject UI;
    // road var
    private GameObject Road;
    public Material RoadSpeed;
    private bool keepPlaceRoad;
    public Vector3 startRoadPosition;
    private float roadScale;
    public int vision;
    public int endVision;
    private GameObject timerRoad;
    // obst var
    private Dictionary<string, GameObject> ObsList;
    private Dictionary<string, List<GameObject>> busyObs;
    private Dictionary<string, List<GameObject>> availableObs;
    private int movementLength;
    public ParticleSystem playerDust;
    public int damage;
    public int level;
    private bool levelup;
    // general
    public float WorldSpeed;
    private float timer = 0;
    public int second = 0;
    private int secondTemp = 0;
    public int FuelCount;
    public int maxFuel;
    public UI_Script healthbar;
    public int Score = 0;
    public Image ScoreTrophyPlus;
    public bool easyMode = false;
    public Text scorebar;
    private List<int> ScoreArray = new List<int>();
    public Animator animPlayer;
    public Animator animPlayerDeath;
    public ParticleSystem SparksDeath;
    public string ModeGame;
    public string CameraPos;
    public Button restart;
    // background
    private Dictionary<string, GameObject> BackList;
    private Dictionary<string, List<GameObject>> busyBack;
    private Dictionary<string, List<GameObject>> availableBack;
    private bool left;

    // Start is called before the first frame update
    void Start()
    {
        loadScore();
        ScoreArray.Sort();
        CameraPos = PlayerPrefs.GetString("CameraPos");
        ModeGame = PlayerPrefs.GetString("ModeGame");
        if (CameraPos.Equals("0"))
        {
            MainCamera.transform.position = new Vector3(0, 5.17f, -7.87f);
            MainCamera.transform.eulerAngles = new Vector3(14.181f, 0, 0);
            if (ModeGame.Equals("0"))
            {
                damage = 5;
                level = -1;
                WorldSpeed = 25;
                easyMode = true;
            }
            else if (ModeGame.Equals("1"))
            {
                damage = 15;
                level = 0;
                WorldSpeed = 29;
            }
            else if (ModeGame.Equals("2"))
            {
                damage = 20;
                level = 1;
                WorldSpeed = 30;
            }
        }
        else if (CameraPos.Equals("1"))
        {
            MainCamera.transform.position = new Vector3(0, 20.27f, 9.4f);
            MainCamera.transform.eulerAngles = new Vector3(90f, 0, 0);
            if (ModeGame.Equals("0"))
            {
                damage = 10;
                level = -1;
                WorldSpeed = 15;
            }
            else if (ModeGame.Equals("1"))
            {
                damage = 15;
                level = 0;
                WorldSpeed = 17;
            }
            else if (ModeGame.Equals("2"))
            {
                damage = 20;
                level = 1;
                WorldSpeed = 19;
            }
        }
        
        // initialization Obstacles, roads and var
        RoadSpeed.SetVector("Vector2_3E1B73F6", new Vector4(0, WorldSpeed * 0.04f));
        timerRoad = GameObject.Find("Timer");
        Player = GameObject.Find("Player");
        startRoadPosition = new Vector3(0, Player.transform.position.y, Player.transform.position.z - 10);
        movementLength = Player.GetComponent<PlayerScript>().movementLength;
        Road = GameObject.Find("Road");
        roadScale = Road.transform.localScale.z;
        ObsList = new Dictionary<string, GameObject>();
        FuelCount = maxFuel - 1;
        busyObs = new Dictionary<string, List<GameObject>>();
        availableObs = new Dictionary<string, List<GameObject>>();
        BackList = new Dictionary<string, GameObject>();
        busyBack = new Dictionary<string, List<GameObject>>();
        availableBack = new Dictionary<string, List<GameObject>>();
        healthbar.SetMaxFuel(maxFuel);
        restart.gameObject.SetActive(false);

        // insert all the obstable to stock
        foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.tag.Equals("Obstacle"))
            {
                ObsList.Add(obj.name, obj);
            }
            if (obj.tag.Equals("Background"))
            {
                BackList.Add(obj.name, obj);
            }
        }
        // initialization stack of obstacle
        foreach (string name in ObsList.Keys)
        {
            busyObs.Add(name, new List<GameObject>());
            availableObs.Add(name, new List<GameObject>());
        }
        foreach (string name in BackList.Keys)
        {
            busyBack.Add(name, new List<GameObject>());
            availableBack.Add(name, new List<GameObject>());
        }

        // start game
        keepPlaceRoad = true;
        placeObstacle(ObsList, availableObs, busyObs, vision, endVision, -1, 2, 0);
        placeObstacle(BackList, availableBack, busyBack, 150, 180, -25, 3, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (keepPlaceRoad)
        {
            // check how much time pass
            timer += Time.deltaTime;
            secondTemp = (int)(timer % 60);

            if (secondTemp > second)
            {
                second = secondTemp;
                Score++;
                if (maxFuel == FuelCount)
                    Score++;
                scorebar.text = "";
                for (int i = 0; i < 5-(Score + "").Length; i++)
                    scorebar.text += "0";
                scorebar.text += Score;
            }

            if (FuelCount <= 8)
                youDeadSon();
            
            // move the world (all the obstcale)
            MoveObjects(ObsList, busyObs, (int)startRoadPosition.z);
            MoveObjects(BackList, busyBack, 6);

            // move who reset the future obstcale
            timerRoad.transform.position = Vector3.MoveTowards(timerRoad.transform.position, Player.transform.position, Mathf.Min(35, WorldSpeed) * Time.deltaTime);
            if (timerRoad.transform.position.z <= Player.transform.position.z)
            {
                placeObstacle(ObsList, availableObs, busyObs, vision, endVision, -1, 2, 0); // reset the future obstcale
                if (left)
                {
                    placeObstacle(BackList, availableBack, busyBack, 150, 180, 4, 25, 3); // reset the future obstcale
                    left = false;
                }
                else
                {
                    placeObstacle(BackList, availableBack, busyBack, 150, 180, -25, -4, 3); // reset the future obstcale
                    left = true;
                }
                timerRoad.transform.position = new Vector3(0, 0, 50);
                FuelCount -= 2;
                healthbar.SetFuel(FuelCount);
            }
        }
        // level up
        if (levelup && (second == 10 || second == 20 || second == 30 || second == 40))
        {
            levelup = false;
            levelUp();
        }
        else if (!levelup && (second == 19 || second == 29 || second == 39))
            levelup = true;
    }

    private void levelUp()
    {
        level++;
        WorldSpeed += 2;
    }

    // puase a bit the movment in game
    IEnumerator delayBeforeGame(GameObject obst)
    {
        keepPlaceRoad = false; // stop movment
        RoadSpeed.SetVector("Vector2_3E1B73F6", Vector4.zero);
        yield return new WaitForSeconds(0.1f);
        if (obst != null)
        { // if the player Collided in obstcale, then make him Invisible
            MeshRenderer[] arr = obst.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < arr.Length; i++)
                arr[i].enabled = false; // do this to children
        }
        keepPlaceRoad = true; // resume movment
        RoadSpeed.SetVector("Vector2_3E1B73F6", new Vector4(0, WorldSpeed * 0.04f));
    }

    private void MoveObjects(Dictionary<string, GameObject> List, Dictionary<string, List<GameObject>> busy, int z)
    {
        // first check which obstacle i have in stack
        foreach (string name in List.Keys)
        {
            if (busy[name].Count != 0)
            { // second, goes through any list of obstcale in this name (name)
                foreach (GameObject item in busy[name])
                {
                    // move them to start point (somewhere Behind the player)
                    Vector3 destin = new Vector3(item.transform.position.x, item.transform.position.y, z);
                    item.transform.position = Vector3.MoveTowards(item.transform.position, destin, WorldSpeed * Time.deltaTime);
                }
            }
        }
    }

    // place the future obstcale
    private void placeObstacle(Dictionary<string, GameObject> List, Dictionary<string, List<GameObject>> available, Dictionary<string, List<GameObject>> busy, int vision ,int endVision, int startX, int endX, int back)
    {
        foreach (string name in List.Keys) // pass on any obstcale that in stock
        {
            bool dup = Random.Range(0, 4 + level - back) != 0;
            if (name.Equals("Pinyata"))
                dup = Random.Range(0, 4) == 0;
            if (dup) // probability for not have this cycle this obstcale (obstcale with name 'name')
            {
                int count;
                if (name.Equals("Fuel")) // probability for how much from fuel 
                    count = Random.Range(1, 7 - level);
                if (name.Equals("Pinyata"))
                    count = 0;
                else
                    count = Random.Range(1, 1 + level); // probability for how much from this obstcale 
                for (int j = 0; j <= count; j++)
                {
                    if (available[name].Count == 0) // check if we have some free (available) obstcale in stack, if not
                    { // create new obstcale
                        GameObject temp = Instantiate(List[name], randomPosition(name, vision, endVision, startX, endX, List[name].transform.position.y), List[name].transform.rotation);
                        randomRotation(name, temp);
                        if (!name.Equals("Fuel"))
                            randomScale(temp);
                        busy[name].Add(temp); // mark it as busy
                    }
                    else
                    { // there is some free obstcale that we can use
                        GameObject temp = available[name][0]; // take the first obstcale
                        MeshRenderer[] arr = temp.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < arr.Length; i++)
                            arr[i].enabled = true;
                        temp.transform.position = randomPosition(name, vision, endVision, startX, endX, List[name].transform.position.y); // give him random position
                        busy[name].Add(temp); // mark him as busy
                        available[name].Remove(temp); // remove him from the free stack
                    }
                }
            }
        }
    }

    // give random position
    private Vector3 randomPosition(string name, int vision, int endVision, int startX, int endX, float y)
    {// first suggestion
        Vector3 ans = new Vector3(Random.Range(startX, endX) * movementLength, y, Random.Range(vision, endVision));
        List<Vector3> BadPositions = new List<Vector3>(); // collect all the bad suggestion
        foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
        { // check every object on the game
            if (obj.tag == "Obstacle" || obj.tag == "Background") // if you obstcale
            {
                if (Vector3.Distance(obj.transform.position, ans) < 5) // and you neer to our last suggestion
                {
                    BadPositions.Add(ans); // if near, insert to bad suggestion, so we know not to suggestion him again
                    foreach (Vector3 item in BadPositions)
                    { //pass on every bad suggestion we suggest before
                        while (Vector3.Distance(item, ans) < 5) // until we dont find some suggestion that not neer, keep looking
                            ans = new Vector3(Random.Range(startX, endX) * movementLength, y, Random.Range(vision, endVision));
                    } // new suggestion ^ 
                }
            }
        }
        return ans;
    }

    private void randomRotation(string name, GameObject temp)
    {
        Vector3 ans = temp.GetComponentsInChildren<Transform>()[2].transform.eulerAngles;
        if (!name.Equals("Fuel")) {
            float y = Random.Range(-25, 40);
            ans += new Vector3(0, y, 0);
        }
        temp.GetComponentsInChildren<Transform>()[2].transform.eulerAngles = ans;
    }

    private void randomScale(GameObject obs)
    {
        float old = obs.transform.localScale.x;
        float ans = Mathf.Max(1, Random.Range(old-0.2f, old+0.2f));
        obs.transform.localScale = new Vector3(ans, ans, ans);
    }

    // this obstcale finish his road, and now he is a free and happy obstcale
    public void freeObst(GameObject free)
    {
        string name = free.name.Split('(')[0];
        availableObs[name].Add(free);
        busyObs[name].Remove(free);
    }

    public void freeBack(GameObject free)
    {
        string name = free.name.Split('(')[0];
        availableBack[name].Add(free);
        busyBack[name].Remove(free);
    }

    // if the player crash this obstcale
    public void freeObstcle(GameObject obst)
    {
        string name = obst.name.Split('(')[0];
        if (name.Equals("Pinyata"))
        {
            MeshRenderer[] arr = obst.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < arr.Length; i++)
                arr[i].enabled = false;
            Score += 10;
            scorebar.text = "";
            for (int i = 0; i < 5-(Score + "").Length; i++)
                scorebar.text += "0";
            scorebar.text += Score;
        }
        else if (name.Equals("Fuel")) // if fuel, add fuel
        {
            if (maxFuel > FuelCount)
            {
                FuelCount += 2;
                healthbar.SetFuel(FuelCount);
            } 
            MeshRenderer[] arr = obst.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < arr.Length; i++)
                arr[i].enabled = false;
        }
        else
        { // if not fuel, pause for a second 
            FuelCount -= damage;
            healthbar.SetFuel(FuelCount);

            if (FuelCount <= 8) 
                youDeadSon();
            else
            {
                animPlayer.Play("Player_Hit");
                StartCoroutine(delayBeforeGame(obst));
            }
        }
    }

    IEnumerator plusTen()
    {
        ScoreTrophyPlus.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ScoreTrophyPlus.gameObject.SetActive(false);
    }

    private void youDeadSon()
    {
        UI.GetComponent<UI_Script>().Restart();
        Player.GetComponent<PlayerScript>().isDead = true;
        keepPlaceRoad = false;
        RoadSpeed.SetVector("Vector2_3E1B73F6", Vector4.zero);
        // animation dead
        playerDust.Stop();
        animPlayerDeath.Play("PlayerDeathAnim");
        animPlayer.enabled = false;
        SparksDeath.Play();
        // back to start site
        if (!easyMode)
        {
            updateScore(Score);
            saveScore();
            PlayerPrefs.Save();
            loadScore();
        }
    }

    private void loadScore()
    {
        string name = "Score";
        for (int i = 0; i < 3; i++)
            ScoreArray.Add(PlayerPrefs.GetInt(name + i, 0));
    }

    private void saveScore()
    {
        string name = "Score";
        for (int i = 0; i < 3; i++)
            PlayerPrefs.SetInt(name + i, ScoreArray[i]);
    }

    private void updateScore(int score)
    {
        bool change = false;
        for (int i = 0; i < 3; i++)
        {
            if (score > ScoreArray[i])
            {
                change = true;
                i = 4;
            }
        }
        if (change)
        {
            ScoreArray.Remove(ScoreArray[0]);
            ScoreArray.Add(score);
        }
    }

}
