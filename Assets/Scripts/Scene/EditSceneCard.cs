using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditSceneCard : MonoBehaviour
{
    private Image _image;
    public Image image
    {
        get
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }
            return _image;
        }
    }

    public Image filter;
    public TextMeshProUGUI selectText;
    public int selectedFlg;

    private void Awake()
    {
        UIupdate();
    }

    public void select()
    {
        if (selectedFlg == 0)
        {
            selectedFlg = 1;
        }
        else
        {
            selectedFlg = 0;
        }

        UIupdate();
    }
    public void UIupdate()
    {
        if (selectedFlg == 0)
        {
            filter.gameObject.SetActive(false);
        }
        else
        {
            filter.gameObject.SetActive(true);
        }
    }
}
