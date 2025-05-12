using UnityEngine;

namespace TSS.Utils
{
    public static partial class TSSUtils
    {
        public static float ExpDecay(float a, float b, float decay, float deltaTime) =>
            b + (a - b) * Mathf.Exp(-decay * deltaTime);
        public static Vector2 ExpDecay(Vector2 a, Vector2 b, float decay, float deltaTime) =>
            b + (a - b) * Mathf.Exp(-decay * deltaTime);
        public static Vector3 ExpDecay(Vector3 a, Vector3 b, float decay, float deltaTime) =>
            b + (a - b) * Mathf.Exp(-decay * deltaTime);

        public static Vector2 DampedSpringMotion(Vector2 currentPosition, Vector2 targetPosition, ref Vector2 velocity,
            float dampingRatio, float naturalFrequency, float deltaTime)
        {
            float omega = naturalFrequency * 2 * Mathf.PI;
            float zeta = dampingRatio;

            float f = 1.0f + 2.0f * deltaTime * zeta * omega;
            float oo = omega * omega;
            float hoo = deltaTime * oo;
            float hhoo = deltaTime * hoo;
            float detInv = 1.0f / (f + hhoo);
            float detX = f * currentPosition.x + deltaTime * velocity.x + hhoo * targetPosition.x;
            float detV = velocity.x + hoo * (targetPosition.x - currentPosition.x);

            float detY = f * currentPosition.y + deltaTime * velocity.y + hhoo * targetPosition.y;
            float detVY = velocity.y + hoo * (targetPosition.y - currentPosition.y);

            Vector2 newPosition = new Vector2(detX * detInv, detY * detInv);
            velocity = new Vector2(detV * detInv, detVY * detInv);

            return newPosition;
        }

        public static Vector3 DampedSpringMotion(Vector3 currentPosition, Vector3 targetPosition, ref Vector3 velocity, float dampingRatio, float naturalFrequency, float deltaTime)
        {
            float omega = naturalFrequency * 2 * Mathf.PI;
            float zeta = dampingRatio;

            float f = 1.0f + 2.0f * deltaTime * zeta * omega;
            float oo = omega * omega;
            float hoo = deltaTime * oo;
            float hhoo = deltaTime * hoo;
            float detInv = 1.0f / (f + hhoo);

            Vector3 det = f * currentPosition + deltaTime * velocity + hhoo * targetPosition;
            Vector3 detV = velocity + hoo * (targetPosition - currentPosition);

            Vector3 newPosition = det * detInv;
            velocity = detV * detInv;

            return newPosition;
        }
    }
}
