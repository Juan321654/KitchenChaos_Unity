using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private float footstepVolume = 0.35f;
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = 0.1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;

            if (player.IsWalking())
            {
                SoundManager.Instance.PlayFootstepsSound(Camera.main.transform.position, footstepVolume);
            }
        }
    }
}
