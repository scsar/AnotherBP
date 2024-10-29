using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("PortalId") == SceneManager.GetActiveScene().buildIndex)
        {
            float x = PlayerPrefs.GetFloat("SavedX");
            float y = PlayerPrefs.GetFloat("SavedY");

            GameObject.FindWithTag("Player").transform.position = new Vector2(x, y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SavePosition();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void SavePosition()
    {
        // 현재 위치 저장
        PlayerPrefs.SetFloat("SavedX", transform.position.x - 2f);
        PlayerPrefs.SetFloat("SavedY", transform.position.y - 1f);
        PlayerPrefs.SetInt("PortalId", SceneManager.GetActiveScene().buildIndex);
    }
}
