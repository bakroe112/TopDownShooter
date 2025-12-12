using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Inventory/New Player")]
public class Player : ScriptableObject
{
    [TextArea]
    public string playerName;
    [TextArea]
    public string playerDescription;
    public Sprite playerImage;
    public int fullHP;
    public int fullShield;
    public int fullMP;
    public float critNum;
    public Animator playerAnimator;
    public AnimationClip playerIdle;
    public AnimationClip playerWalk;
}
