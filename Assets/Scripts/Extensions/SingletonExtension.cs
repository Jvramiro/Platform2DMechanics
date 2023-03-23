using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SingletonExtensions{
    public class SingletonExtension : MonoBehaviour
    {
        public static Interactions GetInteractions(){
            return FindObjectOfType<Interactions>();
        }
    }
}
