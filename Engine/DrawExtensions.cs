using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

/// <summary>
/// Primative shape drawing
/// </summary>
public static class DrawExtensions {
    private static Texture2D _pixel;
    private static void CreatePixel(SpriteBatch sb) {
        _pixel = new Texture2D(sb.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        _pixel.SetData(new[] { Color.White });
    }

    #region // Line

    /// <summary>
    /// Draws a singular line between 2 points
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="point1">Vector2 point to start line drawing</param>
    /// <param name="point2">Vector2 point to end line drawing</param>
    /// <param name="thickness">Line thickness (px)</param>
    /// <param name="color">Color of line to draw</param>
    public static void DrawLine(
        this SpriteBatch sb,
        Vector2 point1,
        Vector2 point2,
        float thickness,
        Color color
    ) {
        float distance = Vector2.Distance(point1, point2);
        float angle = MathF.Atan2(point2.Y - point1.Y, point2.X - point1.X);

        DrawLine(sb, point1, distance, angle, thickness, color);
    }

    /// <summary>
    /// Draws a singular line between 2 points, centered so
    /// rotation doesn't surround top-left pixel position
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="point1">Vector2 point to start line drawing</param>
    /// <param name="point2">Vector2 point to end line drawing</param>
    /// <param name="thickness">Line thickness (px)</param>
    /// <param name="color">Color of line to draw</param>
    public static void DrawLineCentered(
        this SpriteBatch sb,
        Vector2 point1,
        Vector2 point2,
        float thickness,
        Color color
    ) {
        // make angle that faces perpendicular to line between two points
        float angle = MathF.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        angle -= MathF.PI / 2;

        // offset is halfway across thickness in perpendicular direction
        Vector2 offset = new(
            MathF.Cos(angle) * thickness / 2,
            MathF.Sin(angle) * thickness / 2
        );

        // draw with point offset applied
        DrawLine(sb, point1 + offset, point2 + offset, thickness, color);
    }

    /// <summary>
    /// Draws a singular line from starting point and angle
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="point">Vector2 point to start line drawing</param>
    /// <param name="length">Length of line drawn starting at point</param>
    /// <param name="angle">Angle of line to draw (0 facing right)</param>
    /// <param name="thickness">Thickness of line</param>
    /// <param name="color">Color of line</param>
    public static void DrawLine(
        this SpriteBatch sb,
        Vector2 point,
        float length,
        float angle,
        float thickness,
        Color color
    ) {
        if (_pixel == null) {
            CreatePixel(sb);
        }

        // stretch pixel across by length and thickness
        sb.Draw(
            _pixel,
            point,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(length, thickness),
            SpriteEffects.None,
            0
        );
    }

    #endregion

    #region // Rect

    /// <summary>
    /// Draws a rectangle, outline border, not fill
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="rect">Rectangle itself that is drawn</param>
    /// <param name="thickness">Thickness of rectangle's lines</param>
    /// <param name="color">Color of rectangle</param>
    public static void DrawRectOutline(this SpriteBatch sb, Rectangle rect, float thickness, Color color) {
        // top line
        DrawLine(sb,
            new Vector2(rect.X, rect.Y),
            new Vector2(rect.X + rect.Width, rect.Y),
            thickness,
            color
        );

        // right line
        DrawLine(sb,
            new Vector2(rect.X + rect.Width, rect.Y),
            new Vector2(rect.X + rect.Width, rect.Y + rect.Height),
            thickness,
            color
        );

        // bottom line
        DrawLine(sb,
            new Vector2(rect.X + rect.Width, rect.Y + rect.Height),
            new Vector2(rect.X, rect.Y + rect.Height),
            thickness,
            color
        );

        // left line
        DrawLine(sb,
            new Vector2(rect.X, rect.Y + rect.Height),
            new Vector2(rect.X, rect.Y),
            thickness,
            color
        );
    }

    /// <summary>
    /// Draws a filled a rectangle
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="rect">Rectangle itself that is drawn</param>
    /// <param name="color">Color of rectangle</param>
    public static void DrawRectFill(this SpriteBatch sb, Rectangle rect, Color color) {
        if (_pixel == null) {
            CreatePixel(sb);
        }

        sb.Draw(_pixel, rect, color);
    }

    #endregion

    #region // Circle drawing

    /// <summary>
    /// Draws a circle to the screen
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="centerPoint">Center of circle</param>
    /// <param name="radius">Radius of circle</param>
    /// <param name="divisions">
    /// Number of subdivisions of circle, higher value means
    /// higher number of sides (and more circle-y circle)
    /// </param>
    /// <param name="thickness">Thickness of circle outline line</param>
    /// <param name="color">Color of circle</param>
    public static void DrawCircleOutline(
        this SpriteBatch sb,
        Vector2 centerPoint,
        float radius,
        int divisions,
        float thickness,
        Color color
    ) {
        // set up variables
        float angleStep = 2 * MathF.PI / divisions;
        float angle = 0;
        Vector2 thisPos = new Vector2(
            MathF.Cos(angle) * radius,
            MathF.Sin(angle) * radius
        ) + centerPoint;

        // iterate for number of divisions, rotate around
        //   360 degrees and draw line at each iteration
        for (int i = 0; i < divisions; i++) {
            // increase angle
            angle += angleStep;

            // set new vector positions based on angle
            Vector2 prevPos = thisPos;
            thisPos = new Vector2(
                MathF.Cos(angle) * radius,
                MathF.Sin(angle) * radius
            ) + centerPoint;

            // draw line with these positions
            DrawLine(sb, prevPos, thisPos, thickness, color);
        }
    }

    /// <summary>
    /// Draws a circle to the screen
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="centerX">X coordinate of circle center</param>
    /// <param name="centerY">Y coordinate of circle center</param>
    /// <param name="radius">Radius of circle</param>
    /// <param name="divisions">
    /// Number of subdivisions of circle, higher value means
    /// higher number of sides (and more circle-y circle)
    /// </param>
    /// <param name="thickness">Thickness of circle outline line</param>
    /// <param name="color">Color of circle</param>
    public static void DrawCircleOutline(
        this SpriteBatch sb,
        int centerX,
        int centerY,
        float radius,
        int divisions,
        float thickness,
        Color color
    ) {
        DrawCircleOutline(
            sb,
            new Vector2(centerX, centerY),
            radius,
            divisions,
            thickness,
            color
        );
    }

    /// <summary>
    /// Draws a filled circle to the screen
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="centerPoint">Center of circle</param>
    /// <param name="radius">Radius of circle</param>
    /// <param name="divisions">
    /// Number of subdivisions of circle, higher value means
    /// higher number of sides (and more circle-y circle)
    /// </param>
    /// <param name="color">Color of circle</param>
    public static void DrawCircleFill(
        this SpriteBatch sb,
        Vector2 centerPoint,
        float radius,
        int divisions,
        Color color
    ) {
        // set up variables
        float angleStep = 2 * MathF.PI / divisions;
        float angle = 0;
        Vector2 edgePos = new Vector2(
            MathF.Cos(angle) * radius,
            MathF.Sin(angle) * radius
        ) + centerPoint;

        // thickness equals the distance between two edge points
        float thickness = Vector2.Distance(
            edgePos,
            new Vector2(
                MathF.Cos(angle + angleStep) * radius,
                MathF.Sin(angle + angleStep) * radius
            ) + centerPoint
        );

        // iterate for number of divisions, rotate around
        //   360 degrees and draw line between center point
        //   an edge point in the circle
        for (int i = 0; i < divisions; i++) {
            DrawLineCentered(sb, centerPoint, edgePos, thickness, color);

            angle += angleStep;

            edgePos = new Vector2(
                MathF.Cos(angle) * radius,
                MathF.Sin(angle) * radius
            ) + centerPoint;
        }
    }

    /// <summary>
    /// Draws a filled circle to the screen
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="centerX">X coordinate of circle center</param>
    /// <param name="centerY">Y coordinate of circle center</param>
    /// <param name="radius">Radius of circle</param>
    /// <param name="divisions">
    /// Number of subdivisions of circle, higher value means
    /// higher number of sides (and more circle-y circle)
    /// </param>
    /// <param name="color">Color of circle</param>
    public static void DrawCircleFill(
        this SpriteBatch sb,
        int centerX,
        int centerY,
        float radius,
        int divisions,
        Color color
    ) {
        DrawCircleFill(
            sb,
            new Vector2(centerX, centerY),
            radius,
            divisions,
            color
        );
    }

    #endregion

    #region // Triangle drawing

    /// <summary>
    /// Draws a triangle to the screen
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="point1">First triangle vertex</param>
    /// <param name="point2">Second triangle vertex</param>
    /// <param name="point3">Third triangle vertex</param>
    /// <param name="thickness">Thickness of circle outline line</param>
    /// <param name="color">Color of triangle</param>
    public static void DrawTriangleOutline(
        this SpriteBatch sb,
        Vector2 point1,
        Vector2 point2,
        Vector2 point3,
        float thickness,
        Color color
    ) {
        //DrawLineCentered(sb, point1, point2, thickness, color);
        //DrawLineCentered(sb, point2, point3, thickness, color);
        //DrawLineCentered(sb, point3, point1, thickness, color);

        // This should be expandable, and *should* work
        DrawShapeOutline(sb, thickness, color, point1, point2, point3);
    }

    #endregion

    #region // Shape Drawing

    /// <summary>
    /// Draws a shape to the screen
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    /// <param name="thickness">Thickness of circle outline line</param>
    /// <param name="color">Color of triangle</param>
    /// /// <param name="points">Array of points</param>
    public static void DrawShapeOutline(
        this SpriteBatch sb,
        float thickness,
        Color color,
        params Vector2[] points
    ) {
        for (int i = 0; i < points.Length; i++)
            DrawLineCentered(sb, points[i], points[(i + 1) % points.Length], thickness, color);
    }

    #endregion
}
