using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Defines a line tracer for the template method pattern.
/// </summary>
public interface ILineTracer
{
    RaycastHit[] LineTrace(Vector3 origin, Vector3 direction, float farClipDistance, int layerMask, QueryTriggerInteraction queryTriggerInteractions);
}

/// <summary>
/// Defines a sphere tracer for the template method pattern.
/// </summary>
public interface ISphereTracer
{
    RaycastHit[] SphereTrace(Vector3 origin, float radius, Vector3 direction, float farClipDistance, int layerMask, QueryTriggerInteraction queryTriggerInteractions);
}

/// <summary>
/// Shoots a blocking raycast and returns the result.
/// If anything was hit, returns the hit. Else returns null.
/// </summary>
public struct BlockingLineTracer : ILineTracer
{
    public RaycastHit[] LineTrace(Vector3 origin, Vector3 direction, float farClipDistance, int layerMask, QueryTriggerInteraction queryTriggerInteractions)
    {
        if (Physics.Raycast(origin, direction, out var hit, farClipDistance, layerMask, queryTriggerInteractions))
        {
            RaycastHit[] result = new RaycastHit[1];
            result[0] = hit;
            return result;
        }

        return null;
    }
}

/// <summary>
/// Shoots a blocking sphere cast and returns the result. 
/// If anything was hit, returns the hit. Else returns null.
/// </summary>
public struct BlockingSphereTracer : ISphereTracer
{
    public RaycastHit[] SphereTrace(Vector3 origin, float radius, Vector3 direction, float farClipDistance, int layerMask, QueryTriggerInteraction queryTriggerInteractions)
    {
        if (Physics.SphereCast(origin, radius, direction, out var hit, farClipDistance, layerMask, queryTriggerInteractions))
        {
            RaycastHit[] result = new RaycastHit[1];
            result[0] = hit;
            return result;
        }

        return null;
    }
}

/// <summary>
/// Shoots a pass-through line trace and returns all that was hit.
/// If nothing was hit, returns null.
/// </summary>
public struct OverlappingLineTracer : ILineTracer
{
    public RaycastHit[] LineTrace(Vector3 origin, Vector3 direction, float farClipDistance, int layerMask, QueryTriggerInteraction queryTriggerInteractions)
    {
        RaycastHit[] result = Physics.RaycastAll(origin, direction, farClipDistance, layerMask, queryTriggerInteractions);

        if (result != null && result.Length > 0)
        {
            return result;
        }

        return null;
    }
}

/// <summary>
/// Shoots a pass-through sphere trace and returns all that was hit. 
/// If nothing was hit, returns null.
/// </summary>
public struct OverlappingSphereTracer : ISphereTracer
{
    public RaycastHit[] SphereTrace(Vector3 origin, float radius, Vector3 direction, float farClipDistance, int layerMask, QueryTriggerInteraction queryTriggerInteractions)
    {
        RaycastHit[] result = Physics.SphereCastAll(origin, radius, direction, farClipDistance, layerMask, queryTriggerInteractions);

        if (result != null && result.Length > 0)
        {
            return result;
        }

        return null;
    }
}

