using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine;

public struct Polygon
{
    public List<Vector2> model; // Untransformed model of polygon
    public List<Vector2> points; // Transformed polygon points
    public Vector2 origin; // Position
    public float angle; // Angle of rotation
    public bool overlap; // Collision
    public bool dirty; // Has changed, flag for updating
}

public static class ProjectionCollision
{
    private static List<Polygon> polygons = new List<Polygon>();

    public static void AddPolygon(Polygon p) => polygons.Add(p);

    public static bool Intersect(Polygon p1, Polygon p2)
    {
        Polygon polygon1 = p1;
        Polygon polygon2 = p2;

        for (int shape = 0; shape < 2; shape++)
        {
            // Swap to check each shape on each other
            if (shape == 1)
            {
                polygon1 = p2;
                polygon2 = p1;
            }

            // For each edge
            for (int i = 0; i < polygon1.points.Count; i++)
            {
                int j = (i + 1) % polygon1.points.Count;

                // Creates Normal
                Vector2 axisProj = new Vector2(-(polygon1.points[j].Y - polygon1.points[i].Y), polygon1.points[j].X - polygon1.points[i].X);

                // Sets min and max for new 1D axis
                float min_r1 = float.PositiveInfinity;
                float max_r1 = float.NegativeInfinity;

                for (int points = 0; points < polygon1.points.Count; points++)
                {
                    float dot = (polygon1.points[points].X * axisProj.X + polygon1.points[points].Y * axisProj.Y);

                    min_r1 = Math.Min(min_r1, dot);
                    max_r1 = Math.Max(max_r1, dot);
                }

                // Sets min and max for new 1D axis along shape two
                float min_r2 = float.PositiveInfinity;
                float max_r2 = float.NegativeInfinity;

                for (int points = 0; points < polygon2.points.Count; points++)
                {
                    float dot = (polygon2.points[points].X * axisProj.X + polygon2.points[points].Y * axisProj.Y);

                    min_r2 = Math.Min(min_r2, dot);
                    max_r2 = Math.Max(max_r2, dot);
                }

                if (!(max_r2 >= min_r1 && max_r1 >= min_r2))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static void UpdatePolygons()
    {
        // for each polygon
        for (int i = 0; i < polygons.Count; i++)
        {
            Polygon polygon = polygons[i];
            bool dirty = polygon.dirty;
            polygon.overlap = false;
            polygon.dirty = false;
            polygons[i] = polygon;

            // If the polygon hasn't been updated, we skip it
            if (!dirty) return;

            // For each point
            for (int j = 0; j < polygons[i].points.Count; j++)
            {
                Vector2 point = polygons[i].model[j];
                float angle = polygons[i].angle;
                Vector2 origin = polygons[i].origin;

                // Rotates points via 'angle'
                polygons[i].points[j] =
                    new Vector2(
                        ((point.X * (float)Math.Cos(angle)) - (point.Y * (float)Math.Sin(angle)) + origin.X),
                        ((point.X * (float)Math.Sin(angle)) + (point.Y * (float)Math.Cos(angle)) + origin.Y));
            }
        }
    }
}
