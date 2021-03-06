﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kratos9
{

    public class CameraBehaviour : MonoBehaviour
    {

        public List<movement_manager> ships;
        public float min_padding, max_padding;
        Transform camera_transform;
        Camera camera_component;
        Vector3 move_speed = Vector3.zero;
        public float smooth_time;

        public float min_distance;
        public float min_fov;
        public float max_fov;

        float min_left_position;
        float max_right_position;
        float min_bottom_position;
        float max_top_position;

        float rotation_speed = 0.5f;
        

        // Start is called before the first frame update
        void Start()
        {
            camera_transform = transform;
            camera_component = camera_transform.GetComponentInChildren<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 position = GetCenter();
  
            Vector3 target = new Vector3(position.x, camera_transform.position.y, position.y);
            camera_transform.position = Vector3.SmoothDamp(camera_transform.position, target, ref move_speed, smooth_time);
            
            camera_component.fieldOfView = (GetDistance() * min_fov) / min_distance;
            camera_component.fieldOfView = Mathf.Clamp(camera_component.fieldOfView, min_fov, max_fov);



            //foreach (movement_manager ship in ships)
            //{
            //    UpdateValues(ship);
            //    while (CheckIfNeedToGrow()) camera_component.fieldOfView += Time.deltaTime;
            //    if (CheckIfNeedToReduce()) camera_component.fieldOfView -= Time.deltaTime;
            //}
        }


        bool CheckIfNeedToGrow()
        {        

            return (min_left_position - min_padding < 0 || min_bottom_position - min_padding < 0 || max_top_position + min_padding > Screen.height || max_right_position + min_padding > Screen.width);
        }

        bool CheckIfNeedToReduce()
        {
            return (min_left_position - max_padding > 0 && max_right_position + max_padding < Screen.width);
        }

        
        void UpdateValues(movement_manager ship)
        {
            BoxCollider collider = ship.GetComponentInChildren<BoxCollider>();
            Vector2 min_extents = new Vector2(collider.bounds.min.x, collider.bounds.min.y);
            Vector2 max_extents = new Vector2(collider.bounds.max.x, collider.bounds.max.y);

            min_left_position = Camera.main.WorldToScreenPoint(new Vector3(min_extents.x, 0, collider.bounds.center.y)).x;
            max_right_position = Camera.main.WorldToScreenPoint(new Vector3(max_extents.x, 0, collider.bounds.center.y)).x;
            min_bottom_position = Camera.main.WorldToScreenPoint(new Vector3(collider.bounds.center.y, 0, min_extents.y)).z;
            max_top_position = Camera.main.WorldToScreenPoint(new Vector3(collider.bounds.center.y, 0, max_extents.y)).z;

        }


        Vector2 GetCenter()
        {
            float Ax = ships[0].ship_transform.position.x;
            float Az = ships[0].ship_transform.position.z;
            float Bx = ships[1].ship_transform.position.x;
            float Bz = ships[1].ship_transform.position.z;

            return new Vector2(Ax + (Bx - Ax) * 0.5f, Az + (Bz - Az) * 0.5f);
        }

        float GetDistance()
        {
            return Vector3.Distance(ships[0].ship_transform.position, ships[1].ship_transform.position);
        }


    }
}
