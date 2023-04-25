using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField _gridX;
    [SerializeField] TMP_InputField _gridY;

    private int _inputGridX;
    private int _inputGridY;

    public void StartGame()
    {
        if(int.TryParse(_gridX.text, out int result))
        {
            GridManager.Instance.GridWidth = result;
        }
        if (int.TryParse(_gridY.text, out int result2))
        {
            GridManager.Instance.GridHeight = result2;
        }
       
        
        GameManager.Instance.StartTheGame();
        
    }

}
