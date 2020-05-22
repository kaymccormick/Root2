namespace AnalysisControls
{
    public interface ILineDrawer
    {
        void PrepareDrawLines(LineContext lineContext, bool clear);
        void PrepareDrawLine(LineContext lineContext);
        void DrawLine(LineContext lineContext);

        void EndDrawLines(LineContext lineContext);
    }
}