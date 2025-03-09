using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 70f;
    [SerializeField] private int arrowDamage = 25;
    public void Setup(Vector3 shootDir, float arrowAngle)
    {
        Debug.Log("arrow");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Debug.Log(shootDir);
        transform.eulerAngles = new Vector3(0, 0, arrowAngle);
        rb.AddForce(shootDir*moveSpeed, ForceMode2D.Impulse);
        
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.01f);
    }
    public int GetArrowDamage()
    {
        return arrowDamage;
    }
}
