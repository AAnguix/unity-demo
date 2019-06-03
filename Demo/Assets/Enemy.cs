using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Renderer rend;

    public Color Color { get { return rend.material.color; } }

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetColor(Color color)
    {
        rend.material.color = color; 
    }
}
