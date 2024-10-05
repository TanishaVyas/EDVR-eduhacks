using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InstructionList : MonoBehaviour
{
    public TextMeshProUGUI instructionText;
    public string[] instructions = { "1. Connect the LDR to the digital multimeter", "2. Switch on the lamp"," 3. place the lamp at 100 cm distance from LDR", "4.Switch off the lights ", "5. Measure and note down the resistance ", "6. Repeat the process for 90, 80, 70, 60, 50, 40, 30, 20 cm.", "7. Plot the r vs 1/d^2 graph" };
    private int currentIndex = 0;

    void Start()
    {
        ShowInstruction();
 
    }

    public void NextInstruction()
    {
        if (instructions.Length > 0)
        {
            currentIndex = (currentIndex + 1)%instructions.Length;
            ShowInstruction();
        }
    }

    public void PreviousInstruction()
    {
        if (instructions.Length > 0)
        {
            currentIndex = (currentIndex - 1 + instructions.Length) % instructions.Length;
            ShowInstruction();
        }
    }

    void ShowInstruction()
    {
        instructionText.text = instructions[currentIndex];

    }
}
