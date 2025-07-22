using UnityEngine;
namespace Invector
{
    public class vRotateObject : MonoBehaviour
    {
        public Vector3 rotationSpeed;

        // Update is called once per frame
        void Update()
        {
            if(this.gameObject.activeSelf)
                transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}