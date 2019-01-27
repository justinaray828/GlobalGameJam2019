using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class SpeechBubbleSettings
{
    public string Text;

    public int TimeUntilClose = 5;

    public float TimeBetweenChars = 0.03F;

    public int WriteTextTime = 0;

    public int MaxWidth = 200;
}

public class SpeechBubble : MonoBehaviour
{
    private class MarkupLetter
    {
        public char letter;
        public int alphaIndex;
    }

    public LayoutElement BlackBox;
    public Text UITextComponent;
    public int FramesToOpen;
    public HorizontalLayoutGroup HorizontalGroup;
    private SpeechBubbleSettings[] _Bubbles = {};
    private float _TimeBetweenChars;
    private float _Timer;
    private List<MarkupLetter> _MarkupText = new List<MarkupLetter>();
    private string[] _MarkupColors = {"FFFFFF33", "FFFFFF66", "FFFFFF99", "FFFFFFCC", "FFFFFFFF"};
    private float _MarkupTimer;
    private string _FullText;
    private int _TrueWidth;
    private float _TrueHeight;
    private int _DefaultFramesToOpen = 10;
    private int _TimeToClose;
    private float _TimeOpened;

    public static SpeechBubble Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        int frames = _DefaultFramesToOpen;
        if (FramesToOpen != 0)
        {
            frames = FramesToOpen;
        }

        if (_TimeOpened >= _TimeToClose)
        {
            UITextComponent.text = "";

            if (_Bubbles.Length > 1)
            {
                ResetValues();
                SpeechBubbleSettings[] newBubbles = new SpeechBubbleSettings[_Bubbles.Length - 1];
                for (int index = 1; index < _Bubbles.Length; index++)
                {
                    newBubbles[index - 1] = _Bubbles[index];
                }
                DisplaySpeech(newBubbles);
            }

            if (BlackBox.minHeight > 0 || BlackBox.minWidth > 0)
            {
                BlackBox.minHeight -= _TrueHeight / frames;
                BlackBox.minWidth -= _TrueWidth / frames; 
            } else
            {
                BlackBox.minHeight = 0;
                BlackBox.minWidth = 0;
                ResetValues();
            }
            return;
        }

        if (BlackBox.minHeight < _TrueHeight || BlackBox.minWidth < _TrueWidth)
        {
            BlackBox.minHeight += _TrueHeight / frames;
            BlackBox.minWidth += _TrueWidth / frames;
            return;
        }
        else
        {
            BlackBox.minHeight = _TrueHeight;
            BlackBox.minWidth = _TrueWidth;
        }

        if (_FullText == "")
        {
            return;
        }

        if (_MarkupText.Count > 0 &&
            _MarkupText[_MarkupText.Count - 1].alphaIndex == _MarkupColors.Length - 1 && 
            _MarkupText.Count == _FullText.Length)
        {
            _TimeOpened += Time.deltaTime;
            return;
        }

        _Timer += Time.deltaTime;
        _MarkupTimer += Time.deltaTime;

        if (_MarkupTimer >= _TimeBetweenChars / _MarkupColors.Length)
        {
            // Add new character
            if (_Timer >= _TimeBetweenChars && _MarkupText.Count != _FullText.Length)
            {
                MarkupLetter nextLetter = new MarkupLetter();
                nextLetter.letter = _FullText[_MarkupText.Count];
                nextLetter.alphaIndex = 0;
                _MarkupText.Add(nextLetter);
                _Timer = 0;
            }
            UITextComponent.text = UpdateMarkupTextAlphas();
        }

        if (_MarkupTimer >= _TimeBetweenChars)
        {
            _MarkupTimer = 0;
        }
    }

    private string UpdateMarkupTextAlphas()
    {
        string fullString = "";
        foreach(MarkupLetter letter in _MarkupText)
        {
            if (letter.alphaIndex < _MarkupColors.Length - 1) {
                letter.alphaIndex++;
            }
            fullString += "<color=#" + _MarkupColors[letter.alphaIndex] + ">" + letter.letter + "</color>";
        }

        return fullString;
    }

    public void DisplaySpeech(SpeechBubbleSettings[] speechBubbleSettings)
    {
        _Bubbles = speechBubbleSettings;
        UITextComponent.font.RequestCharactersInTexture(speechBubbleSettings[0].Text, UITextComponent.fontSize, UITextComponent.fontStyle);
        ResetValues();
        HorizontalGroup.padding = new RectOffset(20, 20, 20, 20);
        SetTimeBetweenChars(speechBubbleSettings[0]);
        string modifiedString = GetModifiedString(speechBubbleSettings[0].Text, speechBubbleSettings[0].MaxWidth);
        _TimeToClose = speechBubbleSettings[0].TimeUntilClose;
        _TrueHeight = UITextComponent.fontSize + GetNumberLines(modifiedString) * (UITextComponent.fontSize + UITextComponent.lineSpacing);
        _TrueWidth = speechBubbleSettings[0].MaxWidth;
        _FullText = modifiedString;
    }

    private void SetTimeBetweenChars(SpeechBubbleSettings speechBubbleSettings)
    {
        if (speechBubbleSettings.WriteTextTime == 0)
        {
            _TimeBetweenChars = speechBubbleSettings.TimeBetweenChars;
        } else
        {
            _TimeBetweenChars = speechBubbleSettings.WriteTextTime / speechBubbleSettings.Text.Length;
        }
    }

    private string GetModifiedString(string speech, int maxWidth) 
    {
        string[] words = speech.Split(' ');

        int currentLineWidth = 0;
        string fullSpeech = "";

        int spaceWidth = GetCharWidth(' ');

        foreach(string word in words)
        {
            int width = GetWordWidth(word) + spaceWidth;
            currentLineWidth += width;
            if (currentLineWidth > maxWidth) {
                fullSpeech += '\n';
                currentLineWidth = width;
            }
            fullSpeech += word + " ";
        }

        return fullSpeech;
    }

    private int GetWordWidth(string word)
    {
        int wordWidth = 0;
        foreach(char letter in word)
        {
            wordWidth += GetCharWidth(letter);
        }

        return wordWidth;
    }

    private int GetCharWidth(char letter)
    {
        CharacterInfo characterInfo;
        UITextComponent.font.GetCharacterInfo(letter, out characterInfo, UITextComponent.fontSize, UITextComponent.fontStyle);
        return characterInfo.advance;
    }

    private int GetNumberLines(string speech)
    {
        string[] lines = speech.Split('\n');
        return lines.Length;
    }

    private void ResetValues()
    {
        _Timer = 0;
        _TimeOpened = 0;
        UITextComponent.text = "";
        _FullText = "";
        _MarkupText = new List<MarkupLetter>();
        HorizontalGroup.padding = new RectOffset(0, 0, 0, 0);
        _TrueHeight = 0;
        _TrueWidth = 0;
    }
}
