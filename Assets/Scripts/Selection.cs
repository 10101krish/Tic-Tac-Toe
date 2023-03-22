using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    public int choiceValue;
    private Button _button;

    private void Awake()
    {
        this._button = GetComponent<Button>();
        this._button.onClick.AddListener(BtnClick);
    }

    private void BtnClick()
    {
        // GameManager.Instance.SelectionChoice(choiceValue);
    }
}
