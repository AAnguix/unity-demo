using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Important! Class name must match the file name.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float AccelerationFactor;
    public Color Color;
    public AudioClip EnemyKilledClip;
    public AudioClip LooserClip;

    private Rigidbody rigidBody;
    private Renderer rend;
    private AudioSource audioSource;

    /// <summary>
    /// Awake is executed before Start. It is a good place to get the references to the components.
    /// </summary>
    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
        this.rigidBody = GetComponent<Rigidbody>();
        this.rend = GetComponent<Renderer>();

        if (this.rend != null)
        {
            Color = this.rend.material.color;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.GameState == GameState.Playing)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Vector3 acceleration = new Vector3(Input.acceleration.x, Input.acceleration.y, 0.0f);
                acceleration = Quaternion.Euler(90, 0, 0) * acceleration;

                this.rigidBody.AddForce(acceleration * AccelerationFactor);
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                Vector3 acceleration = Vector3.zero;
                float value = 15.0f;

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    acceleration = new Vector3(-value, 0.0f, 0.0f);
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    acceleration = new Vector3(0.0f, 0.0f, value);
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    acceleration = new Vector3(value, 0.0f, 0.0f);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    acceleration = new Vector3(0.0f, 0.0f, -value);
                }

                this.rigidBody.AddForce(acceleration * AccelerationFactor);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        var gameObject = collision.gameObject;
        var enemy = gameObject.GetComponent<Enemy>();

        //Ineficient way to do this. GetComponent is an expensive operation.
        if (enemy != null)
        {
            if (!enemy.Color.Equals(this.Color))
            {
                GameManager.instance.DestroyEnemy(enemy);
 
                if (GameManager.instance.GameFinished())
                {
                    audioSource.PlayOneShot(LooserClip, 1.5f);
                    GameManager.instance.GameState = GameState.Finished;
                }
                else
                {
                    audioSource.PlayOneShot(EnemyKilledClip);
                }
            }
        }
    }
}