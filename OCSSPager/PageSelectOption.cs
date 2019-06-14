using System;
using System.Collections.Generic;

namespace OCSS.Pager {

   public class PageSelectOption {
      public string OptText { get; set; }
      public string PageLinkText { get; set; }
      public int PageNum { get; set; }
      public bool IsCurrent {  get; set; }

      public PageSelectOption(): this(string.Empty, string.Empty, 1, false) { }
      public PageSelectOption(string optTxt, string pageLinkText, int pgNum, bool isCurrent = false) {
         this.OptText = optTxt;
         this.PageLinkText = pageLinkText;
         this.PageNum = pgNum;
         this.IsCurrent = isCurrent;
      }

      /// <summary>
      /// Builds a generic list of Page selection options based on current page, total number of pages,
      /// and pageBloom, which is the number of pages on each side of current page to generate.
      /// For example, if you are on page 3 with a bloom of 2, 5 page options will be generated like such: 1 2 current 4 5
      /// </summary>
      /// <param name="stats">the current stats (page number and total pages)</param>
      /// <param name="pageBloom">the number of pages to buffer on each side of the current page. (must be 1 or more)</param>
      /// <returns>IEnumerable<PageSelectOption> of generic page selection options</returns>
      public static IEnumerable<PageSelectOption> BuildOpts(PageResultStats stats, int pageBloom, Func<int, string> pageLinkBuilder) {
         string link;
         if (pageBloom < 0)   // handle negative bloom as no bloom
            pageBloom = 0;

         // add first page entry, but only if first page isn't current or in pre-bloom
         if (stats.CurrPageNumber > 1 && stats.CurrPageNumber - pageBloom > 1) {
            link = (pageLinkBuilder == null) ? string.Empty : pageLinkBuilder(1);
            yield return new PageSelectOption(1.ToString(),  link, 1, false);
         }
         // add pre-bloom options (pages prior to current page)
         for (int z = stats.CurrPageNumber - pageBloom; z < stats.CurrPageNumber; z++) {
            if (z >= 1) {   // pages must be 1 or more
               link = (pageLinkBuilder == null) ? string.Empty : pageLinkBuilder(z);
               yield return new PageSelectOption(z.ToString(), link, z, false);
            }
         }
         // add current page
         link = (pageLinkBuilder == null) ? string.Empty : pageLinkBuilder(stats.CurrPageNumber);
         yield return new PageSelectOption(stats.CurrPageNumber.ToString(), link, stats.CurrPageNumber, true);
         // process post bloom options (pages after current page)
         for (int z = stats.CurrPageNumber + 1; z <= stats.CurrPageNumber + pageBloom; z++) {
            if (z <= stats.NumPages) {   // pages must in a valid range
               link = (pageLinkBuilder == null) ? string.Empty : pageLinkBuilder(z);
               yield return new PageSelectOption(z.ToString(), link, z, false);
            }
         }
         // add last page, but only if last page isn't current or in post-bloom
         if (stats.NumPages > stats.CurrPageNumber && stats.NumPages > stats.CurrPageNumber + pageBloom) {
            link = (pageLinkBuilder == null) ? string.Empty : pageLinkBuilder(stats.NumPages);
            yield return new PageSelectOption(stats.NumPages.ToString(), link, stats.NumPages, false);
         }
      }

   }

}
