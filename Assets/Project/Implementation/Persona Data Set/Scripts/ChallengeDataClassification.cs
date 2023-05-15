using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    [CreateAssetMenu(fileName = "New Challenge Classification", menuName = "Challenge Generator/New Challenge Classification", order = 0)]
    public class ChallengeDataClassification : ScriptableObject
    {
        [Serializable]
        public struct Road
        {
            public float MinDistance;
            public float MaxDistance;
            public float Size;
            public float Turns;
            [Range(0,1)]
            public float Rate;
        }

        [Serializable]
        public struct Intersections
        {
            public int Count;
            [Range(0, 1)]
            public float Rate;
        }

        [Serializable]
        public struct IntersectionTypeActive
        {
            [ListDrawerSettings(Expanded = true)]
            public List<Intersection.StopType> Types;
            [Range(0, 1)]
            public float ErrorRate;
            [Range(0, 1)]
            public float Ratio;
        }

        [Serializable]
        public struct SpeedLimiter
        {
            public float Count;
            public float Rate;
        }

        [Header("Properties")]
        [ListDrawerSettings(Expanded = true)]
        public List<Road> RoadLengthClassification;
        [ListDrawerSettings(Expanded = true)]
        public List<Intersections> IntersectionClassification;
        [ListDrawerSettings(Expanded = true)]
        public List<IntersectionTypeActive> IntersectionTypeClassification;
        [ListDrawerSettings(Expanded = true)] 
        public List<SpeedLimiter> SpeedLimiterClassification;

        #region Method

        public Road GetRoadClassificationByRate(float ratio)
        {
            for (int i = 0; i < RoadLengthClassification.Count; i++)
            {
                if (i == RoadLengthClassification.Count - 1)
                    return RoadLengthClassification[i];

                if (ratio < RoadLengthClassification[i].Rate)
                {
                    return RoadLengthClassification[i - 1];
                }
            }

            return default;
        }

        public Intersections GetIntersectionsClassificationByRate(float ratio)
        {
            for (int i = 0; i < IntersectionClassification.Count; i++)
            {
                if (i == IntersectionClassification.Count - 1)
                    return IntersectionClassification[i];

                if (ratio < IntersectionClassification[i].Rate)
                {
                    return IntersectionClassification[i - 1];
                }
            }

            return default;
        }

        public IntersectionTypeActive GetIntersectionsTypeClassificationByRate(float errorRate, float ratio)
        {
            if (IntersectionTypeClassification.Count == 0)
                return default;

            if (IntersectionTypeClassification.Count < 2) 
                return IntersectionTypeClassification[0];

            if (errorRate < IntersectionTypeClassification[1].ErrorRate)
                return IntersectionTypeClassification[0];

            List<IntersectionTypeActive> interTypeByErrorRate = IntersectionTypeClassification.FindAll(x => errorRate >= x.ErrorRate);

            for (int i = 0; i < IntersectionTypeClassification.Count; i++)
            {
                if (i == IntersectionTypeClassification.Count - 1)
                    return IntersectionTypeClassification[i];

                if (ratio < IntersectionTypeClassification[i].Ratio)
                {
                    return IntersectionTypeClassification[i - 1];
                }
            }

            return default;
        }

        public SpeedLimiter GetSpeedLimiterClassificationByRate(float ratio)
        {
            for (int i = 0; i < SpeedLimiterClassification.Count; i++)
            {
                if (i == SpeedLimiterClassification.Count - 1)
                    return SpeedLimiterClassification[i];

                if (ratio < SpeedLimiterClassification[i].Rate)
                {
                    return SpeedLimiterClassification[i - 1];
                }
            }

            return default;
        }

        #endregion
    }
}
