using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour
{
    public AudioSource audioSource;
    public List<Transform> bodyParts = new List<Transform>();
    public GameObject bodyprefabs;
    public int beginSize;
    public List<Sprite> levelSprites;
    public AudioClip hitClip;
    public AudioClip growClip;
    public AudioClip winClip;
    public AudioClip looseClip;
    public GameObject mainSound;
    public float cooldownDamage = 1f;
    float cdDamage;

    public float speed = 1;
    public float rotationSpeed = 50;
    public float backDistance = 2;
    public int level = 0;

    public bool isSlowed = false;
    public float slowTime = 2f;
    public float minDist = 0.5f;

    public GameObject deathScreen;
    public GameObject winScreen;
    public Clock clock;


    // Start is called before the first frame update
    void Start()
    {
        cdDamage = cooldownDamage;
        for (int i = 0; i < beginSize - 1; i++)
        {
            AddBodyPart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    AddBodyPart();
        //}

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    RemoveBodyParts(3);
        //}

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    BounceBack();
        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    LevelUp();
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    LevelDown();
        //}

        cdDamage -= Time.deltaTime;
        Move();

        if (isSlowed)
        {
            slowTime -= Time.deltaTime;
            if (slowTime < 0)
            {
                isSlowed = false;
            }
        }
    }

    public void Move()
    {
        float curspeed = speed;
        if (Input.GetKey(KeyCode.Space))
        {
            curspeed /= 2;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            curspeed *= 2;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (vertical != 0 || horizontal != 0)
        {
            Vector3 target = new Vector3(vertical * float.MaxValue, -horizontal * float.MaxValue);
            Vector2 direction = target - bodyParts[0].transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            bodyParts[0].transform.rotation = Quaternion.Slerp(bodyParts[0].transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        bodyParts[0].Translate(bodyParts[0].up * curspeed * Time.deltaTime, Space.World);

        for (int i = 1; i < bodyParts.Count; i++)
        {
            Transform curBodyPart = bodyParts[i];
            Transform PrevBodyPart = bodyParts[i - 1];

            float dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);

            Vector3 newpos = PrevBodyPart.position;

            float T = Time.deltaTime * dis / minDist * curspeed;
            curBodyPart.position = Vector3.Lerp(curBodyPart.position, newpos, T);
            curBodyPart.rotation = Quaternion.Lerp(curBodyPart.rotation, PrevBodyPart.rotation, T);

        }
    }

    public void death()
    {
        speed = 0;
        clock.isRunning = false;
        FinishGame();
        StartCoroutine(DeathEnumerator());
    }

    private void FinishGame()
    {
        CameraController cameraController = FindObjectOfType<CameraController>();
        cameraController.finished = true;
        AttackerSpawnerController attackerSpawnerController = FindObjectOfType<AttackerSpawnerController>();
        attackerSpawnerController.interval = float.MaxValue;
    }

    public void win()
    {
        speed = 0;
        clock.isRunning = false;
        FinishGame();
        StartCoroutine(WinEnumerator());
    }

    public void AddBodyPart()
    {
        Transform newpart = (Instantiate(bodyprefabs, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;

        newpart.SetParent(transform);

        bodyParts.Add(newpart);
        audioSource.clip = growClip;
        audioSource.Play();
    }

    public void RemoveBodyParts(int count)
    {
        if (cdDamage <= 0)
        {
            if (bodyParts.Count <= 2)
            {
                death();
                return;
            }

            if (count >= bodyParts.Count)
            {
                count = bodyParts.Count - 1;
            }

            for (int i = 0; i < count; i++)
            {
                int index = bodyParts.Count - 1;
                Destroy(bodyParts[index].gameObject);
                bodyParts.RemoveAt(index);
                audioSource.clip = hitClip;
                audioSource.Play();
            }

            cdDamage = cooldownDamage;
        }
    }

    public void BounceBack()
    {
        for (int i = bodyParts.Count - 1; i >= 0; i--)
        {
            Vector3 direction = -bodyParts[0].up;
            bodyParts[i].Translate(direction * backDistance, Space.World);
        }
        Slow(2);
    }

    public void Slow(float seconds)
    {
        if (!isSlowed)
        {
            isSlowed = true;
        }
        slowTime = seconds;
    }

    public void LevelUp()
    {
        if (level < levelSprites.Count - 1)
        {
            level++;
            UpdateHeadSprite();
        }
        else
        {
            win();
        }
    }
    public void LevelDown()
    {
        if (level > 0)
        {
            level--;
            UpdateHeadSprite();
        }
    }

    public void UpdateHeadSprite()
    {
        bodyParts[0].GetComponent<SpriteRenderer>().sprite = levelSprites[level];
    }

    public void SetLevel(int newLevel)
    {
        if (newLevel >= 0 || newLevel < levelSprites.Count)
        {
            level = newLevel;
            UpdateHeadSprite();
        }
    }

    IEnumerator WinEnumerator()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        winScreen.SetActive(true);
        mainSound.GetComponent<AudioSource>().clip = winClip;
        mainSound.GetComponent<AudioSource>().Play();
    }

    IEnumerator DeathEnumerator()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        deathScreen.SetActive(true);

        mainSound.GetComponent<AudioSource>().clip = looseClip;
        mainSound.GetComponent<AudioSource>().loop = false;
        mainSound.GetComponent<AudioSource>().Play();
    }
}