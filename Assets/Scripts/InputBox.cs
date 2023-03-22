using UnityEngine;
using UnityEngine.UI;

public class InputBox : MonoBehaviour
{
    public int boxValue;
    private Button _button;

    private void Awake()
    {
        this._button = GetComponent<Button>();
        this._button.onClick.AddListener(BtnClick);
    }

    private void BtnClick()
    {
        GameManager.Instance.PlayerPlay(this.boxValue);
    }
}
