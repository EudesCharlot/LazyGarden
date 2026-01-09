using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;
    float droneSpeed;
    void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }


    void Update()
    {
        droneSpeed = playerController.GetSpeed();
        
        float distanceToVehicle = Vector3.Distance(transform.position, player.transform.position);
        
        transform.LookAt(player.transform);
    }
}
