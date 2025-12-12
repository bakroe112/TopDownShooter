using UnityEngine;
using UnityEngine.UI;

public class DoSomethingUIChange : MonoBehaviour
{

    public Sprite Target;
    public Sprite Interact;
    public Sprite Fire;
    public Sprite Transport;
    public Image thisImage;
    public Transform thisTransform;
    public float speed;
    public bool change;
    private void Start()
    {
        thisImage = GetComponent<Image>();
        thisTransform = GetComponent<Transform>();
    }
    private void Update()
    {
        if (change)
        {
            if (thisTransform.localScale.x >= speed * Time.deltaTime && thisImage.sprite != Target)
            {
                thisTransform.localScale -= new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);
            }
            else if (thisTransform.localScale.x < speed * Time.deltaTime && thisImage.sprite != Target)
            {
                thisTransform.localScale -= new Vector3(0, 0, 0);
                thisImage.sprite = Target;
            }
            else if (thisTransform.localScale.x <= 1 - speed * Time.deltaTime)
            {
                thisTransform.localScale += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);
            }
            else if (thisTransform.localScale.x > 1 - speed * Time.deltaTime)
            {
                thisTransform.localScale = new Vector3(1, 1, 1);
                change = false;
            }
        }
    }
    public void ChangeToInteract()
    {
        change = true;
        Target = Interact;
    }
    public void ChangeToFire()
    {
        change = true;
        Target = Fire;
    }
    public void ChangeToTransport()
    {
        change = true;
        Target = Transport;
    }
}
