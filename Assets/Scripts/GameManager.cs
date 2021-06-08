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
    [SerializeField] Color PlayerDmgColor;

    void Awake()
    {
        Gm = this;
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
        if ((collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("Player")) && rb.velocity.magnitude > 20 && collision.transform.GetComponent<PartsLifes>().isActiveAndEnabled)
        {
            float dmg = DmgMultiplier * rb.velocity.magnitude;
            DmgSum += dmg;
            collision.transform.GetComponent<PartsLifes>().lifes -= dmg;

            yield return new WaitForSeconds(0.1f);

            if (DmgSum > 0)
                DmgText.GetComponent<TextMeshPro>().text = Mathf.RoundToInt(DmgSum) + "";
            else
                DmgText.GetComponent<TextMeshPro>().text = "";

            if (!PickedByEnemy)
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
    }
}