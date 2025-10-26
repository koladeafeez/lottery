

using BedeLotteryConsole.Views;

namespace BedeLotteryConsole.Configurations
{

    public class ViewCollection : IViewCollection
    {
     
        private readonly Dictionary<string, ViewBase> _views = new();
        private readonly List<string> _executionOrder = new();
        private int _autoIdCounter = 0;

        public void AddView<TView>(string id, TView view) where TView : ViewBase
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("View ID cannot be null or empty", nameof(id));

            if (view == null)
                throw new ArgumentNullException(nameof(view));


            _views[id] = view;
            _executionOrder.Add(id);
        }


        public void AddView<TView>(TView view) where TView : ViewBase
        {
            string id = $"{typeof(TView).Name}_{_autoIdCounter++}";
            AddView(id, view);
   
        }


        public void RunAll()
        {
            foreach (var id in _executionOrder)
            {
                _views[id].Run();
            }
        }

        public void Clear()
        {
            _views.Clear();
            _executionOrder.Clear();
            _autoIdCounter = 0;
        }
    }
}

