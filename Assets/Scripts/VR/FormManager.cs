using TMPro;
using UnityEngine;

public class FormManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;

    [SerializeField] private TMP_InputField ageInputField;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //LoadData();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("Name", nameInputField.text);
        PlayerPrefs.SetInt("Age", int.Parse(ageInputField.text));
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("Name")) nameInputField.text = PlayerPrefs.GetString("Name");
        if (PlayerPrefs.HasKey("Age")) ageInputField.text = PlayerPrefs.GetInt("Age").ToString();
    }
}