using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;


public class Lives : MonoBehaviour
{

    private Text textField;
    private int lives;
    void Awake()
    {
        //lives = 3;
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "death" || collisionInfo.gameObject.tag == "projectile" || collisionInfo.gameObject.tag == "enemy")
        {
            Destroy(collisionInfo.gameObject);
            commonValues.lives--;
            Debug.Log(lives);

            GUILives.GUIDeath = GUILives.GUIDeath - 1;
            if (commonValues.lives == 0)
            {
                commonValues.lives = 3;
                GUILives.GUIDeath = 3;
                SceneManager.LoadScene(0);
            }
        }
    }
}



