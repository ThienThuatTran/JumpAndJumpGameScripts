using UnityEngine;
using UnityEngine.UI;

public class UIPausedMenu : MonoBehaviour
{
    [SerializeField] private Button selectedButton;
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        selectedButton.Select();
    }
}
