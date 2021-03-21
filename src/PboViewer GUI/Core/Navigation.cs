using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PboViewer.Core
{
    /// <summary>
    /// Simulate a navigation system
    /// </summary>
    public class Navigation
    {
        /// <summary>
        /// Main navigation session for the whole program
        /// </summary>
        public static Navigation NavigationSession;


        /// <summary>
        /// Current item at <see cref="Position"/>
        /// </summary>
        public object Item { get => _item; }
        /// <summary>
        /// Current position of the navigation, based on all the items in the navigation
        /// </summary>
        public int Position { get => _position; }


        private object _item;
        private int _position;
        private List<object> _navigationItems;


        public Navigation()
        {
            _position = -1;
            _navigationItems = new List<object>();
        }

        /// <summary>
        /// Navigate to a new navigation
        /// </summary>
        /// 
        /// <param name="navigationObject"></param>
        public void Navigate(object navigationObject)
        {
            _position++;
            _navigationItems.Insert(_position, navigationObject);

            if (_position != _navigationItems.Count - 1)
                _navigationItems.RemoveRange(_position + 1, _navigationItems.Count - _position - 1);

            _item = navigationObject;
        }

        /// <summary>
        /// Navigate through the itmes already inserted
        /// </summary>
        public void Navigate()
        {
            _position++;
            if (_position > _navigationItems.Count - 1) {
                _position--;
                return;
            }

            _item = _navigationItems[_position];
        }

        /// <summary>
        /// Navigate back from the position
        /// </summary>
        public void NavigateBack()
        {
            _position--;
            if (_position == -1) {
                _position++;
                return;
            }

            _item = _navigationItems[_position];
        }

        /// <summary>
        /// Navigate to a specific position
        /// </summary>
        /// 
        /// <param name="position">New position to navigate</param>
        public void NavigateTo(int position)
        {
            if (position > _navigationItems.Count)
                throw new Exception("The position is not valid");

            _position = position;
            _item = _navigationItems[_position];
        }

        /// <summary>
        /// Navigate to a specific object
        /// </summary>
        /// 
        /// <param name="navigationObject">New object to navigate</param>
        public void NavigateTo(object navigationObject)
        {
            int navigationItemsCount = _navigationItems.Count;

            for (int i = 0; i < navigationItemsCount; i++)
            {
                if (navigationObject.Equals(_navigationItems[i]))
                {
                    _position = i;
                    _item = navigationObject;
                    return;
                }
            }

            throw new Exception("The position is not valid");
        }
    }
}
