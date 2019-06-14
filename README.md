## OCSSPager
Simple Pager Component without UI dependencies
This can easily be used in WinForms, Asp.net, etc.

### Usage

1) Include OCSS.Pager

2) Setup a `PageDef` structure to hold your paging setup such as the current page number and number of entries per page.
```
PageDef pagingSetup = new PageDef(pgNum, 20); // 20 items per page
```
If this is a ASP.Net web page, you might retrieve pgNum from the URL, for example.
PageDef also has a handy property called `SkipCount` that you can use if you are reading data from a database.
SkipCount is based on the `PageNum` property which is a counter (starting at 1). 
If PageNum equals 2 (2nd page) and 20 items per page, SkipCount would return 20.

3) Call the static function `PageSelectOption.BuildOpts(PageResultStats stats, int pageBloom, Func<int, string> pageLinkBuilder)` passing 
in a `PageResultStats` instance, a page bloom count (explained later), and a function to format the page number string for the UI.

BuildOpts returns an `IEnumerable<PageSelectOption>` containing the page options which your UI can use.

### What is Page Bloom?
Page bloom is the page number "expansion" for pages on each side of the current page number.
For example, if you are on page 3 with a bloom of 2, 5 page options will be generated: 1 2 current 4 5.
Bloom can be zero, in which case, no extra page options will be generated outside the built-in logic below.

### Page Display Logic
- Add first page (unless first page is current)
- Add current page
- Add last page (unless last page is current)
- If bloom is > 0, add in pre and post bloom pages (skipping duplicate pages from other rules)

### Sample Razor Page code (abbreviated)
```
public class DisplayModel: PageModel {
      public List<PageSelectOption> PageSelectOptions { get; private set; }
      ...
      ...
      public async Task<IActionResult> OnGetAsync(int? pgnum) {
         if (pgnum == null)
            pgnum = 1;
         // initialize a `PageDef` instance
         PageDef pagingSetup = new PageDef((int) pgnum, 20);  // 20 items per page
         // your code to read entries from database here
         ...
         // your code to build `PageOfResults<T>` from your results.
         // Example: var entries = new PageOfResults<LedgerEntry>(ledgerResultList, pagingSetup.PageNum, pageDef.CalcTotPages(totRecordCount));
         ...
         PageSelectOptions = PageSelectOption.BuildOpts(entries.Stats, 3, BuildLedgerLink).ToList();
         return Page();
      }

      // our private function called by PageSelectOption.BuildOpts() to build page links
      private string BuildLedgerLink(int pageNum) {
         return Url.Page("LedgerDisplay", "Get", new { pgnum = pageNum });
      }
```

### Sample Razor shared page using Bootstrap 4 paging UI components
```
@using OCSS.Pager
@model IEnumerable<PageSelectOption>

<div>
   <nav aria-label="Page-number selection">
      <ul class="pagination pagination-lg">
         @foreach (var pageItem in Model) {
            if (pageItem.IsCurrent) {
               <li class="page-item active">
                  <a class="page-link" href="#">@pageItem.OptText <span class="sr-only">(current)</span></a>
               </li>
            }
            else {
               <li class="page-item"><a class="page-link" href="@pageItem.PageLinkText">@pageItem.OptText</a></li>
            }
         }
      </ul>
   </nav>
</div>
```
