using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTargetEnvironment : MonoBehaviour
{  

    [SerializeField] private Transform ShootFlash;
    [SerializeField] private Transform NoFlash;
    public Transform yellowbirds;
    public Transform blackbirds;
    public Transform redbirds;
    public int ammo;

    private Rigidbody2D YbirdRigidbody;
    private SpriteRenderer YbirdSpriteRenderer;
    private Rigidbody2D RbirdRigidbody;
    private SpriteRenderer RbirdSpriteRenderer;
    private Rigidbody2D BbirdRigidbody;
    private SpriteRenderer BbirdSpriteRenderer;
    private Transform RedTransform;
    private int need_red = 0;

    private void Awake() {
        YbirdRigidbody = yellowbirds.GetComponent<Rigidbody2D>();
        YbirdSpriteRenderer = yellowbirds.GetComponent<SpriteRenderer>();
        BbirdRigidbody = blackbirds.GetComponent<Rigidbody2D>();
        BbirdSpriteRenderer = blackbirds.GetComponent<SpriteRenderer>();
        RbirdRigidbody = redbirds.GetComponent<Rigidbody2D>();
        RbirdSpriteRenderer = redbirds.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        ammo = 10;
        SpawnYellowBird();   
        SpawnBlackBird();
        SpawnRedBird();
    }

    // Update is called once per frame
    private void Update()
    {
        if (yellowbirds.localPosition.y < -25f) {
            // Reset
            SpawnYellowBird();
        }
        if (blackbirds.localPosition.y < -25f) {
            // Reset
            SpawnBlackBird();
        }
        if (need_red > 2 && RbirdRigidbody.constraints == RigidbodyConstraints2D.FreezeAll) {
            // Spawn red
            SpawnRedBird();
            need_red = 0;
        }
        if (redbirds.localPosition.x > 50f || redbirds.localPosition.x < -40f){
            // shut red
            redbirds.localPosition = new Vector3(49.8f, -19.9f);
            RbirdRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (ammo < -3){
            ammo = 10;
        }
    }

    private void SpawnYellowBird()
    {
        float force = Random.Range(15f, 20f);
        bool leftSide = Random.Range(0, 100) < 50;
        float sideMultiplier = leftSide ? -1f : +1f;
        yellowbirds.localPosition = new Vector3(28 * sideMultiplier, -18f + UnityEngine.Random.Range(0, 20f));
        YbirdSpriteRenderer.flipX = !leftSide;
        YbirdRigidbody.velocity = Vector2.zero;
        YbirdRigidbody.AddForce(new Vector2(-1f * sideMultiplier, Random.Range(.5f, 1.6f)) * force, ForceMode2D.Impulse);
    }

    private void SpawnRedBird()
    {
        need_red = 0;
        RbirdRigidbody.constraints = RigidbodyConstraints2D.None;
        float force = Random.Range(15f, 30f);
        bool leftSide = Random.Range(0, 100) < 50;
        float sideMultiplier = leftSide ? -1f : +1f;
        redbirds.localPosition = new Vector3(28 * sideMultiplier, -18f + UnityEngine.Random.Range(0, 20f));
        RbirdSpriteRenderer.flipX = !leftSide;
        RbirdRigidbody.velocity = Vector2.zero;
        RbirdRigidbody.AddForce(new Vector2(-1f * sideMultiplier, Random.Range(.5f, 1.6f)) * force, ForceMode2D.Impulse);
    }

    private void SpawnBlackBird()
    {
        float force = Random.Range(20f, 30f);
        bool leftSide = Random.Range(0, 100) < 50;
        float sideMultiplier = leftSide ? -1f : +1f;
        blackbirds.localPosition = new Vector3(28 * sideMultiplier, -18f + UnityEngine.Random.Range(0, 20f));
        BbirdSpriteRenderer.flipX = !leftSide;
        BbirdRigidbody.velocity = Vector2.zero;
        BbirdRigidbody.AddForce(new Vector2(-1f * sideMultiplier, Random.Range(.5f, 1.6f)) * force, ForceMode2D.Impulse);
    }

    public int ShootAt(Vector3 shootPosition) 
    {
        Collider2D collider = Physics2D.OverlapPoint(shootPosition);
        Transform shootFlashTransform;

        if (ammo < 0)
        {
            //No ammo hit is a miss
            shootFlashTransform = Instantiate(NoFlash, shootPosition, Quaternion.identity);
            Destroy(shootFlashTransform.gameObject, 0.4f);
            return 4;
        }

        shootFlashTransform = Instantiate(ShootFlash, shootPosition, Quaternion.identity);
        Destroy(shootFlashTransform.gameObject, 0.4f);
        
        if (collider != null) 
        {
            Transform birdTransform = collider.GetComponent<Transform>();

            if (collider.TryGetComponent<Redbird>(out Redbird redbird)) 
            {
                //reset and free ammo for hitting bonus
                redbirds.localPosition = new Vector3(49.8f, -19.9f);
                ammo += 1;
                RbirdRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                need_red = 0;
                return 1;
            }
            else if (collider.TryGetComponent<Yellowbird>(out Yellowbird yellowbird)) 
            {
                need_red += 1;
                SpawnYellowBird();
                return 2;
            }
            else if (collider.TryGetComponent<Blackbird>(out Blackbird blackbird)) 
            {
                SpawnBlackBird();
                return 3;
            }
        }
        return 4;
    }
}
