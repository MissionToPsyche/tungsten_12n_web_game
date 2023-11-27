using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDatabase;
    public TextMeshProUGUI nameText; 
    public SpriteRenderer artworkSprite; 
    private int selection = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption")) 
        {
            selection = 0;
        }
        else 
        {
            LoadSelection(); 
        }

        UpdateCharacter(selection);
    }

    public void NextOption() 
    {
        selection++; 

        if (selection >= characterDatabase.CharacterCount) 
        {
            selection = 0; 
        }

        UpdateCharacter(selection);
        SaveSelection();
    }

    public void BackOption() 
    {
        selection--; 

        if (selection < 0) 
        {
            selection = characterDatabase.CharacterCount - 1; 
        }

        UpdateCharacter(selection);
        SaveSelection();
    }

    private void UpdateCharacter(int selection) 
    {
        Character character = characterDatabase.GetSelectedCharacter(selection);
        artworkSprite.sprite = character.characterSprite; 
        nameText.text = character.characterName;
    }

    private void LoadSelection()
    {
        selection = PlayerPrefs.GetInt("selectedOption");
    }

    private void SaveSelection() 
    {
        PlayerPrefs.SetInt("selectedOption", selection);
    }

    public void ChangeScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}