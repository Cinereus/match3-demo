using System.Collections.Generic;
using Code.Runtime.Logic.Match3;

namespace Code.Runtime.Infrastructure.Services
{
    public interface IMatchSearchService
    {
        void FindMatches(Match3GridState grid, List<Match> matches);
    }
}