using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Gm;
    public List<Vector2> ocuppedPos;
    public List<GameObject> GeneratedRooms;
    public GameObject PlayerTorso;
    float DmgSum;
    public Material highLight;
    public GameObject Sparks;
    [SerializeField] Color PlayerDmgColor;
    public GameObject PlayerEnemy = null, EnemyLifeBar;
    public List<Collider2D> AlliesColliders;
    public List<Collider2D> EnemyColliders;
    public List<FriendAI> Friends;
    public GameObject DeathScreen, LoadingScreen;
    public float MoneyAmount;
    float lastAmount = 0;
    [SerializeField]RoomsGenerator roomGenerator;
    public float SightDistance = 200;
    DmgText txt;
    [SerializeField] TextMeshProUGUI MoneyCounter;
    public Slider WorldLoadingSlider;
    public Tilemap tilemap;

    void Awake()
    {
        Gm = this;
    }

    private void Start()
    {
        StartCoroutine(CheckRoomDistance());
    }

    private void Update()
    {
        CheckIfEnemyIsAlive();
        if (MoneyAmount != lastAmount)
        {
            if (lastAmount < MoneyAmount)
                lastAmount++;
            else if (lastAmount > MoneyAmount)
                lastAmount--;
            MoneyCounter.text = lastAmount + "";
        }
    }

    public void UpdateAllies()
    {
        float relDis = 0;
        foreach(FriendAI friendAI1 in Friends)
        {
            relDis += 5;
            friendAI1.PlayerPersonalDistance = relDis;
        }
    }


    private void CheckIfEnemyIsAlive()
    {
        if (PlayerEnemy != null)
        {
            if (PlayerEnemy.transform.parent.TryGetComponent(out AI ai))
            {
                if (!ai.enabled) 
                {
                    PlayerEnemy = null; EnemyLifeBar.SetActive(false); 
                }
                else if(PlayerEnemy.transform.parent.TryGetComponent(out StickmanLifesManager lifesManager)) 
                {
                    EnemyLifeBar.SetActive(true);
                    EnemyLifeBar.transform.position = new Vector2(ai.torso.transform.position.x, ai.torso.transform.position.y + 5);
                    EnemyLifeBar.GetComponent<Slider>().minValue = lifesManager.requiredDmgToDie;
                    EnemyLifeBar.GetComponent<Slider>().value = lifesManager.TotalLife / lifesManager.MaxLife;
                }
            }
        }
    }

    public IEnumerator CheckRoomDistance()
    {
        while (true)
        {
            foreach (GameObject room in GeneratedRooms)
            {
                if (Vector2.Distance(room.transform.position, PlayerTorso.transform.position) < SightDistance)
                    room.SetActive(true);
                else
                    room.SetActive(false);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public Vector2 GetMouseVector(Vector3 pos)
    {
        Vector2 dir;
        dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos;
        dir.Normalize();
        return dir;
    }

    public IEnumerator DoDamage(Collision2D collision, Rigidbody2D rb, float DmgMultiplier, bool PickedByEnemy, float minVel, GameObject Holder)
    {
        if ((collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("Player")) && rb.velocity.magnitude >= minVel && collision.transform.TryGetComponent(out PartsLifes pl) && pl.lifes > 0)
        {
            if (!PickedByEnemy)
            {
                PlayerEnemy = collision.gameObject;
            }
            float dmg = DmgMultiplier * rb.velocity.magnitude;
            DmgSum += dmg;
            pl.lifes -= dmg;
            pl.Damager = Holder;

            yield return new WaitForSeconds(0.05f);
            DmgTextManagement(collision);
            DmgSum = 0;
        }

        if (collision.gameObject.TryGetComponent(out HingeJoint2D hing) && collision.gameObject.CompareTag("rope") && rb.velocity.magnitude > 20)
        {
            Destroy(hing);
        }

        if(collision.gameObject.CompareTag("Pickable") && rb.velocity.magnitude > 40)
        {
            ParticleSystemPlay(collision, rb, Sparks);
        }
    }

    private void ParticleSystemPlay(Collision2D collision, Rigidbody2D rb, GameObject particleSystem)
    {
        ParticleSystem Ps = particleSystem.GetComponent<ParticleSystem>();
        particleSystem.transform.position = collision.GetContact(0).point;
        var Emission = Ps.emission;
        Emission.rateOverTime = rb.velocity.magnitude * 5;
        Ps.Play();
    }

    private void DmgTextManagement(Collision2D collision)
    {
        if (DmgSum > 0)
        {
            txt = ObjectPooler.pool.GetPooledObject(1).GetComponent<DmgText>();
            txt.TextSetting(collision.GetContact(0).point, Mathf.RoundToInt(DmgSum), Color.white);
            if (!collision.gameObject.CompareTag("Player"))
            {
                txt.txt.color = PlayerDmgColor;
            }
            else
            {
                txt.txt.color = Color.red;
            }
        }
    }

    public void GenerateNewWorld()
    {
        DeathScreen.SetActive(false);
        LoadingScreen.SetActive(true);
        tilemap.ClearAllTiles();
        ocuppedPos = new List<Vector2>();
        GeneratedRooms = new List<GameObject>();
        Destroy(roomGenerator.WorldContainer);
        StartCoroutine(roomGenerator.Produce());
    }
}
    //public void UpdateColliders(List<Collider2D> Colliders, bool active, List<Collider2D> list)
    //{
    //    foreach (Collider2D col1 in Colliders)
    //    {
    //        if (active)
    //        {
    //            list.Add(col1); 
    //        }

    //        foreach (Collider2D col2 in list)
    //        {
    //            Physics2D.IgnoreCollision(col1, col2, active);
    //        }

    //        if (!active)
    //        {
    //            list.Remove(col1);
    //        }
    //    }
    //}

    //public void UpdateColliders(Collider2D col1, bool active, List<Collider2D> list)
    //{
    //    if (active)
    //    {
    //        list.Add(col1); 
    //    }

    //    foreach (Collider2D col2 in list)
    //    {
    //        Physics2D.IgnoreCollision(col1, col2, active);
    //    }

    //    if (!active)
    //    {
    //        list.Remove(col1);
    //    }
    //}