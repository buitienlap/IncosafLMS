using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using IncosafCMS.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace IncosafCMS.Web.Providers
{
    public static class GridViewCustomBindingHandlers
    {
        static IQueryable<ContractViewModel> Model { get { return LargeDatabaseDataProvider.Contracts.OrderByDescending(e => e.SignDate); } }

        public static void GetDataRowCountSimple(GridViewCustomBindingGetDataRowCountArgs e)
        {
            e.DataRowCount = Model.Count();
        }
        public static void GetDataSimple(GridViewCustomBindingGetDataArgs e)
        {
            e.Data = Model
                .ApplySorting(e.State.SortedColumns)
                .Skip(e.StartDataRowIndex)
                .Take(e.DataRowCount);
        }
       
        public static void GetDataRowCountAdvanced(GridViewCustomBindingGetDataRowCountArgs e)
        {
            int rowCount;
            if (GridViewCustomBindingSummaryCache.TryGetCount(e.FilterExpression, out rowCount))
                e.DataRowCount = rowCount;
            else
                e.DataRowCount = Model.ApplyFilter(e.FilterExpression).Count();
        }

        public static void GetUniqueHeaderFilterValuesAdvanced(GridViewCustomBindingGetUniqueHeaderFilterValuesArgs e)
        {
            e.Data = Model
                .ApplyFilter(e.FilterExpression)
                .UniqueValuesForField(e.FieldName);
        }

        public static void GetGroupingInfoAdvanced(GridViewCustomBindingGetGroupingInfoArgs e)
        {
            e.Data = Model
                .ApplyFilter(e.FilterExpression)
                .ApplyFilter(e.GroupInfoList)
                .GetGroupInfo(e.FieldName, e.SortOrder);
        }
        
        public static void GetDataAdvanced(GridViewCustomBindingGetDataArgs e)
        {
            //30.10.2025 thêm đoạn dưới đây để ẩn thanh tìm kiếm mặc định của grid
            //Và tạo ô tìm kiếm mới đật trên thanh công cụ - Mục đích: mở rộng cửa sổ DS H.Đong
            // 1️⃣ Lấy dữ liệu gốc
            var query = Model; // Model của bạn là IQueryable<ContractViewModel> hoặc tương tự

            // 2️⃣ Lọc theo text tìm kiếm (từ Session) theo Tên HĐ; Mã HĐ; KĐV; K.hàng
            string search = HttpContext.Current.Session["ContractSearchText"] as string;
            if (!string.IsNullOrEmpty(search))
            {
                // Chuyển search về chữ thường để tránh lỗi phân biệt hoa thường
                string keyword = search.Trim().ToLower();

                query = query.Where(c =>
                    (c.Name ?? "").ToLower().Contains(keyword) ||
                    (c.MaHD ?? "").ToLower().Contains(keyword) ||
                    (c.OwnerDisplayName ?? "").ToLower().Contains(keyword) ||
                    (c.CustomerName ?? "").ToLower().Contains(keyword)
                );
            }
            
            e.Data = query
                .ApplyFilter(e.FilterExpression)
                .ApplyFilter(e.GroupInfoList)
                .ApplySorting(e.State.SortedColumns)
                .Skip(e.StartDataRowIndex)
                .Take(e.DataRowCount);
            
        }

        private static IQueryable<ContractViewModel> ApplyContractFilterMode(IQueryable<ContractViewModel> query)
        {
            int mode = GridViewHelper.ContractGridFilterIndex;

            switch (mode)
            {
                case 0://HD Toan bo
                    
                    break;
                case 1://HD chua cap so
                    query = query.Where(x => string.IsNullOrEmpty(x.MaHD));
                    break;
                case 2://HD da cap so
                    query = query.Where(x => !string.IsNullOrEmpty(x.MaHD));
                    break;
                case 3://HD Da xuat hoa don
                    query = query.Where(x => x.XuatHoaDon > 0);
                    break;
                case 4:
                    query = query.Where(x => x.ThuNo > 0);
                    break;
                case 5:
                    query = query.Where(x => x.CongNo > 0);
                    break;
                case 6:
                    query = query.Where(x => x.DoDang > 0);
                    break;
            }

            return query;
        }
        
        public static void GetSummaryValuesAdvanced(GridViewCustomBindingGetSummaryValuesArgs e)
        {
            var query = Model
                .ApplyFilter(e.FilterExpression)
                .ApplyFilter(e.GroupInfoList);

            var summaryValues = query.CalculateSummary(e.SummaryItems);
            e.Data = summaryValues;

            var countSummaryItem = e.SummaryItems.FirstOrDefault(i => i.SummaryType == SummaryItemType.Count);
            if (e.GroupInfoList.Count == 0 && countSummaryItem != null)
            {
                var itemIndex = e.SummaryItems.IndexOf(countSummaryItem);
                var count = summaryValues[itemIndex] != null ? Convert.ToInt32(summaryValues[itemIndex]) : 0;
                GridViewCustomBindingSummaryCache.SaveCount(e.FilterExpression, count);
            }
        }

    }

    public class CriteriaValidator : EvaluatorCriteriaValidator
    {
        private bool isCriteriaOperatorValid = true;
        private CriteriaValidator() : base(null) { }
        public static bool IsCriteriaOperatorValid(CriteriaOperator criteria)
        {
            CriteriaValidator validator = new CriteriaValidator();
            validator.Validate(criteria);
            return validator.isCriteriaOperatorValid;
        }
        public override void Visit(OperandValue theOperand)
        {
            if (theOperand.Value == null)
                isCriteriaOperatorValid = false;
        }
        public override void Visit(JoinOperand theOperand) { }
        public override void Visit(OperandProperty theOperand) { }
        public override void Visit(AggregateOperand theOperand) { }
    }

    public static class GridViewCustomOperationDataHelper
    {
        static ICriteriaToExpressionConverter Converter { get { return new CriteriaToExpressionConverter(); } }

        public static IQueryable Select(this IQueryable query, string fieldName)
        {
            return query.MakeSelect(Converter, new OperandProperty(fieldName));
        }

        public static IQueryable ApplySorting(this IQueryable query, IEnumerable<GridViewColumnState> sortedColumns)
        {
            ServerModeOrderDescriptor[] orderDescriptors = sortedColumns
                .Select(c => new ServerModeOrderDescriptor(new OperandProperty(c.FieldName), c.SortOrder == ColumnSortOrder.Descending))
                .ToArray();
            return query.MakeOrderBy(Converter, orderDescriptors);
        }

        public static IQueryable ApplyFilter(this IQueryable query, IList<GridViewGroupInfo> groupInfoList)
        {
            var criteria = GroupOperator.And(
                groupInfoList.Select(i => new BinaryOperator(i.FieldName, i.KeyValue, BinaryOperatorType.Equal))
            );
            return query.ApplyFilter(CriteriaOperator.ToString(criteria));
        }
        public static IQueryable ApplyFilter(this IQueryable query, string filterExpression)
        {
            
            CriteriaOperator criteria = CriteriaOperator.Parse(filterExpression);
            if (CriteriaValidator.IsCriteriaOperatorValid(criteria))
                query = query.AppendWhere(Converter, criteria);
            return query;
        }

        public static IQueryable UniqueValuesForField(this IQueryable query, string fieldName)
        {
            query = query.Select(fieldName);
            var expression = Expression.Call(typeof(Queryable), "Distinct", new Type[] { query.ElementType }, query.Expression);
            return query.Provider.CreateQuery(expression);
        }

        public static IEnumerable<GridViewGroupInfo> GetGroupInfo(this IQueryable query, string fieldName, ColumnSortOrder order)
        {
            var rowType = query.ElementType;
            query = query.MakeGroupBy(Converter, new OperandProperty(fieldName));
            query = query.MakeOrderBy(Converter, new ServerModeOrderDescriptor(new OperandProperty("Key"), order == ColumnSortOrder.Descending));

            var param = Expression.Parameter(query.ElementType, string.Empty);
            var keyValues = query.ApplyExpression(Expression.Property(param, "Key"), param).ToArray();
            var dataRowCounts = query.ApplyExpression(Expression.Call(typeof(Enumerable), "Count", new Type[] { rowType }, param), param).ToArray();

            var list = new List<GridViewGroupInfo>();
            for (int i = 0; i < keyValues.Length; i++)
                list.Add(new GridViewGroupInfo() { KeyValue = keyValues[i], DataRowCount = (int)dataRowCounts[i] });
            return list;
        }

        public static object[] CalculateSummary(this IQueryable query, List<GridViewSummaryItemState> summaryItems)
        {
            var elementType = query.ElementType;
            query = query.MakeGroupBy(Converter, new OperandValue(0));
            var groupParam = Expression.Parameter(query.ElementType, string.Empty);

            var expressions = GetAggregateExpressions(elementType, summaryItems, groupParam);
            var groupValue = new List<object>();
            foreach (var expression in expressions)
            {
                var result = query.ApplyExpression(expression, groupParam).ToArray();
                if (result.Length > 0)
                    groupValue.Add(result[0]);
            }
            return groupValue.Count > 0 ? groupValue.ToArray() : new object[summaryItems.Count];
        }

        static List<Expression> GetAggregateExpressions(Type elementType, List<GridViewSummaryItemState> summaryItems, ParameterExpression groupParam)
        {
            var list = new List<Expression>();
            var elementParam = Expression.Parameter(elementType, "elem");
            foreach (var item in summaryItems)
            {
                Expression e;
                LambdaExpression elementExpr = null;
                if (!string.IsNullOrEmpty(item.FieldName))
                    elementExpr = Expression.Lambda(Converter.Convert(elementParam, new OperandProperty(item.FieldName)), elementParam);

                switch (item.SummaryType)
                {
                    case SummaryItemType.Count:
                        e = Expression.Call(typeof(Enumerable), "Count", new Type[] { elementType }, groupParam);
                        break;
                    case SummaryItemType.Sum:
                        e = Expression.Call(typeof(Enumerable), "Sum", new Type[] { elementType }, groupParam, elementExpr);
                        break;
                    case SummaryItemType.Min:
                        e = Expression.Call(typeof(Enumerable), "Min", new Type[] { elementType }, groupParam, elementExpr);
                        break;
                    case SummaryItemType.Max:
                        e = Expression.Call(typeof(Enumerable), "Max", new Type[] { elementType }, groupParam, elementExpr);
                        break;
                    case SummaryItemType.Average:
                        e = Expression.Call(typeof(Enumerable), "Average", new Type[] { elementType }, groupParam, elementExpr);
                        break;
                    default:
                        throw new NotSupportedException(item.SummaryType.ToString());
                }
                list.Add(e);
            }
            return list;
        }

        static IQueryable ApplyExpression(this IQueryable query, Expression expression, ParameterExpression param)
        {
            var lambda = Expression.Lambda(expression, param);
            var callExpr = Expression.Call(typeof(Queryable), "Select", new Type[] { query.ElementType, lambda.Body.Type }, query.Expression, Expression.Quote(lambda));
            return query.Provider.CreateQuery(callExpr);
        }

        static object[] ToArray(this IQueryable query)
        {
            var list = new ArrayList();
            foreach (var item in query)
                list.Add(item);
            return list.ToArray();
        }
    }

    public static class GridViewCustomBindingSummaryCache
    {
        const string CacheKey = "B08E5DF5-4D10-45C7-B4F1-C95EB2FE69C8";
        static HttpContext Context { get { return HttpContext.Current; } }
        static Dictionary<string, int> Cache
        {
            get
            {
                if (Context.Items[CacheKey] == null)
                    Context.Items[CacheKey] = new Dictionary<string, int>();
                return (Dictionary<string, int>)Context.Items[CacheKey];
            }
        }
        public static bool TryGetCount(string key, out int count)
        {
            count = 0;
            if (!Cache.ContainsKey(key))
                return false;
            count = Cache[key];
            return true;
        }
        public static void SaveCount(string key, int count)
        {
            Cache[key] = count;
        }
    }
}