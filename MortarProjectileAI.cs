using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarProjectileAI : MonoBehaviour
{
    public float range;
    public float minRange;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    public float distance;
    public float InstantiationTimer = 3f;
    public float Timer = 3f;

    private GameObject Enemy;
    public GameObject target;
    public GameObject BarrelEnd;
    public GameObject Projectile;
    public GameObject cannonEffect;

    public bool canShoot;
    public bool shootEnable;
    public bool snapped;

    private Transform cachedTransform;

    private Vector3 cachedPosition;

    public GameObject minRangeObject;

    void Start()
    {
        //FindObjectOfType<AudioManager>().PlaySound("CannonFire");
        BarrelEnd = GameObject.FindGameObjectWithTag("BarrelEnd");
        canShoot = false;
        shootEnable = false;
        snapped = false;
        target = FindTarget();
        cachedTransform = GetComponent<Transform>();
        if (target)
        {
            cachedPosition = target.transform.position;
            distance = Vector3.Distance(target.transform.position, transform.position);
        }
    }
    void Update()
    {

        if (shootEnable == true)
        {
            //this.GetComponent<LineRenderer>().enabled = false;
            minRangeObject.GetComponent<LineRenderer>().enabled = false;
            if (!target)
            {
                target = FindTarget();
            }
            else if (target)
            {
                distance = Vector3.Distance(target.transform.position, transform.position);
                if (distance < range && distance > minRange)
                {
                    this.transform.LookAt(target.transform);
                    StartCoroutine(FireProjectile());
                    distance = Vector3.Distance(target.transform.position, transform.position);
                }
                else
                {
                    target = FindTarget();
                }                                           
            }
        }
    }




    IEnumerator FireProjectile()
    {

        // Delay before the projectile is shot.
        InstantiationTimer -= Time.deltaTime;
    

        if (InstantiationTimer <= 0)
        {
            //instantiate mortar shell.
            GameObject mortarShell = Instantiate(Projectile, BarrelEnd.transform.position, BarrelEnd.transform.rotation);
            Instantiate(cannonEffect, BarrelEnd.transform.position, BarrelEnd.transform.rotation);

            // Move projectile to the position.
            mortarShell.transform.position = BarrelEnd.transform.position + new Vector3(0, 0.0f, 0);

            // Calculate distance to target.
            float target_Distance = Vector3.Distance(mortarShell.transform.position, target.transform.position);

            // Calculate the velocity needed to throw the object to the target at specified angle.
            float mortarShell_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

            // Extract the X  Y componenent of the velocity
            float Vx = Mathf.Sqrt(mortarShell_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(mortarShell_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

            // Calculate flight time.
            float flightDuration = target_Distance / Vx;

            // Rotate projectile to face the target.
            mortarShell.transform.rotation = Quaternion.LookRotation(target.transform.position - mortarShell.transform.position);

            float elapse_time = 0;

            while (elapse_time < flightDuration)
            {
                mortarShell.transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
                elapse_time += Time.deltaTime;
                InstantiationTimer = Timer;
                yield return null;
            }


        }

    }
    public GameObject FindTarget()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        float min = minRange;
        float max = 3;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance<distance)
            {               
                    closest = go;
                    distance = curDistance;
                    canShoot = true;               
            }
        }
        return closest;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tower")
        {
            if (other.gameObject.GetComponent<towerController>().snapped == false)
            {
                shootEnable = true;
                snapped = true;
            }
        }
        if (other.gameObject.tag == "Hand")
        {
            this.GetComponent<LineRenderer>().enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hand")
        {
            this.GetComponent<LineRenderer>().enabled = false;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, minRange);
    }
 
}
