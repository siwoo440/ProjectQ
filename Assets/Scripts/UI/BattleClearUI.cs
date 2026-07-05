using TMPro;
using UnityEngine;

public class BattleClearUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject clearTextObject; // 클리어 텍스트 오브젝트를 저장한다.
    public TextMeshProUGUI clearText; // 클리어 텍스트 컴포넌트를 저장한다.

    public void Show(string message) // 클리어 UI를 표시한다.
    {
        if (clearText != null) // 클리어 텍스트가 연결되어 있는지 확인한다.
        {
            clearText.text = message; // 표시할 문구를 설정한다.
        }

        if (clearTextObject != null) // 클리어 텍스트 오브젝트가 연결되어 있는지 확인한다.
        {
            clearTextObject.SetActive(true); // 클리어 텍스트 오브젝트를 켠다.
        }
    }

    public void Hide() // 클리어 UI를 숨긴다.
    {
        if (clearTextObject != null) // 클리어 텍스트 오브젝트가 연결되어 있는지 확인한다.
        {
            clearTextObject.SetActive(false); // 클리어 텍스트 오브젝트를 끈다.
        }
    }
}