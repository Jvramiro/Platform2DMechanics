using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollisionExtensions{
    public class CollisionExtension : MonoBehaviour
    {
        
        public static bool IsAbove(Collider2D col, Vector3 position){
            Vector2 bottomPoint = new Vector2(col.bounds.center.x, col.bounds.center.y - col.bounds.extents.y);
            bottomPoint.y += 0.02f;
            return bottomPoint.y > position.y;
        }

    }
}

