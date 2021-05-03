using FinalYearProject.ViewModels.Pages;
using System;
using Xamarin.Forms;

namespace FinalYearProject.Views
{
    public partial class GroupChatPage : ContentPage
    {
        public GroupChatPage()
        {
            InitializeComponent();

            var viewModel = BindingContext as GroupChatPageViewModel;

            viewModel.MessagesLoaded += ScrollToBottom;
        }

        private void ScrollToBottom(object sender, EventArgs e)
        {
            messageCollectionView.ScrollToLast(false);
        }
    }
}