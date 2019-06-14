using System;
using System.Collections.Generic;

namespace OCSS.Pager {

   public class PageResultStats {
      public int CurrPageNumber { get; private set; }
      public int NumPages { get; private set; }

      public bool HasNextPage { get { return CurrPageNumber < NumPages; } }
      public bool HasPrevPage { get { return CurrPageNumber > 1; } }
      public bool HasMultiplePages { get { return NumPages > 1; } }

      public PageResultStats(int pageNum, int numPages) {
         if (pageNum <= 0)
            throw new ArgumentException("Page number must be one or more.");
         if (numPages <= 0)
            throw new ArgumentException("Number of pages must be one or more.");
         if (pageNum > numPages)
            throw new ArgumentException("Page number must be less than or equal to number of pages");
         this.CurrPageNumber = pageNum;
         this.NumPages = numPages;
      }

   }

   public class PageOfResults<T>: List<T> {
      public PageResultStats Stats { get; private set; }
      public int PageItemCount {  get { return (this == null) ? 0 : this.Count; } }

      public PageOfResults(List<T> items, int pageNum, int numPages) {
         if (items == null)
            items = new List<T>();
         this.AddRange(items);
         Stats = new PageResultStats(pageNum, numPages);
      }

   }

}
