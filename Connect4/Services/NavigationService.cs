using System;
using System.Windows.Controls;

namespace Connect4.Services
{
    /// <summary>
    /// Provides centralized navigation between <see cref="Page"/> instances
    /// using a shared <see cref="Frame"/>.
    /// </summary>
    public sealed class NavigationService
    {
        private readonly Frame _frame;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="frame">
        /// The <see cref="Frame"/> used as the navigation host.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="frame"/> is <c>null</c>.
        /// </exception>
        public NavigationService(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        /// <summary>
        /// Navigates the frame to the specified <see cref="Page"/> instance.
        /// Sets the page as the current content of the frame.
        /// </summary>
        /// <param name="page">
        /// The <see cref="Page"/> instance to navigate to. 
        /// The page should have its <see cref="FrameworkElement.DataContext"/> already assigned if needed.
        /// </param>
        public void Navigate(Page page)
        {
            _frame.Navigate(page);
        }

        /// <summary>
        /// Navigates back to the previous page if possible.
        /// </summary>
        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }
    }
}
