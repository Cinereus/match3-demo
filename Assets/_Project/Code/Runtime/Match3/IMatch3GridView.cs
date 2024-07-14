using System.Collections.Generic;
using Code.Match3;
using Cysharp.Threading.Tasks;

namespace Code.Runtime.Match3
{
    public interface IMatch3GridView
    {
        UniTask VisualizeSwap(ShapePos first, ShapePos second);
        UniTask VisualizeFalseSwap(ShapePos first, ShapePos second);
        UniTask VisualizeFalling(List<ShapeFallInfo> fallInfos);
        UniTask CreateNewShapeViews(List<ShapeCreateInfo> createInfos);
        UniTask VisualizeDestruction(Stack<ShapeBonusProcessInfo> destructionChain);
        UniTask VisualizeDestruction(List<ShapePos> destroyedShapes);
        UniTask VisualizeGridReconfiguration(Match3Grid<ShapeInfo> gridState);
        UniTask CreateMatchBonusShapeViews(List<ShapeBonusProcessInfo> processInfos);
        UniTask CreateColorBomb(ShapePos pos);
        void CreateStartState(Match3Grid<ShapeInfo> gridState);
        void VisualizeMatchHint(ShapeMatchPredictionInfo matchPrediction, float delay);
        void StopActiveTweens();
    }
}