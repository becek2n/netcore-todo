using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TODO.DTO
{
    public class ResultModel<T>
    {
        public ResultModel()
        {

        }
        public ResultModel(bool IsSuccess)
        {
            this.ResponseCode = (IsSuccess) ? "200" : "400";
            this.ResponseMessage = (IsSuccess) ? "success" : "failed";
        }

        public ResultModel<T> SetSuccess(string message, T value = default(T))
        {
            this.ResponseCode = "200";
            this.ResponseMessage = message != null ? message : "success";
            this.ResponseData = value;
            return this;
        }

        public ResultModel<T> SetFailed(string message, string statusCode = "400", T value = default(T), Exception Ex = null)
        {
            this.ResponseCode = "400";
            this.ResponseMessage = message != null ? message.Replace("'", "") : "failed";
            this.ResponseData = value;
            return this;
        }

        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public T ResponseData { get; set; }
    }

    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {

            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }

    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }

    public static class GetPaged
    {
        public static PagedResult<T> GetPage<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}

