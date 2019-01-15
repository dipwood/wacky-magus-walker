using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Crypt.Entities
{
    public class Polygon
    {
        private List<Vector2> _points = new List<Vector2>();
        private List<Vector2> _edges = new List<Vector2>();

        public Polygon(IEnumerable<Vector2> points)
        {
            _points.AddRange(points);
            BuildEdges();
        }

        private void BuildEdges()
        {
            _edges.Clear();
            for (int i = 0; i < _points.Count; i++)
            {
                Vector2 p1 = _points[i];
                Vector2 p2;
                if (i + 1 >= _points.Count)
                {
                    p2 = _points[0];
                }
                else
                {
                    p2 = _points[i + 1];
                }
                _edges.Add(p2 - p1);
            }
        }

        public List<Vector2> Edges
        {
            get { return _edges; }
        }

        public List<Vector2> Points
        {
            get { return _points; }
        }

        public Vector2 Center
        {
            get
            {
                float totalX = 0;
                float totalY = 0;
                for (int i = 0; i < _points.Count; i++)
                {
                    totalX += _points[i].X;
                    totalY += _points[i].Y;
                }

                return new Vector2(totalX / _points.Count, totalY / _points.Count);
            }
        }

        public void Offset(Vector2 v)
        {
            Offset(v.X, v.Y);
        }

        public void Offset(float x, float y)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                Vector2 p = _points[i];
                _points[i] = new Vector2(p.X + x, p.Y + y);
            }
        }
    }

    public static class Intersector
    {
        public static PolygonCollisionResult Intersect(Polygon polygon, Polygon polygon2)
        {
            return PolygonCollision(polygon, polygon2);
        }

        private static void ProjectPolygon(Vector2 axis, Polygon polygon, ref float min, ref float max)
        {
            // To project a point on an axis use the dot product
            float dotProduct = Vector2.Dot(axis, polygon.Points[0]);
            min = dotProduct;
            max = dotProduct;
            foreach (Vector2 t in polygon.Points)
            {
                dotProduct = Vector2.Dot(t, axis);
                if (dotProduct < min)
                {
                    min = dotProduct;
                }
                else
                {
                    if (dotProduct > max)
                    {
                        max = dotProduct;
                    }
                }
            }
        }

        // Calculate the distance between [minA, maxA] and [minB, maxB]
        // The distance will be negative if the intervals overlap
        private static float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }

        // Check if polygon A is going to collide with polygon B.
        // The last parameter is the *relative* velocity
        // of the polygons (i.e. velocityA - velocityB)
        private static PolygonCollisionResult PolygonCollision(Polygon polygonA, Polygon polygonB)
        {
            PolygonCollisionResult result = new PolygonCollisionResult
            {
                Intersect = true
            };

            int edgeCountA = polygonA.Edges.Count;
            int edgeCountB = polygonB.Edges.Count;
            float minIntervalDistance = float.PositiveInfinity;
            Vector2 translationAxis = new Vector2();
            Vector2 edge;

            // Loop through all the edges of both polygons

            for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                if (edgeIndex < edgeCountA)
                {
                    edge = polygonA.Edges[edgeIndex];
                }
                else
                {
                    edge = polygonB.Edges[edgeIndex - edgeCountA];
                }

                // ===== 1. Find if the polygons are currently intersecting =====
                // Find the axis perpendicular to the current edge
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                // Find the projection of the polygon on the current axis

                float minA = 0;
                float minB = 0;
                float maxA = 0;
                float maxB = 0;
                ProjectPolygon(axis, polygonA, ref minA, ref maxA);
                ProjectPolygon(axis, polygonB, ref minB, ref maxB);

                // Check if the polygon projections are currentlty intersecting

                if (IntervalDistance(minA, maxA, minB, maxB) > 0)
                    result.Intersect = false;

                // ===== 2. Now find if the polygons *will* intersect =====


                // Project the velocity on the current axis

                //float velocityProjection = Vector2.Dot(axis, velocity);

                // Get the projection of polygon A during the movement

                // Do the same test as above for the new projection

                float intervalDistance = IntervalDistance(minA, maxA, minB, maxB);

                // If the polygons are not intersecting and won't intersect, exit the loop

                if (!result.Intersect) break;

                // Check if the current interval distance is the minimum one. If so store

                // the interval distance and the current distance.

                // This will be used to calculate the minimum translation vector

                intervalDistance = Math.Abs(intervalDistance);
                if (intervalDistance < minIntervalDistance)
                {
                    minIntervalDistance = intervalDistance;
                    translationAxis = axis;

                    Vector2 d = polygonA.Center - polygonB.Center;
                    if (Vector2.Dot(d, translationAxis) < 0)
                        translationAxis = -translationAxis;
                }
            }

            // The minimum translation vector
            // can be used to push the polygons appart.

            result.MinimumTranslationVector = translationAxis * minIntervalDistance;

            return result;
        }
    }

    public class PolygonCollisionResult
    {
        public bool Intersect;
        public Vector2 MinimumTranslationVector;
    }
}