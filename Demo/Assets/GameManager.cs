using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private const int firstSceneIndex = 0;

    private Randomizer<Color> randomizer;
    public List<Enemy> Enemies;
    public List<Color> EnemyColors;
    public PlayerController Player;
    public Text TimeText;
    private float elapsedTime = 0.0f;
    public GameState GameState = GameState.Playing;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyColors.Add(Player.Color);
        randomizer = new Randomizer<Color>(EnemyColors);

        foreach (var enemy in Enemies)
        {
            var randomColor = randomizer.GetNext();
            enemy.SetColor(randomColor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GameState == GameState.Playing)
        {
            this.elapsedTime += Time.deltaTime;
            TimeText.text = elapsedTime.ToString("n2");
        }
        else if (this.GameState == GameState.Finished)
        {
            TimeText.text = "LOOSER!";

            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(firstSceneIndex, LoadSceneMode.Single);
            }
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public bool GameFinished()
    {    
        return Enemies.Count(e => !e.Color.Equals(Player.Color)) <= 0;
    }
}