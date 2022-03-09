using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Delivery : MonoBehaviour
{
    [SerializeField] Color32 hasPackageColor = new Color32 (243,0,210,255);
    [SerializeField] Color32 noPackageColor = new Color32 (255,255,255,255);
    [SerializeField] Color32 hitColor = new Color32 (255,0,0,255);

    [SerializeField] Color32 infoBackgroundColor01 = new Color32(0, 0, 0, 125);
    [SerializeField] Color32 infoBackgroundColor02 = new Color32(0, 0, 0, 0);

    [SerializeField] float destroyDelay = 0.2f;

    SpriteRenderer spriteRenderer;

    public TextMeshProUGUI scoreText;
    public string scoreTextValue;

    public TextMeshProUGUI infoText;
    [SerializeField] Button infoButton;
    Image image;
    public string infoTextValue;

    public TextMeshProUGUI statusText;
    public string statusTextValue;

    public AudioClip otherClip;

    public GameObject enemyCar;

    public TextMeshProUGUI goText;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        StartCoroutine(WelcomeCoroutine());
    }

    float ToFloat(double value)
    {
        double oldValue = value;
        float newValue;

        newValue = (float)oldValue;
        return newValue;
    }

    IEnumerator WelcomeCoroutine()
    {
        Driver.moveSpeed = 0f;
        Driver.steerSpeed = 0f;
        infoButton.image.color = infoBackgroundColor01;
        infoText.color = new Color32(245, 210, 0, 255);
        infoTextValue = "Goal: get 1000 points to leave the town.";  
        infoText.text = infoTextValue;
        yield return new WaitForSeconds(ToFloat(2.5));
        infoText.color = new Color32(0, 55, 243, 255);
        infoTextValue = "Deliver packages to the customer.";
        infoText.text = infoTextValue;
        yield return new WaitForSeconds(ToFloat(2.5));
        infoText.color = new Color32(255, 6, 6, 255);
        infoTextValue = "Be careful with the condition of your car!";
        infoText.text = infoTextValue;
        yield return new WaitForSeconds(2);
        goText.text = "GO!";
        Driver.moveSpeed = 15f;
        Driver.steerSpeed = 250f;
        infoText.color = new Color32(255, 255, 255, 255);
        infoTextValue = "Someone will try to stop you...";
        infoText.text = infoTextValue;
        yield return new WaitForSeconds(2);
        infoTextValue = ""; 
        goText.text = "";
        infoText.text = infoTextValue;
        infoButton.image.color = infoBackgroundColor02;
    }

    void Update()
    {
        scoreTextValue = "Score: " + Driver.scoreValue.ToString();
        scoreText.text = scoreTextValue;  
        statusTextValue = "Car status: " + Driver.carHP.ToString() + "%";
        statusText.text = statusTextValue;  
        infoText.text = infoTextValue;

        if (Driver.carHP <= 0)
        {
            Driver.moveSpeed = 0f;
            Driver.steerSpeed = 0f;
            StartCoroutine(GameOver());
        }

        if (Driver.scoreValue == 1000)
        {
            Driver.moveSpeed = 0f;
            Driver.steerSpeed = 0f;
            StartCoroutine(PlayerWin());
        }
    }

    IEnumerator PlayerWin()
    {
        infoText.color = new Color32(245, 210, 0, 255);
        infoButton.image.color = infoBackgroundColor01;
        SoundManagerScript.PlaySound("playerWin");
        infoTextValue = "YOU WIN!";
        yield return new WaitForSeconds(2);
        infoTextValue = "";
        infoButton.image.color = infoBackgroundColor02;
        infoText.color = new Color32(255, 255, 255, 255);
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    IEnumerator GameOver()
    {
        infoText.color = new Color32(194, 197, 204, 255);
        infoButton.image.color = infoBackgroundColor01;
        infoTextValue = "GAME OVER!";
        yield return new WaitForSeconds(2);
        infoTextValue = "";
        infoButton.image.color = infoBackgroundColor02;
        infoText.color = new Color32(255, 255, 255, 255);
        SceneLoader.LoadStartScene();
    }

    void OnCollisionEnter2D(Collision2D coll) 
    {
        if (coll.collider.tag == "Enemy" || coll.collider.tag == "Env")
        {
            if (Driver.carHP > 0)
            {
                //Debug.Log("D: Auæ, to musia³o boleæ!");
                SoundManagerScript.PlaySound("playerHit");
                StartCoroutine(HitCoroutine());
            }
        }

        if (coll.collider.tag == "Block")
        {
            StartCoroutine(BlockAccessCoroutine());
        }
    }

    IEnumerator HitCoroutine()
    {
        spriteRenderer.color = hitColor;
        Driver.carHP -= 5;
        yield return new WaitForSeconds(1);

        if (Driver.hasPackage == false)
        {
            spriteRenderer.color = noPackageColor;
        }
        else {
            spriteRenderer.color = hasPackageColor;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Package" && !Driver.hasPackage)
        {
            StartCoroutine(PickUpPackageCoroutine());
            Destroy(other.gameObject, destroyDelay);
        }
        else if (other.tag == "Package" && Driver.hasPackage)
        {
            StartCoroutine(DriveHavePackageCoroutine());
        }

        if (other.tag == "Customer" && Driver.hasPackage)
        {
            StartCoroutine(PackageDeliveryCoroutine());
        }
        else if (other.tag == "Customer" && !Driver.hasPackage)
        {
            StartCoroutine(DriveHaveNoPackageCoroutine());
        }

        if (other.tag == "Boost")
        {
            StartCoroutine(GotBoostCoroutine());
        }
    }

    IEnumerator PickUpPackageCoroutine()
    {
        infoText.color = new Color32(0, 243, 88, 255);
        infoButton.image.color = infoBackgroundColor01;
        SoundManagerScript.PlaySound("playerGetNot");
        infoTextValue = "A package has been picked up!";
        Driver.hasPackage = true;
        spriteRenderer.color = hasPackageColor;
        yield return new WaitForSeconds(ToFloat(1.5));
        infoTextValue = "";
        infoButton.image.color = infoBackgroundColor02;
        infoText.color = new Color32(255, 255, 255, 255);
    }

    IEnumerator DriveHaveNoPackageCoroutine()
    {
        infoButton.image.color = infoBackgroundColor01;
        SoundManagerScript.PlaySound("playerGetNot");
        infoTextValue = "You don't have a package for me. Come back later!";
        yield return new WaitForSeconds(ToFloat(1.5));
        infoTextValue = "";
        infoButton.image.color = infoBackgroundColor02;
    }

    IEnumerator DriveHavePackageCoroutine()
    {
        infoButton.image.color = infoBackgroundColor01;
        SoundManagerScript.PlaySound("playerGetNot");
        infoTextValue = "You already have one package!";
        yield return new WaitForSeconds(ToFloat(1.5));
        infoTextValue = "";
        infoButton.image.color = infoBackgroundColor02;
    }

    IEnumerator PackageDeliveryCoroutine()
    {
        if (Driver.carHP < 100)
        {
            Driver.carHP += 5;
        }

        infoText.color = new Color32(0, 55, 243, 255);
        infoButton.image.color = infoBackgroundColor01;
        SoundManagerScript.PlaySound("playerGetScore");
        infoTextValue = "A package has been delivered!";
        Driver.hasPackage = false;
        spriteRenderer.color = noPackageColor;
        Driver.scoreValue += 200;
        yield return new WaitForSeconds(ToFloat(1.5));
        infoTextValue = "";
        infoButton.image.color = infoBackgroundColor02;
        infoText.color = new Color32(255, 255, 255, 255);
    }

    IEnumerator BlockAccessCoroutine()
    {
        infoButton.image.color = infoBackgroundColor01;
        SoundManagerScript.PlaySound("playerGetNot");
        infoTextValue = "You do not have access to this part of the map.";
        yield return new WaitForSeconds(ToFloat(1.5));
        infoTextValue = "Unlock it by earning points.";
        yield return new WaitForSeconds(ToFloat(1.5));
        infoTextValue = "";
        infoButton.image.color = infoBackgroundColor02;
    }

    IEnumerator GotBoostCoroutine()
    {
        infoText.color = new Color32(255, 227, 0, 255);
        infoButton.image.color = infoBackgroundColor01;
        SoundManagerScript.PlaySound("playerGetBoost");
        infoTextValue = "Booosted!";
        yield return new WaitForSeconds(ToFloat(1.5));
        infoTextValue = "";
        infoButton.image.color = infoBackgroundColor02;
        infoText.color = new Color32(255, 255, 255, 255);
    }
}