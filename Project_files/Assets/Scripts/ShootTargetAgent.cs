using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class ShootTargetAgent : Agent
{
    [SerializeField] private ShootTargetEnvironment shootTargetEnvironment;
    [SerializeField] private Transform mapTransform;
    [SerializeField] private Transform shootTransform;

    //can use shootTargetEnvironment.bird_present to shoot when bird is present
    public override void CollectObservations(VectorSensor sensor){
        bool ammo_avail = shootTargetEnvironment.ammo < 1;
        sensor.AddObservation(ammo_avail);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        int positionX = actions.DiscreteActions[0];
        int positionY = actions.DiscreteActions[1];
        //No shoot Zone. Enter when no bird to hit or no bullets
        if (positionY == 0){
            // Enter No shoot Zone when u have no ammo to get reward
            if (shootTargetEnvironment.ammo < 0){
                    shootTargetEnvironment.ammo -= 1;
                    AddReward(1f);
                    EndEpisode();
                }
        }
        else{
            shootTargetEnvironment.ammo -= 1;
            Vector2 shootPositionLocal = new Vector2(positionX, positionY);
            shootTransform.localPosition = shootPositionLocal;

            Vector2 shootPosition = mapTransform.TransformPoint(shootPositionLocal);
            int shot = shootTargetEnvironment.ShootAt(shootPosition);

            if (shot == 1) 
            {
                //Hit Red Bird!
                AddReward(1.4f);
                EndEpisode();
            } 
            else if (shot == 2) 
            {
                //Hit Yellow Bird!
                AddReward(1f);
                EndEpisode();
            }
            else if (shot == 3) 
            {
                //Hit Black Bird!
                AddReward(-0.5f);
            }
            else 
            {
                //Didn't Hit
                AddReward(-0.5f);
            }
        }
    }
    
    public override void Heuristic(in ActionBuffers actionsOut){
        //Used for making expert demo
        //Vector3 worldPosition = Input.MousePosition();
        Vector3 worldPosition;
        ActionSegment<int> discreteAction = actionsOut.DiscreteActions;
        Rigidbody2D RbirdRigidbody = shootTargetEnvironment.redbirds.GetComponent<Rigidbody2D>();
        if (shootTargetEnvironment.ammo < 1){
            discreteAction[0] = 0;
            discreteAction[1] = 0;   
        }
        else{
            if (RbirdRigidbody.constraints == RigidbodyConstraints2D.None)
            {
                worldPosition = shootTargetEnvironment.redbirds.localPosition;
            }
            else
            {
                worldPosition = shootTargetEnvironment.yellowbirds.localPosition;
            }
            Vector3 localPosition = mapTransform.InverseTransformPoint(worldPosition);
            discreteAction[0] = Mathf.Clamp(Mathf.RoundToInt(localPosition.x), 0, 49);
            discreteAction[1] = Mathf.Clamp(Mathf.RoundToInt(localPosition.y), 0, 49);
        }        
    }
}
