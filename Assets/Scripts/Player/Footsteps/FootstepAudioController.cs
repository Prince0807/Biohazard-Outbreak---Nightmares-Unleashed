using UnityEngine;

public class FootstepAudioController : MonoBehaviour
{
    private AudioSource audioSource; // Reference to the Audio Source component
    private CharacterController characterController;
    private TerrainDetector terrainDetector;
    
    [SerializeField]
    private AudioClip stoneClip;
    [SerializeField]
    private AudioClip mudClip;
    [SerializeField]
    private AudioClip grassClip;

    public float minTimeBetweenFootsteps = 0.3f; // Minimum time between footstep sounds
    public float maxTimeBetweenFootsteps = 0.6f; // Maximum time between footstep sounds

    private bool isWalking = false; // Flag to track if the player is walking
    private float timeSinceLastFootstep; // Time since the last footstep sound

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        terrainDetector = new TerrainDetector();
    }

    private void Update()
    {
        if (characterController.velocity.sqrMagnitude > 50)
        {
            isWalking = true;
        }
        else
        {
            isWalking= false;
        }
        // Check if the player is walking
        if (isWalking && characterController.isGrounded)
        {
            // Check if enough time has passed to play the next footstep sound
            if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
            {
                // Play a random footstep sound from the array
                AudioClip clip = GetRandomClip();
                audioSource.PlayOneShot(clip);
                Debug.Log(clip.name);

                timeSinceLastFootstep = Time.time; // Update the time since the last footstep sound
            }
        }
    }

    // Call this method when the player starts walking
    public void StartWalking()
    {
        isWalking = true;
    }

    // Call this method when the player stops walking
    public void StopWalking()
    {
        isWalking = false;
    }

    private AudioClip GetRandomClip()
    {
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

        switch (terrainTextureIndex)
        {
            case 0:
                return stoneClip;
            case 1:
                return mudClip;
            case 2:
            default:
                return grassClip;
        }

    }
}
