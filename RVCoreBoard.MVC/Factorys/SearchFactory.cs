using RVCoreBoard.MVC.Services;
using System;
using RVCoreBoard.MVC.Attributes;

namespace RVCoreBoard.MVC.Factorys
{
    public class SearchFactory
    {
        public static BoardSearchBase GetSearchBoardList(int id, RVCoreBoard.MVC.Models.BoardListInfoModel.SearchType searchType, IBoardService boardService)
        {
            Type baseType = searchType.GetEnumAttributeValue<Type>(0);
            BoardSearchBase searchBase = (BoardSearchBase)Activator.CreateInstance(baseType, boardService);
            return searchBase;
        }

        public static UserSearchBase GetSearchUserList(RVCoreBoard.MVC.Models.UserListInfoModel.SearchType searchType, IUserService userService)
        {
            Type baseType = searchType.GetEnumAttributeValue<Type>(0);
            UserSearchBase searchBase = (UserSearchBase)Activator.CreateInstance(baseType, userService);
            return searchBase;
        }

        //public static SearchBase GetSearchBoardList(int id, SearchType searchType, string searchString, IBoardService boardService)
        //{
        //    List<Board> Data = new List<Board>();

        //    switch (searchType)
        //    {
        //        case SearchType.All:
        //            All all = new All(searchString);
        //            Data = await boardService.GetBoardList(all.Predicate);
        //            break;
        //        case SearchType.Title:
        //            Title title = new Title(searchString);
        //            Data = await boardService.GetBoardList(title.Predicate);
        //            break;
        //        case SearchType.Writer:
        //            Writer writer = new Writer(searchString);
        //            Data = await boardService.GetBoardList(writer.Predicate);
        //            break;
        //        case SearchType.Content:
        //            Content content = new Content(searchString);
        //            Data = await boardService.GetBoardList(content.Predicate);
        //            break;
        //        case SearchType.Comment:
        //            Comment comment = new Comment(searchString);
        //            Data = await boardService.GetBoardList(comment.Predicate);
        //            break;
        //        case SearchType.FileName:
        //            FileName fileName = new FileName(searchString);
        //            Data = await boardService.GetBoardList(fileName.Predicate);
        //            break;
        //    }
        //    Data = Data.Where(d => d.category.Id.Equals(id)).ToList();
        //    return Data;
        //}
    }
}
