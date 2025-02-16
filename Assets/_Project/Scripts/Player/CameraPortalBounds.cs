using UnityEngine;

namespace _Project.Scripts.Player
{
    public class CameraPortalBounds : MonoBehaviour
    {
        private Camera _camera;
        private float _objectHeight;
        private float _objectWidth;

        private void Start()
        {
            _camera = Camera.main;

            BoxCollider2D objectCollider = gameObject.GetComponent<BoxCollider2D>();
            if (objectCollider != null)
            {
                _objectHeight = objectCollider.size.y;
                _objectWidth = objectCollider.size.x;
            }
        }

        private void Update()
        {
            WrapAroundCamera();
        }

        private void WrapAroundCamera()
        {
            Vector3 playerPosition = transform.position;

            Vector3 min = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            Vector3 max = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));

            if (playerPosition.y > max.y + _objectHeight)
            {
                playerPosition.y = min.y + _objectHeight;
                transform.position = playerPosition;
            }
            else if (playerPosition.y < min.y - _objectHeight)
            {
                playerPosition.y = max.y - _objectHeight;
                transform.position = playerPosition;
            }

            if (playerPosition.x > max.x + _objectWidth)
            {
                playerPosition.x = min.x + _objectWidth;
                transform.position = playerPosition;
            }
            else if (playerPosition.x < min.x - _objectWidth)
            {
                playerPosition.x = max.x + _objectWidth;
                transform.position = playerPosition;
            }
        }
    }
}