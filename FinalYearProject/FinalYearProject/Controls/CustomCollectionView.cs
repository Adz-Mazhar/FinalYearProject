using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace FinalYearProject.Controls
{
    public class CustomCollectionView : CollectionView
    {
        public void ScrollToLast(bool animate)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (ItemsSource is not null && ItemsSource.Cast<object>().Count() > 0)
                {
                    if (IsGrouped)
                    {
                        var group = ItemsSource.Cast<IEnumerable<object>>().LastOrDefault();
                        if (group is not null)
                        {
                            var item = group.LastOrDefault();
                            if (item is not null)
                            {
                                ScrollTo(item, group, ScrollToPosition.End, animate);
                            }
                        }
                    }
                    else
                    {
                        var item = ItemsSource.Cast<object>().LastOrDefault();
                        if (item is not null)
                        {
                            ScrollTo(item, animate: animate);
                        }
                    }
                }
            });
        }
    }
}
