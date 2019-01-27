using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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
    private float _TimeBetweenChars;
    private float _DefaultTimeBetweenChars = 0.03F;
    private float _Timer;
    private int _DefaultMaxWidth = 200;
    private List<MarkupLetter> _MarkupText = new List<MarkupLetter>();
    private string[] _MarkupColors = {"FFFFFF33", "FFFFFF66", "FFFFFF99", "FFFFFFCC", "FFFFFFFF"};
    private float _MarkupTimer;
    private string _FullText;
    private int _TrueWidth;
    private float _TrueHeight;
    private int _DefaultFramesToOpen = 10;
    private int _DefaultTimeToClose = 5;
    private int _TimeToClose;
    private float _TimeOpened;

    // Start is called before the first frame update
    void Start()
    {
        // DisplaySpeech("Lorem ipsum dolor sit amet, consectetur adipLorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam purus tellus, placerat in dolor sit amet, iaculis porta mauris. Proin lobortis posuere libero vel facilisis. Etiam molestie enim a sodales bibendum. Pellentesque blandit risus vitae nisl egestas commodo. In lectus sem, iaculis eget urna vel, commodo faucibus erat. Integer efficitur aliquet arcu nec hendrerit.", 400);
        DisplaySpeech("Almost before we knew it we had left the ground...", 0, _DefaultMaxWidth);
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
            HorizontalGroup.padding = new RectOffset(0, 0, 0, 0);
            if (BlackBox.minHeight != 0 || BlackBox.minWidth != 0)
            {
                BlackBox.minHeight -= _TrueHeight / frames;
                BlackBox.minWidth -= _TrueWidth / frames;
            }
            return;
        }

        if (BlackBox.minHeight < _TrueHeight || BlackBox.minWidth < _TrueWidth)
        {
            BlackBox.minHeight += _TrueHeight / frames;
            BlackBox.minWidth += _TrueWidth / frames;
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

    public void DisplaySpeech(string speech, int timeToClose = 0, int maxWidth = 0, float displaySeconds = 0)
    {
        if (maxWidth == 0)
        {
            maxWidth = _DefaultMaxWidth;
        }
        UITextComponent.text = "";
        _Timer = 0;
        SetTimeBetweenChars(speech, displaySeconds);
        string modifiedString = GetModifiedString(speech, maxWidth - UITextComponent.fontSize);
        _TimeToClose = timeToClose == 0 ? _DefaultTimeToClose : timeToClose;
        _TrueHeight = UITextComponent.fontSize * 1.5f + GetNumberLines(modifiedString) * (UITextComponent.fontSize + UITextComponent.lineSpacing * 2);
        _TrueWidth = maxWidth;
        _FullText = modifiedString;
    }

    private void SetTimeBetweenChars(string speech, float displaySeconds)
    {
        if (displaySeconds == 0)
        {
            _TimeBetweenChars = _DefaultTimeBetweenChars;
        } else
        {
            _TimeBetweenChars = displaySeconds / speech.Length;
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
}
