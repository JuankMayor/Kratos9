﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kratos9
{
    public class ImpactManager : MonoBehaviour
    {
        public bool calculating;


        private void Start()
        {
            calculating = false;
        }
        /// <summary>
        /// Cambia las direcciones y las velocidades entre los botes que colisionan.
        /// </summary>
        /// <param name="collided"></param>
        /// <param name="contact"></param>
        public void CalculateImpactForces(Transform collided, ContactPoint contact)
        {
           // if(!calculating)
            //{
                Debug.Log(calculating);
                calculating = true;
                Kratos9.movement_manager collided_movement_manager = collided.GetComponent<Kratos9.movement_manager>();
                Kratos9.movement_manager other_collider_movement_manager = contact.otherCollider.transform.GetComponent<Kratos9.movement_manager>();
                Kratos9.punch_manager collided_punch_manager = collided.GetComponent<Kratos9.punch_manager>();
                Kratos9.punch_manager other_collider_punch_manager = contact.otherCollider.transform.GetComponent<Kratos9.punch_manager>();
            

                collided_movement_manager.my_anim.SetTrigger("Hit");
                other_collider_movement_manager.my_anim.SetTrigger("Hit");
            Vector3 normal = new Vector3(contact.normal.x, 0, contact.normal.z);
            Debug.Log(contact.normal);

                if (!collided_movement_manager.hitting)
                {

                    if (!other_collider_movement_manager.hitting)
                    {

                        collided_movement_manager.RecieveImpact(normal * (other_collider_movement_manager.GetDirectorVector().magnitude + collided_movement_manager.GetDirectorVector().magnitude * 0.5f)); //* ((collided_movement_manager.GetDirectorVector() * collided_movement_manager.GetSpeed()).magnitude + (other_collider_movement_manager.GetDirectorVector() * other_collider_movement_manager.GetSpeed()).magnitude));
                                                                                                                                                                                                             //collided_movement_manager.RecieveImpact(other_collider_movement_manager.GetDirectorVector() * other_collider_movement_manager.GetSpeed() - collided_movement_manager.GetDirectorVector() * collided_movement_manager.GetSpeed());
                                                                                                                                                                                                             //other_collider_movement_manager.RecieveImpact(collided_movement_manager.GetDirectorVector() * collided_movement_manager.GetSpeed() - other_collider_movement_manager.GetDirectorVector() * other_collider_movement_manager.GetSpeed());
                        other_collider_movement_manager.RecieveImpact(-normal * (other_collider_movement_manager.GetDirectorVector().magnitude * 0.5f + collided_movement_manager.GetDirectorVector().magnitude));

                        if (collided_movement_manager.GetDirectorVector().magnitude > other_collider_movement_manager.GetDirectorVector().magnitude && !other_collider_punch_manager.punch_ready)
                        {
                            other_collider_punch_manager.IncreasePunchCharge(1);

                            if (other_collider_punch_manager.punch_charge >= other_collider_punch_manager.punch_charge_needed)
                            {
                                other_collider_punch_manager.punch_ready = true;
                            }
                        }
                        else if (collided_movement_manager.GetDirectorVector().magnitude < other_collider_movement_manager.GetDirectorVector().magnitude && !collided_punch_manager.punch_ready)
                        {
                            collided_punch_manager.IncreasePunchCharge(1);

                            if (collided_punch_manager.punch_charge >= collided_punch_manager.punch_charge_needed)
                            {
                                collided_punch_manager.punch_ready = true;
                            }
                        }
                    }
                    else
                    {
                        collided_movement_manager.RecieveImpact(normal * (collided_movement_manager.GetDirectorVector().magnitude*0.5f + other_collider_movement_manager.dash_speed));
                        other_collider_movement_manager.GetComponent<Kratos9.punch_manager>().StopPunchEffect();
                    }
                }
                else
                {
                    collided_movement_manager.GetComponent<Kratos9.punch_manager>().StopPunchEffect();

                    if (!other_collider_movement_manager.hitting)
                    {
                        other_collider_movement_manager.RecieveImpact(-normal * (collided_movement_manager.dash_speed + other_collider_movement_manager.GetDirectorVector().magnitude * 0.5f));
                    }
                    else
                    {
                        other_collider_movement_manager.GetComponent<Kratos9.punch_manager>().StopPunchEffect();
                    }
                }

                
          //  }
        }
        
        public void DisableCalculating()
        {
            calculating = false;

        }
    }
}

