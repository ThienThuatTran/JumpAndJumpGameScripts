using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridTesting : MonoBehaviour
{
    private static GridTesting instance;
    private Grid grid;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(4, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CreateWorldText(string textToShow, Vector2 position)
    {
        GameObject textWorld = new GameObject("World_text", typeof(TextMeshProUGUI));
        Transform textTransform = textWorld.transform;
        //textTransform.localPosition = position;
        textTransform.parent = instance.transform;
        
        TextMeshProUGUI textMesh = textWorld.GetComponent<TextMeshProUGUI>();
        textMesh.rectTransform.sizeDelta = Vector2.one;
        textMesh.rectTransform.position = position;
        textMesh.fontSize = 0.3f;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.color = Color.black;
        textMesh.text = textToShow;

    }
}
