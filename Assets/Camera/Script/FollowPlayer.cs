using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private CircleMouvement        player                          = null;
    [SerializeField] private float                  localPosY                       = 0f;
    [SerializeField] private float                  zoomPlayer                      = 0f;
    [SerializeField] private float                  horizontalCamSpeed              = 0f;
    [SerializeField] private float                  verticalCamSpeed                = 0f;
    [SerializeField] private float                  heightLimit                     = 0f;
    [SerializeField] private float                  lookAtHeightLimit               = 0f;
    [SerializeField] private float                  timeScalePortalTransition       = 1f;
    [SerializeField] private float                  anglePortalTransition           = 1f;
    [SerializeField] private bool                   useDelay                        = true;
    [SerializeField] private bool                   followPlayer                    = true;
    [SerializeField] private bool                   findPlayerAutomatic             = true;
                     private bool                   isPortalTransition              = false;

    private GameObject center                   = null;
    private float      portalTransitionSpeed    = 1f;

    void Start()
    {
        if (findPlayerAutomatic && player == null)
        {
            player = FindObjectOfType<CircleMouvement>();
        }

        player.TeleportEvent += PortalSmoothTransition;
        center = player.origin;

        Vector3 centerToPlayer   = player.transform.position - center.transform.position;
        Vector3 playerToCam2D = new Vector3(centerToPlayer.x, 0f, centerToPlayer.z).normalized * zoomPlayer;
        this.transform.position = player.transform.position + playerToCam2D + new Vector3(0f, localPosY, 0f);
        this.transform.LookAt(new Vector3(center.transform.position.x, player.transform.position.y, center.transform.position.z));
    }
    
    void Update()
    {
        float deltaTimeHorizontalSpeed = Time.unscaledDeltaTime * horizontalCamSpeed * portalTransitionSpeed;
        float deltaTimeVerticalSpeed   = Time.unscaledDeltaTime * verticalCamSpeed * portalTransitionSpeed;

        Vector3 centerToPlayer = player.transform.position - center.transform.position;
        Vector3 playerToCam2D = new Vector3(centerToPlayer.x, 0f, centerToPlayer.z).normalized * zoomPlayer;
        Vector3 posToReach = player.transform.position + playerToCam2D + new Vector3(0f, localPosY, 0f);

        Vector3 centerToPosToReach = posToReach - center.transform.position;
        Vector3 centerToPosToReach2D = new Vector3(centerToPosToReach.x, 0f, centerToPosToReach.z);

        if (useDelay)
        {
            // Get angle between cam and posToReach 
            Vector3 centerToCamera = this.transform.position - center.transform.position;
            Vector3 centerToCamera2D = new Vector3(centerToCamera.x, 0f, centerToCamera.z);

            float angle = Vector3.Angle(centerToCamera2D, centerToPosToReach2D);

            if (isPortalTransition && angle < anglePortalTransition)
            {
                EndPortalSmoothTransition();
            }
            
            Vector3 crossProduct = Vector3.Cross(centerToCamera2D, centerToPosToReach2D);

            if (crossProduct.y < 0)
                angle = -angle;

            // Rotate
            this.transform.RotateAround(center.transform.position, Vector3.up, angle * deltaTimeHorizontalSpeed);

            // Translate on Y axis
            Vector3 lerpToAdd = Vector3.Lerp(this.transform.position, new Vector3(transform.position.x, posToReach.y, transform.position.z), deltaTimeVerticalSpeed);
            if (followPlayer && transform.position.y + lerpToAdd.y <= heightLimit)
                this.transform.position = lerpToAdd;
        }
        else if (!useDelay)
        {
            if (posToReach.y <= heightLimit)
                this.transform.position = posToReach;
            else
                this.transform.position = new Vector3(posToReach.x, heightLimit, posToReach.z);
        }
        
        // LookAt and LookAtHeightLimit
        if (player.transform.position.y <= lookAtHeightLimit)
            this.transform.LookAt(new Vector3(center.transform.position.x, player.transform.position.y, center.transform.position.z));
        else
            this.transform.LookAt(new Vector3(0f, lookAtHeightLimit, 0f));

        // Some Data
        Vector3 centerToThisPos = this.transform.position - center.transform.position;
        Vector3 centerToThisPos2D = new Vector3(centerToThisPos.x, 0f, centerToThisPos.z);

        // If our camera is not the circle
        if (centerToPosToReach2D.magnitude != centerToThisPos2D.magnitude && !isPortalTransition)
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(posToReach.x, this.transform.position.y, posToReach.z), deltaTimeHorizontalSpeed);

        // If our cameraY is too high
        if (useDelay && !followPlayer && this.transform.position.y < center.transform.position.y + localPosY && isPortalTransition)
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, center.transform.position.y + localPosY, this.transform.position.z), deltaTimeVerticalSpeed);
    }

    public void PortalSmoothTransition()
    {
        portalTransitionSpeed = timeScalePortalTransition;
        isPortalTransition = true;
        player.SpeedFactor(Vector2.zero, 100f);
    }

    private void EndPortalSmoothTransition()
    {
        isPortalTransition = false;
        portalTransitionSpeed = 1f;
        player.SpeedFactor(Vector2.zero, 0f);
    }
}
