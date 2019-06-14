using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCSS.Pager;

namespace OCSSPager.Tests {

#region PageDef Tests
   [TestClass]
   public class PageDefTests {

      [TestMethod]
      public void PageDefCreateIsSuccessful() {
         var pg = new PageDef(1, 20);
         Assert.AreEqual(1, pg.PageNum, "Page not 1");
         Assert.AreEqual(20, pg.NumItemsPerPage, "# of Page items not 20");
         Assert.AreEqual(0, pg.SkipCount, "skip count not 0");
      }

      [TestMethod]
      public void PageDefCalcsNumPagesCorrectly() {
         var pg = new PageDef(1, 20);
         Assert.AreEqual(1, pg.CalcTotPages(20), "calc of 20 isn't 1");
         Assert.AreEqual(2, pg.CalcTotPages(21), "calc of 21 isn't 2");
         Assert.AreEqual(1, pg.CalcTotPages(1), "calc of 1 isn't 1");
         Assert.AreEqual(1, pg.CalcTotPages(0), "calc of 0 isn't 1");
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException), "didn't raise exception")]
      public void PageDefCreateWithNegativePageNumRaisesException() {
         var pg = new PageDef(-1);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException), "didn't raise exception")]
      public void PageDefCreateWithZeroPageNumRaisesException() {
         var pg = new PageDef(0);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException), "didn't raise exception")]
      public void PageDefCreateWithNegativeItemsPPRaisesException() {
         var pg = new PageDef(1, -1);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException), "didn't raise exception")]
      public void PageDefCreateWithZeroItemsPPRaisesException() {
         var pg = new PageDef(1, 0);
      }

   }
#endregion

#region PageSelectOption Tests
   [TestClass]
   public class PageSelectOptionTests {

      [TestMethod]
      public void PageSelectBuilderOneOfOneAndNoBloomSucceeds() {
         var opts = PageSelectOption.BuildOpts(new PageResultStats(1, 1), 0, null).ToList();
         Assert.AreEqual(1, opts.Count, "Count not 1");
         Assert.IsTrue(opts[0].IsCurrent, "opt[0] not current");
         Assert.AreEqual(1, opts[0].PageNum, "opt[0] page not 1");
      }

      [TestMethod]
      public void PageSelectBuilderOneOfTwoAndNoBloomSucceeds() {
         var opts = PageSelectOption.BuildOpts(new PageResultStats(1, 2), 0, null).ToList();
         Assert.AreEqual(2, opts.Count, "Count not 2");
         Assert.IsTrue(opts[0].IsCurrent, "opt[0] is not current");
         Assert.AreEqual(1, opts[0].PageNum, "opt[0] page not 1");
         Assert.IsFalse(opts[1].IsCurrent, "opt[1] is current");
         Assert.AreEqual(2, opts[1].PageNum, "opt[1] page not 2");
      }

      [TestMethod]
      public void PageSelectBuilderTwoOfTwoAndNoBloomSucceeds() {
         var opts = PageSelectOption.BuildOpts(new PageResultStats(2, 2), 0, null).ToList();
         Assert.AreEqual(2, opts.Count, "Count not 2");
         Assert.IsFalse(opts[0].IsCurrent, "opt[0] is current");
         Assert.AreEqual(1, opts[0].PageNum, "opt[0] page not 1");
         Assert.IsTrue(opts[1].IsCurrent, "opt[1] is not current");
         Assert.AreEqual(2, opts[1].PageNum, "opt[1] page not 2");
      }

      [TestMethod]
      public void PageSelectBuilderOneOfOneWithBloom1Succeeds() {
         var opts = PageSelectOption.BuildOpts(new PageResultStats(1, 1), 1, null).ToList();
         Assert.AreEqual(1, opts.Count, "Count not 1");
         Assert.IsTrue(opts[0].IsCurrent, "opt[0] not current");
         Assert.AreEqual(1, opts[0].PageNum, "opt[0] page not 1");
      }

      [TestMethod]
      public void PageSelectBuilderOneOfTwoWithBloom1Succeeds() {
         var opts = PageSelectOption.BuildOpts(new PageResultStats(1, 2), 1, null).ToList();
         Assert.AreEqual(2, opts.Count, "Count not 2");
         Assert.IsTrue(opts[0].IsCurrent, "opt[0] is not current");
         Assert.AreEqual(1, opts[0].PageNum, "opt[0] page not 1");
         Assert.IsFalse(opts[1].IsCurrent, "opt[1] is current");
         Assert.AreEqual(2, opts[1].PageNum, "opt[1] page not 2");
      }

      [TestMethod]
      public void PageSelectBuilderTwoOfTwoWithBloom1Succeeds() {
         var opts = PageSelectOption.BuildOpts(new PageResultStats(2, 2), 1, null).ToList();
         Assert.AreEqual(2, opts.Count, "Count not 2");
         Assert.IsFalse(opts[0].IsCurrent, "opt[0] is current");
         Assert.AreEqual(1, opts[0].PageNum, "opt[0] page not 1");
         Assert.IsTrue(opts[1].IsCurrent, "opt[1] is not current");
         Assert.AreEqual(2, opts[1].PageNum, "opt[1] page not 2");
      }

      [TestMethod]
      public void PageSelectBuilderFiveofTenWithNoBloomSucceeds() {
         var opts = PageSelectOption.BuildOpts(new PageResultStats(5, 10), 0, null).ToList();
         Assert.AreEqual(3, opts.Count, "Count not 3");
         Assert.IsFalse(opts[0].IsCurrent, "opt[0] is current");
         Assert.AreEqual(1, opts[0].PageNum, "opt[0] page not 1");
         Assert.IsTrue(opts[1].IsCurrent, "opt[1] is not current");
         Assert.AreEqual(5, opts[1].PageNum, "opt[1] page not 5");
         Assert.IsFalse(opts[2].IsCurrent, "opt[2] is current");
         Assert.AreEqual(10, opts[2].PageNum, "opt[2] page not 10");
      }

      [TestMethod]
      public void PageSelectBuilderFiveofTenWithBloom1Succeeds() {
         var opts = PageSelectOption.BuildOpts(new PageResultStats(5, 10), 1, null).ToList();
         Assert.AreEqual(5, opts.Count, "Count not 5");

         Assert.IsFalse(opts[0].IsCurrent, "opt[0] is current");
         Assert.AreEqual(1, opts[0].PageNum, "opt[0] page not 1");

         Assert.IsFalse(opts[1].IsCurrent, "opt[1] is current");
         Assert.AreEqual(4, opts[1].PageNum, "opt[1] page not 4");

         Assert.IsTrue(opts[2].IsCurrent, "opt[2] is not current");
         Assert.AreEqual(5, opts[2].PageNum, "opt[2] page not 5");

         Assert.IsFalse(opts[3].IsCurrent, "opt[3] is current");
         Assert.AreEqual(6, opts[3].PageNum, "opt[3] page not 6");

         Assert.IsFalse(opts[4].IsCurrent, "opt[4] is current");
         Assert.AreEqual(10, opts[4].PageNum, "opt[4] page not 10");

      }

   }
   #endregion