/// <summary>
/// Casts lines in a cone shape with optional extra sphere casts on top of them.
/// The spread and density of the cones can be set.
/// </summary>
public static class ConePhysics
{
    /// <summary>
    /// Cast line with optional spheres in a cone shape to find the nearest hit (no overlap, only direct hit). 
    /// </summary>
    /// <param name="hits">all successful hits or empty if none</param>
    /// <param name="origin">start point of the cone</param>
    /// <param name="direction">direction of the cone</param>
    /// <param name="coneAngle">angle, or spread, of the cone</param>
    /// <param name="subdivision">the density of the lines that make up the cone. increasing this means a square increase in line count so keep it as low as possible</param>
    /// <param name="nearClipDistance">near cutoff of the cone</param>
    /// <param name="farClipDistance">far cutoff of the cone</param>
    /// <param name="layerMask">affected layers</param>
    /// <param name="useExtraSpheres">should there be extra spheres shot on top of the lines? having this enabled can help reduce line count and as such improve performance</param>
    /// <param name="extraSphereRadius">if used, the radius of the extra spheres</param>
    /// <param name="queryTriggerInteraction">query trigger interaction for the lines</param>
    /// <param name="visualize">should debug lines be drawn?</param>
    /// <param name="drawColor">if debug lines are enabled, the color of the lines</param>
    /// <returns>if there was at least one successful hit</returns>
    public static bool ConeCast(
        out RaycastHit hit,
        Vector3 origin,
        Vector3 direction, 
        float coneAngle, 
        int subdivision, 
        float nearClipDistance = 0, 
        float farClipDistance = 1000, 
        int layerMask = 0,
        bool useExtraSpheres = false,
        float extraSphereRadius = 0.1f,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, 
        bool visualize = false,
        Color drawColor = new Color())
    {
        ILineTracer lineTracer = new BlockingLineTracer();
        ISphereTracer sphereTracer = new BlockingSphereTracer();
        List<RaycastHit> hits = new();
        hit = new RaycastHit();

        bool result = ConeCastCommon(
            lineTracer,
            sphereTracer,
            out hits, 
            origin, 
            direction, 
            coneAngle, 
            subdivision,
            nearClipDistance, 
            farClipDistance, 
            layerMask, 
            useExtraSpheres,
            extraSphereRadius,
            queryTriggerInteraction,
            visualize,
            drawColor);

        if (hits != null && hits.Count > 0)
        {
            hit = hits.OrderBy(hit => hit.distance).FirstOrDefault();
        }

        return result;
    }

    /// <summary>
    /// Cast line with optional spheres in a cone shape to find all hits with overlap.
    /// </summary>
    /// <param name="origin">start point of the cone</param>
    /// <param name="direction">direction of the cone</param>
    /// <param name="coneAngle">angle, or spread, of the cone</param>
    /// <param name="subdivision">the density of the lines that make up the cone. increasing this means a square increase in line count so keep it as low as possible</param>
    /// <param name="nearClipDistance">near cutoff of the cone</param>
    /// <param name="farClipDistance">far cutoff of the cone</param>
    /// <param name="layerMask">affected layers</param>
    /// <param name="useExtraSpheres">should there be extra spheres shot on top of the lines? having this enabled can help reduce line count and as such improve performance</param>
    /// <param name="extraSphereRadius">if used, the radius of the extra spheres</param>
    /// <param name="queryTriggerInteraction">query trigger interaction for the lines</param>
    /// <param name="visualize">should debug lines be drawn?</param>
    /// <param name="drawColor">if debug lines are enabled, the color of the lines</param>
    /// <returns>all hits that were found, or an empty array if none was found.</returns>
    public static RaycastHit[] ConeCastAll(
        Vector3 origin,
        Vector3 direction,
        float coneAngle,
        int subdivision,
        float nearClipDistance = 0,
        float farClipDistance = 1000,
        int layerMask = 0,
        bool useExtraSpheres = false,
        float extraSphereRadius = 0.1f,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
        bool visualize = false,
        Color drawColor = new Color())
    {
        ILineTracer lineTracer = new OverlappingLineTracer();
        ISphereTracer sphereTracer = new OverlappingSphereTracer();

        ConeCastCommon(
            lineTracer,
            sphereTracer,
            out var hits,
            origin,
            direction,
            coneAngle,
            subdivision,
            nearClipDistance,
            farClipDistance,
            layerMask,
            useExtraSpheres,
            extraSphereRadius,
            queryTriggerInteraction,
            visualize,
            drawColor);

        if (hits != null)
        {
            List<RaycastHit> results = new List<RaycastHit>();
            foreach (RaycastHit hit in hits)
            {
                bool entryAlreadyExists = false;
                foreach(RaycastHit result in results)
                {
                    if(result.transform.gameObject == hit.transform.gameObject)
                    {
                        entryAlreadyExists = true;
                    }
                }
                if (!entryAlreadyExists)
                {
                    results.Add(hit);
                }
            }
            return results.ToArray();
        }

        return new RaycastHit[0];
    }

