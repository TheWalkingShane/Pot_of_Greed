using UnityEngine;
using UnityEngine.Animations;

namespace Card_Game.Card_Room
{
    public class LightSway : MonoBehaviour
    {
        public float minimum =  80.0f;
        public float maximum =  100.0f;
        private GameObject lightBulb;
        static float t = 0.0f;
        
        
        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.Euler(Mathf.Lerp(minimum, maximum, t), 0, 0);

            
            t += 0.1f * Time.deltaTime;
            
            if (t > 1.0f)
            {
                (maximum, minimum) = (minimum, maximum);
                t = 0.0f;
            }
        }
    }
}
