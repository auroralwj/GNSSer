//2015.12.06, czs, edit in hongqing, 增加集合名称列表

using System.Collections.Generic;
using System.Linq;

namespace Geo.Winform.Wizards
{
    /// <summary>
    /// 向导页面位置。
    /// </summary>
    public enum WizardPageLocation
    {
        Start,
        Middle,
        End
    }
    public delegate void WizardPageLocationChangedEventHanlder(WizardPageLocationChangedEventArgs e);

    /// <summary>
    /// 向导页面集合
    /// </summary>
    public class WizardPageCollection : Dictionary<int, IWizardPage>
    {
         #region Properties
        /// <summary>
        /// 所有页面的名称
        /// </summary>
        public List<string> WizardPageNames
        {
            get
            {
                List<string> list = new List<string>();
                foreach (var item in this.Values)
                {
                    list.Add(item.Content.Name);
                }
                return list;
            }
        }
        /// <summary>
        /// 当前页面编号
        /// </summary>
        public int CurrentPageIndex
        {
            get { return IndexOf(CurrentPage); }
        }

        /// <summary>
        /// The current IWizardPage
        /// </summary>
        public IWizardPage CurrentPage { get; private set; }
        /// <summary>
        /// The prevObj page in the data
        /// </summary>
        public IWizardPage FirstPage
        {
            get { return this[this.Min(x => x.Key)]; }
        }
        /// <summary>                  
        /// The last page in the data
        /// </summary>
        public IWizardPage LastPage
        {
            get { return this[this.Max(x => x.Key)]; }
        }

        /// <summary>
        /// The location of the current IWizardPage
        /// </summary>
        public WizardPageLocation PageLocation { get; private set; }