    private static bool ConeCastCommon(
        ILineTracer lineTracer,
        ISphereTracer sphereTracer,
        out List<RaycastHit> hits,
        Vector3 origin,
        Vector3 direction,
        float coneAngle,
        int subdivision,
        float nearClipDistance,
        float farClipDistance,
        int layerMask,
        bool useExtraSpheres,
        float extraSphereRadius,
        QueryTriggerInteraction queryTriggerInteraction,
        bool visualize,
        Color drawColor)
    {
        bool result = false;
        hits = new List<RaycastHit>();

        if (direction == Vector3.zero || 
            coneAngle <= 0.0f || 
            subdivision < 1 || 
            nearClipDistance < 0.0f || 
            farClipDistance <= 0.0f || 
            extraSphereRadius <= 0.0f || 
            farClipDistance <= nearClipDistance ||
            coneAngle > 90.0f)
        {
            return result;
        }

        direction.Normalize();

        Vector3 translatedForward = origin + direction * nearClipDistance;
        float angle = Mathf.PI / (2 * subdivision);
        if (ShootLineForResult(lineTracer, sphereTracer, hits, origin, direction, farClipDistance, nearClipDistance, layerMask, useExtraSpheres, extraSphereRadius, queryTriggerInteraction, visualize, drawColor, result, direction, translatedForward))
        {
            result = true;
        }

        for (int i = 0; i < subdivision; i++)
        {
            float ring = ((float)(i + 1)) / subdivision;
            for (int j = 0; j < subdivision * 4; j++)
            {
                // calculate the individual directions that make up the cone
                Quaternion rotation = Quaternion.Euler(coneAngle * Mathf.Sin(angle * j) * ring, coneAngle * Mathf.Cos(angle * j) * ring, 0);
                Quaternion directionRotation = Quaternion.LookRotation(direction);
                Vector3 conePartDirection = directionRotation * rotation * Vector3.forward;

                // offset the start point of the current line in its direction over near clip distance
                Vector3 translatedOrigin = origin + conePartDirection * nearClipDistance;
                if (ShootLineForResult(lineTracer, sphereTracer, hits, origin, direction, farClipDistance, nearClipDistance, layerMask, useExtraSpheres, extraSphereRadius, queryTriggerInteraction, visualize, drawColor, result, conePartDirection, translatedOrigin))
                {
                    result = true;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Shoots a line and an optional sphere to find a target.
    /// </summary>
    /// <returns>if anything was hit</returns>
    private static bool ShootLineForResult(ILineTracer lineTracer, ISphereTracer sphereTracer, List<RaycastHit> hits, Vector3 origin, Vector3 direction, float farClipDistance, float nearClipDistance, int layerMask, bool useExtraSpheres, float extraSphereRadius, QueryTriggerInteraction queryTriggerInteraction, bool visualize, Color drawColor, bool result, Vector3 conePartDirection, Vector3 translatedOrigin)
    {
        RaycastHit[] lineResult = lineTracer.LineTrace(translatedOrigin, conePartDirection, farClipDistance - nearClipDistance, layerMask, queryTriggerInteraction);
        if (lineResult != null)
        {
            result = true;
            hits.AddRange(lineResult);
        }

        if (visualize)
        {
            Debug.DrawLine(translatedOrigin, origin + conePartDirection * farClipDistance, drawColor);
        }

        if (useExtraSpheres)
        {
            RaycastHit[] sphereResult = sphereTracer.SphereTrace(translatedOrigin, extraSphereRadius, conePartDirection, farClipDistance - nearClipDistance, layerMask, queryTriggerInteraction);
            if (sphereResult != null)
            {
                result = true;
                hits.AddRange(sphereResult);
            }

            if (visualize)
            {
                DrawDebugCircle(origin + conePartDirection * farClipDistance, direction, extraSphereRadius, drawColor, 12);
            }
        }

        return result;
    }

    /// <summary>
    /// Approximate a circle with lines, facing a given direction.
    /// </summary>
    private static void DrawDebugCircle(Vector3 center, Vector3 direction, float radius, Color color, int segments)
    {
        direction = direction.normalized;

        Vector3 axis1 = Vector3.Cross(direction, Vector3.up).normalized;
        if (axis1 == Vector3.zero) axis1 = Vector3.Cross(direction, Vector3.right).normalized;

        Vector3 axis2 = Vector3.Cross(direction, axis1).normalized;

        float angleStep = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle1 = angleStep * i * Mathf.Deg2Rad;
            float angle2 = angleStep * (i + 1) * Mathf.Deg2Rad;

            Vector3 point1 = center + (axis1 * Mathf.Cos(angle1) + axis2 * Mathf.Sin(angle1)) * radius;
            Vector3 point2 = center + (axis1 * Mathf.Cos(angle2) + axis2 * Mathf.Sin(angle2)) * radius;

            Debug.DrawLine(point1, point2, color);
        }
    }
}



