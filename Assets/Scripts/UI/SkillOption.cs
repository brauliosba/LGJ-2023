using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillOption : MonoBehaviour
{
    [SerializeField]
    private Button skillButton;
    [SerializeField]
    private TMP_Text skillName;
    [SerializeField]
    private TMP_Text skillCooldown;

    public Button SkillButton { get { return skillButton; } }
    public TMP_Text SkillName { get { return skillName; } }
    public TMP_Text SkillCooldown { get { return skillCooldown; } }
}