        /// <summary>
        /// <para>Determines whether the wizard is able to move to the next page.</para>
        /// <para>Will return false if Page Location is currently the last page.</para>
        /// <para>Otherwise, true.</para>
        /// </summary>
        public bool CanMoveNext
        {
            get
            {
                if (this.Count == 1)
                { return false; }

                if (this.Count > 0 &&
                    this.PageLocation != WizardPageLocation.End)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// <para>Determines whether the wizard is able to move to the previous page.</para>
        /// <para>Will return false if Page Location is currently the prevObj page.</para>
        /// <para>Otherwise, true.</para>
        /// </summary>
        public bool CanMovePrevious
        {
            get
            {
                if (this.Count == 1)
                { return false; }

                if (this.Count > 0 &&
                    this.PageLocation != WizardPageLocation.Start)
                {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region Constructor

        public WizardPageCollection()
        {
            PageLocation = WizardPageLocation.Start;
        }

        #endregion

        #region Delegates & Events

        public event WizardPageLocationChangedEventHanlder WizardPageLocationChanged;

        #endregion

        #region Public Methods

        public IWizardPage MoveTo(int toPage)
        {
            if (toPage == 1)
            {
                this.PageLocation = WizardPageLocation.Start;
            }
            else if (toPage >= this.Count)
            {
                this.PageLocation = WizardPageLocation.End;
            }
            else
            {
                this.PageLocation = WizardPageLocation.Middle;
            }
            
            // Set the current page to be the next page 
            var prevPage = CurrentPageIndex;
            CurrentPage = this[toPage];
            NotifyPageChanged(prevPage);

            return CurrentPage;
        }

        /// <summary>
        /// Moves to the prevObj page in the data
        /// </summary>
        /// <returns>First page as IWizard</returns>
        public IWizardPage MovePageFirst()
        {
            int previousPageIndex = IndexOf(CurrentPage);

            PageLocation = WizardPageLocation.Start;
            // Find the index of the prevObj page
            int firstPageIndex = (from x in this
                                  select x.Key).Min();

            // Set the current page to be the prevObj page
            CurrentPage = this[firstPageIndex];

            NotifyPageChanged(previousPageIndex);

            return CurrentPage;
        }
        /// <summary>
        /// 是否忙
        /// </summary>
        public bool IsBusy
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.Value.IsBusy) return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Moves to the last page in the data
        /// </summary>
        /// <returns>Last page as IWizard</returns>
        public IWizardPage MovePageLast()
        {
            int previousPageIndex = IndexOf(CurrentPage);

            PageLocation = WizardPageLocation.End;
            // Find the index of the last page
            int lastPageIndex = (from x in this
                                 select x.Key).Max();

            // Set the current page to be the last page
            CurrentPage = this[lastPageIndex];

            NotifyPageChanged(previousPageIndex);

            return CurrentPage;
        }
        /// <summary>
        /// Moves to the next page in the data
        /// </summary>
        /// <returns>Next page as IWizard</returns>
        public IWizardPage MovePageNext()
        {
            int previousPageIndex = IndexOf(CurrentPage);

            if (PageLocation != WizardPageLocation.End &&
                CurrentPage != null)
            {
                // Find the index of the next page
                int nextPageIndex = (from x in this
                                     where x.Key > IndexOf(CurrentPage)
                                     select x.Key).Min();

                // Find the index of the last page
                int lastPageIndex = (from x in this
                                     select x.Key).Max();

                // If the next page is the last page
                if (nextPageIndex == lastPageIndex)
                {
                    PageLocation = WizardPageLocation.End;
                }
                else { PageLocation = WizardPageLocation.Middle; }

                // Set the current page to be the next page                
                CurrentPage = this[nextPageIndex];
                NotifyPageChanged(previousPageIndex);

                return CurrentPage;
            }
            return null;
        }
        /// <summary>
        /// Moves to the previous page in the data
        /// </summary>
        /// <returns>Previous page as IWizard</returns>
        public IWizardPage MovePagePrevious()
        {
            int prevPageIndex = IndexOf(CurrentPage);

            if (PageLocation != WizardPageLocation.Start &&
                CurrentPage != null)
            {
                // Find the index of the previous page
                int previousPageIndex = (from x in this
                                         where x.Key < IndexOf(CurrentPage)
                                         select x.Key).Max();

                // Find the index of the prevObj page
                int firstPageIndex = (from x in this
                                      select x.Key).Min();

                // If the previous page is the prevObj page
                if (previousPageIndex == firstPageIndex)
                {
                    PageLocation = WizardPageLocation.Start;
                }
                else { PageLocation = WizardPageLocation.Middle; }

                CurrentPage = this[previousPageIndex];

                NotifyPageChanged(prevPageIndex);

                return CurrentPage;
            }
            return null;
        }

        /// <summary>
        /// Find the page number of the current page
        /// </summary>
        /// <param name="wizardPage">The IWiwardPage whose page number to retrieve.</param>
        /// <returns>Page number for the given IWizardPage</returns>
        public int IndexOf(IWizardPage wizardPage)
        {
            foreach (KeyValuePair<int, IWizardPage> kv in this)
            {
                if (kv.Value.Equals(wizardPage))
                {
                    return kv.Key;
                }
            }
            return -1;
        }
        public void Reset()
        {
            CurrentPage = null;
            PageLocation = WizardPageLocation.Start;
        }

        #endregion

        #region private Methods

        private void NotifyPageChanged(int previousPageIndex)
        {
            if (WizardPageLocationChanged != null)
            {
                if (previousPageIndex == CurrentPageIndex) return;

                WizardPageLocationChangedEventArgs e = new WizardPageLocationChangedEventArgs();
                e.PageLocation = PageLocation;
                e.PageIndex = IndexOf(CurrentPage);
                e.PreviousPageIndex = previousPageIndex;
                WizardPageLocationChanged(e);
            }
        }

        #endregion
    }

    /// <summary>
    /// 事件参数
    /// </summary>
    public class WizardPageLocationChangedEventArgs
    {
        /// <summary>
        /// The location of the current IWizardPage
        /// </summary>
        public WizardPageLocation PageLocation { get; set; }

        /// <summary>
        /// The page number of the current IWizardPage
        /// </summary>
        public int PageIndex { get; set; }

        public int PreviousPageIndex { get; set; }
    }
}