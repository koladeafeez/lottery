
using BedeLotteryConsole.Configurations;
using BedeLotteryConsole.Extensions;
using BedeLotteryConsole.Views;

namespace BedeLotteryConsole.Extensions
{
    public static class ViewConfiguration
    {
        public static IViewCollection ConfigureViews(
            this IViewCollection views,ViewBase view)
        {
           views.AddView(view);
            return views;
        }
    }
}
