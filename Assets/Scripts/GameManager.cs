using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    public static GameManager Gm;
    public List<Vector2> ocuppedPos;
    public List<GameObject> GeneratedRooms;
    public GameObject PlayerTorso;
    public GameObject DmgText;
    float DmgSum;
    public Material highLight;
    public GameObject Sparks;
    [SerializeField] Color PlayerDmgColor;
    public GameObject PlayerEnemy = null;
    public List<Collider2D> AlliesColliders;
    public List<Collider2D> EnemyColliders;

    void Awake()
    {
        Gm = this;
    }

    private void Start()
    {
        StartCoroutine(CheckRoomDistance());
    }

    public void UpdateColliders(Collider2D col, bool t, bool Allie)
    {
        if (Allie)
        {
            AlliesColliders.Add(col); 
            foreach (Collider2D AllieCol1 in AlliesColliders)
            {
                Physics2D.IgnoreCollision(AllieCol1, col, t);
            }
            foreach(GameObject InteractionZoneCollider in GameObject.FindGameObjectsWithTag("InteractionZone"))
            {
                Physics2D.IgnoreCollision(InteractionZoneCollider.GetComponent<Collider2D>(), col, t);
            }
        }
        else
        {
            EnemyColliders.Add(col);
            foreach (Collider2D EnemyCol1 in EnemyColliders)
            {
                Physics2D.IgnoreCollision(EnemyCol1, col, t);
            }
        }
    }

    private void Update()
    {
        if(PlayerEnemy != null)
        {
            if (PlayerEnemy.transform.parent.TryGetComponent(out AI ai))
            {
                if (!ai.enabled) { PlayerEnemy = null; }
            }
        }
    }

    public IEnumerator CheckRoomDistance()
    {
        while (true)
        {
            foreach (GameObject room in GeneratedRooms)
            {
                if (Vector2.Distance(room.transform.position, PlayerTorso.transform.position) < 200)
                    room.SetActive(true);
                else
                    room.SetActive(false);
            }
            yield return new WaitForSeconds(0.3f); 
        }
    }

    public Vector2 GetMouseVector(Vector3 pos)
    {
        Vector2 dir;
        dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos;
        dir.Normalize();
        return dir;
    }

    public IEnumerator DoDamage(Collision2D collision, Rigidbody2D rb, float DmgMultiplier, bool PickedByEnemy)
    {
        if ((collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("Player")) && rb.velocity.magnitude > 20 && collision.transform.TryGetComponent(out PartsLifes pl) && pl.lifes > 0)
        {
            if(!PickedByEnemy && collision.gameObject.GetComponent<AIPartLifes>())
            {
                PlayerEnemy = collision.gameObject;
            }
            float dmg = DmgMultiplier * rb.velocity.magnitude;
            DmgSum += dmg;
            collision.transform.GetComponent<PartsLifes>().lifes -= dmg;

            yield return new WaitForSeconds(0.05f);

            if (DmgSum > 0)
                DmgText.GetComponent<TextMeshPro>().text = Mathf.RoundToInt(DmgSum) + "";
            else
                DmgText.GetComponent<TextMeshPro>().text = "";

            if (!collision.gameObject.CompareTag("Player"))
                DmgText.GetComponent<TextMeshPro>().color = PlayerDmgColor;
            else
                DmgText.GetComponent<TextMeshPro>().color = Color.red;

            Instantiate(DmgText, collision.GetContact(0).point, Quaternion.identity);
            DmgSum = 0;
        } 

        if (collision.gameObject.TryGetComponent(out HingeJoint2D hing) && collision.gameObject.CompareTag("rope") && rb.velocity.magnitude > 20)
        {
            Destroy(hing);
        }

        if(collision.gameObject.CompareTag("Pickable") && rb.velocity.magnitude > 40)
        {
            ParticleSystem Ps = Sparks.GetComponent<ParticleSystem>();
            Sparks.transform.position = collision.GetContact(0).point;
            var Emission = Ps.emission;
            Emission.rateOverTime = rb.velocity.magnitude * 5;
            Ps.Play();
        }
    }
}