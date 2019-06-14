using System;

namespace OCSS.Pager {

   public class PageDef {

      public static readonly int DefaultPageSize = 20;

      public int PageNum { get; set; }
      public int NumItemsPerPage { get; set; }

      public int SkipCount { get { return (PageNum - 1) * NumItemsPerPage; } }

      public PageDef(int pgNum) : this(pgNum, DefaultPageSize) { }
      public PageDef(int pgNum, int numItemsPerPage) {
         if (pgNum <= 0)
            throw new ArgumentException("Page number must be one or more.");
         if (numItemsPerPage <= 0)
            throw new ArgumentException("Number of items per page must be one or more.");
         this.PageNum = pgNum;
         this.NumItemsPerPage = numItemsPerPage;
      }

      public int CalcTotPages(int totCnt) {
         if (totCnt < 0)
            throw new ArgumentException("Item count must be zero or more.");
         if (totCnt == 0)
            return 1;
         var numPages = totCnt / NumItemsPerPage;
         if (NumItemsPerPage > 1) {
            if ((totCnt % NumItemsPerPage) > 0) // any overflow?
               numPages++;
         }
         return numPages;
      }
   }

}
