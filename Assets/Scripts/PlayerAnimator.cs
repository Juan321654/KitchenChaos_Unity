using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Player player;

    private const string IS_WALKING = "IsWalking";
    
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
