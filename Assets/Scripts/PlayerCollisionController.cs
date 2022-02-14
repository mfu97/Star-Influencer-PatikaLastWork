using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    #region Variables
    [SerializeField] private ParticleSystem smokePink;
    [SerializeField] private ParticleSystem smokeRed;
    [SerializeField] private ParticleSystem confetti;

    [SerializeField] private Transform sideMovementRoot;

    [SerializeField] private List<GameObject> bodies;

    private Vector3 finalPos;
    private Vector3 endPos;

    private int index;
    private int point;

    private float endingLerpTimer;
    private float endingLerpDuration = 1f;

    private bool finishLine;


    public bool End;
    public bool FullReward;
    #endregion

    #region SINGLETON
    public static PlayerCollisionController Instance => instance ??= FindObjectOfType<PlayerCollisionController>();
    private static PlayerCollisionController instance;
    #endregion

    private void Update()
    {
        if (finishLine)
        {
            Timer();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // Positive Gate
        {
            point += 50;
            UIManager.Instance.SetTypeBar(point);
            if (point >= 100 && index < bodies.Count - 1)
            {
                index++;
                smokePink.Play();
                ChangeType();
                UIManager.Instance.SetTypeBar(0);
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == 8) // Negative Gate;
        {
            point -= 50;
            UIManager.Instance.SetTypeBar(point);
            if (point <= 0 && index > 0)
            {
                index--;
                smokeRed.Play();
                ChangeType();
                UIManager.Instance.SetTypeBar(50);
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == 9) // Positive Collectables
        {
            point += 5;
            if (point >= 100 && index < bodies.Count - 1)
            {
                index++;
                smokePink.Play();
                ChangeType();
                UIManager.Instance.SetTypeBar(0);
            }
            UIManager.Instance.SetTypeBar(point);
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == 10) // Negative Collectables
        {
            point -= 5;
            UIManager.Instance.SetTypeBar(point);
            if (point <= 0 && index > 0)
            {
                index--;
                smokeRed.Play();
                ChangeType();
                UIManager.Instance.SetTypeBar(50);
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == 7) // Ending
        {
            SetFinishLine();
        }
    }

    private void ChangeType()
    {
        point = 0;
        foreach (var body in bodies)
        {
            body.SetActive(false);
        }
        bodies[index].SetActive(true);
        Player.Instance.AnimationChange();
    }

    private void Timer()
    {
        if (endingLerpTimer < endingLerpDuration)
        {
            endingLerpTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(endPos, finalPos, endingLerpTimer / endingLerpDuration);
            sideMovementRoot.position = Vector3.Lerp(endPos, finalPos, endingLerpTimer / endingLerpDuration);
        }
        else
        {
            endingLerpTimer = 0;
            finishLine = false;
            transform.position = finalPos;
            confetti.Play();
            GameManager.Instance.OnLevelEnd();
            for (int i = 0; i < bodies.Count; i++)
            {
                if (bodies[i].activeSelf) // 0 1 2 3
                {
                    UIManager.Instance.SetFinishText(i);
                    UIManager.Instance.SetFinishStars(i - 1);
                    FullReward = (i == bodies.Count - 1);
                    if (i==bodies.Count-1)
                    {
                        if (point>=100)
                        {
                            point = 100;
                        }
                    }
                }
            }
            Player.Instance.AnimationChange();
        }
    }
    private void SetFinishLine()
    {
        UIManager.Instance.SetBarActice(false);
        End = true;
        finishLine = true;
        endPos = transform.position;
        finalPos = LevelManager.GetCurrentLevel().GetFinalPos();
        endingLerpDuration = 1 + (0.33f * (point / 10));
    }

    public void GameReset()
    {
        confetti.Stop();
        transform.position = Vector3.zero;
        index = 0;
        ChangeType();
        End = false;
    }
}