#region PageDef Tests
   // This actually tests PageResultStats indirectly
   [TestClass]
   public class PageOfResultsTests {

      [TestMethod]
      [ExpectedException(typeof(ArgumentException), "didn't raise exception")]
      public void PageOfResultsWithNegativePageNumRaisesException() {
         var pg = new PageOfResults<int>(null, -1, 10);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException), "didn't raise exception")]
      public void PageOfResultsWithZeroPageNumRaisesException() {
         var pg = new PageOfResults<int>(null, 0, 10);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException), "didn't raise exception")]
      public void PageOfResultsWithNegativeNumPagesRaisesException() {
         var pg = new PageOfResults<int>(null, 1, -1);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException), "didn't raise exception")]
      public void PageOfResultsWithZeroNumPagesRaisesException() {
         var pg = new PageOfResults<int>(null, 1, 0);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException), "didn't raise exception")]
      public void PageOfResultsWithPageNumMoreThanNumPagesRaisesException() {
         var pg = new PageOfResults<int>(null, 11, 10);
      }

      [TestMethod]
      public void PageOfResultsWithNullItemsIsEmptyList() {
         var pg = new PageOfResults<int>(null, 1, 10);
         Assert.AreEqual(0, pg.Count);
      }

      [TestMethod]
      public void PageOfResultsWithItemsIsValidList() {
         var pg = new PageOfResults<int>(new int[] { 1, 2, 3, 4, 5 }.ToList(), 1, 10);
         Assert.AreEqual(5, pg.Count, "count of page items not 5");
         Assert.AreEqual(5, pg.PageItemCount, "PageItemCount not 5");
         Assert.AreEqual(1, pg.Stats.CurrPageNumber, "stats page is not 1");
         Assert.AreEqual(10, pg.Stats.NumPages, "stats page count is not 10");
         Assert.IsFalse(pg.Stats.HasPrevPage, "stats has prev is true");
         Assert.IsTrue(pg.Stats.HasNextPage, "stats has next is false");
         Assert.IsTrue(pg.Stats.HasMultiplePages, "stats has multiple is false");
      }

   }
#endregion

}
