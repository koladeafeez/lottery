
using BedeLotteryConsole.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeLotteryConsole.Configurations
{
    public interface IViewCollection
    {
        void AddView<TView>(TView view) where TView : ViewBase;

        void RunAll();
        void Clear();
    }
}
