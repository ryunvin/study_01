using RVCoreBoard.MVC.Models;
using System;
using System.Collections.Generic;
using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    abstract class  Search
    {
        public System.Linq.Expressions.Expression<Func<Board, bool>> predicate { get; set; }
    }
}
