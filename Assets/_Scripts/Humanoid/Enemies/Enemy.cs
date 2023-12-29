using UnityEngine;

public class Enemy : Humanoid
{
    [Header("Values")]
    [SerializeField] private float rotationSlerp = 10;

    public Player player { get; private set; }
    private Animator enemyAnim;

    protected override void Awake()
    {
        base.Awake();
        enemyAnim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    protected override void Update()
    {
        base.Update();
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        Vector3 alteredPlayerPos = new Vector3(player.Position().x, transform.position.y, player.Position().z);
        Vector3 playerDirection = alteredPlayerPos - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSlerp);
    }
    
    //Called from other scripts
    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void Hit()
    {
        enemyAnim.SetTrigger("Hit");
    }

}
