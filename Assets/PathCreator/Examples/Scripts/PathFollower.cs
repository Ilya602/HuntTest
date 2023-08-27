using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public Vector3 startPosOffset;
        public Vector3 rotationOffset;
        float distanceTravelled;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                Vector3 path = pathCreator.path.GetPointAtDistance(distanceTravelled + startPosOffset.z, endOfPathInstruction);
                Quaternion rot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

                distanceTravelled += speed * Time.deltaTime;
                transform.position = new Vector3(path.x + startPosOffset.x, path.y, path.z);
                transform.rotation = new Quaternion(rot.x + rotationOffset.x, rot.y + rotationOffset.y, rot.z + rotationOffset.z, rot.w);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}